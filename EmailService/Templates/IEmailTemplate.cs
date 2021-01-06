using EmailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Templates
{
	public interface IEmailTemplate
	{
		string GetView();

		string GetTitle();
		Task<string> Render(int user_id, ApplicationContext db, object model);
		Object DbProcess(ApplicationContext db,Object model);
	}
}
