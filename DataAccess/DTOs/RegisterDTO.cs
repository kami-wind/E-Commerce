using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs;

public class RegisterDTO
{
    //[Required(ErrorMessage = "Username is required")]
    //public string Username { get; set; }

    //[Required(ErrorMessage = "Email is required")]
    //[EmailAddress]
    //public string Email { get; set; }

    //[Required(ErrorMessage = "Password is required")]
    //[DataType(DataType.Password)]
    //public string Password { get; set; }
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
