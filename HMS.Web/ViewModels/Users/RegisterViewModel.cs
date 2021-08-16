using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.ViewModels.Users
{
    public class RegisterViewModel
    {
        public string Flag { get; set; }
        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Введите город")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Введите адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Выберите пол")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите проверочный пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}
