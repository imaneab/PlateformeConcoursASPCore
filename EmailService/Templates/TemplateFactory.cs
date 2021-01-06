using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Templates
{
	public class TemplateFactory
	{
		private IRazorViewToStringRenderer render;

		public TemplateFactory(IRazorViewToStringRenderer razor)
		{
			this.render = razor;
		}
		public IEmailTemplate GetTempalte(string type,dynamic json)
		{
			if(type.ToLower().Equals("signup"))
			{
				return new SignupTemplate(render, json);
			}
			else if (type.ToLower().Equals("accepted"))
			{
				return new AcceptedTemplate(render, json);
			}
			else if (type.ToLower().Equals("preselection"))
			{
				return new PreselectionTemplate(render, json);
			}
			else
			{
				throw new Exception("Type not defined");
			}
		}
	}
}
