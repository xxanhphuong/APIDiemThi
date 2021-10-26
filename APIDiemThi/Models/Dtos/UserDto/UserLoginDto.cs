using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.UserDto
{
    public class UserLoginDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
    }
}
