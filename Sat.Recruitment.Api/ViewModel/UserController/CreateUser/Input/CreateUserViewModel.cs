using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.ViewModel.UserController.CreateUser.Input
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "The address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "The phone is required")]
        public string Phone { get; set; }
        public string UserType { get; set; }
        public decimal Money { get; set; }
    }
}
