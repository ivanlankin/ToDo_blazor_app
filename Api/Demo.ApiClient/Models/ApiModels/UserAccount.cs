using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Demo.ApiClient.Models.ApiModels
{
    public class UserAccount
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пожалуйста введите логин")]
        public string? UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пожалуйста введите пароль")]
        public string? Password { get; set; }
    }
}
