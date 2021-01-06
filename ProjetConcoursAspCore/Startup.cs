using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ProjetConcoursAspCore.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetConcoursAspCore.Areas.Admin.Repository;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjetConcoursAspCore.Repositories;
using Confluent.Kafka;
using ProjetConcoursAspCore.Kafka;
using HostedServices = Microsoft.Extensions.Hosting;

namespace ProjetConcoursAspCore
{
	public class Startup
	{
		public static String AuthSchema = "authSchema";
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddTransient<IEtudiant3aRepository, Etudiant3aRepository>();
			services.AddTransient<Areas.Admin.Repository.IFiliereRepository, Areas.Admin.Repository.FiliereRepository>();
			services.AddTransient<IEtudiant4aRepository, Etudiant4aRepository>();
			services.AddTransient<IStudent, StudentRepository>();
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(10);
				options.Cookie.HttpOnly = false;
				options.Cookie.IsEssential = true;
			});
			ConfigureKafka(services);
			//Auth
			this.ConfigureAuth(services);

			services.ConfigureApplicationCookie(config =>
			{
				config.Cookie.Name = "auth";
				config.LoginPath = "/home/inscription";
			});


			services.AddRazorPages();
			services.AddMvc(option => option.EnableEndpointRouting = false);
			services.AddMemoryCache();

		}

		public void ConfigureAuth(IServiceCollection services)
		{
			services.AddDefaultIdentity<IdentityUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount = true;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.AddAuthorization(options =>
			{
				options.AddPolicy("AdminPolicy", policy =>
			            policy.RequireClaim("IsAdmin", "true"));
				options.AddPolicy("MemberPolicy", policy =>
						policy.RequireClaim("IsMember", "true"));
			});
			services.AddAuthentication(options =>
			{
				options.DefaultChallengeScheme = AuthSchema;
				options.DefaultAuthenticateScheme = AuthSchema;
				options.DefaultScheme = AuthSchema;
			}) // Sets the default scheme to cookies
				.AddCookie(AuthSchema, options =>
				{
					options.AccessDeniedPath = "/auth/denied";
					options.LoginPath = "/auth";
				});

			//services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureMyCookie>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		private void ConfigureKafka(IServiceCollection services)
		{
			var producerConfig = new ProducerConfig();
			Configuration.Bind("producer", producerConfig);
			services.AddSingleton<ProducerConfig>(producerConfig);
			var producerWrapper = new ProducerWrapper(producerConfig, "email");
			services.AddSingleton<ProducerWrapper>(producerWrapper);
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseSession();
			app.UseAuthentication();
			app.UseAuthorization();

			//app.UseEndpoints(endpoints =>
			//{
			//    endpoints.MapControllerRoute(
			//        name: "default",
			//        pattern: "{controller=Home}/{action=Index}/{id?}");
			//    endpoints.MapRazorPages();
			//});
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "Admin",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
