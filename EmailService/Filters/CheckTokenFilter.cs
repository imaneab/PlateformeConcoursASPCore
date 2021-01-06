using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Filters
{
	public class CheckTokenFilter : Attribute, IAsyncActionFilter
	{
		public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var headers = context.HttpContext.Request.Headers;
			if (!headers.ContainsKey("token") || !headers["token"].Equals(Config.Config.Token))
				throw new UnauthorizedAccessException("Token not valid");
			return next();
		}
	}
}
