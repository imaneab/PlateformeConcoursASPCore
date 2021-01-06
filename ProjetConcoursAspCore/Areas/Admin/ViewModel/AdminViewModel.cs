using System;
namespace ProjetConcoursAspCore.Areas.Admin.ViewModel
{
    public class AdminViewModel
    {
		public String Message { get; set; }
		public String Login { get; set; }
		public String Password { get; set; }

		public AdminViewModel(string message)
		{
			Message = message;
		}

		public AdminViewModel(string message, string Login) : this(message)
		{
			this.Login = Login;
			
		}
	}
}
