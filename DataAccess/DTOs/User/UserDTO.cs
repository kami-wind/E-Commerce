using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.User;

public class UserDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}
