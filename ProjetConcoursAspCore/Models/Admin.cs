using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetConcoursAspCore.Models
{
    public class Admin
    {
        public Admin()
        {
        }
        public int ID { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
