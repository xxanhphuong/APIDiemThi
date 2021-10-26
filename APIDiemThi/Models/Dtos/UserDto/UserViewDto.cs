using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.UserDto
{
    public class UserViewDto
    {
        [Required]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

    }
}
