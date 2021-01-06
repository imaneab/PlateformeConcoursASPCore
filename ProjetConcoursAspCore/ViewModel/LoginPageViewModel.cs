using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.ViewModel
{
	public class LoginPageViewModel
	{
		public String Message { get; set; }
		public String CNE{ get; set; }
		public String CIN { get; set; }
		
		public LoginPageViewModel(string message)
		{
			Message = message;
		}

		public LoginPageViewModel(string message, string cNE, string cIN) : this(message)
		{
			CNE = cNE;
			CIN = cIN;
		}
	}
}
