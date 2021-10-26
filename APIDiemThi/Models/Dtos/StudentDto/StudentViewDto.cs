using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.StudentDto
{
    public class StudentViewDto
    {
        public int StudentId { get; set; }

        public int TypeStudent { get; set; }

        public int ClassesId { get; set; }

        public UserDto.UserViewDto User { get; set; }
    }
}
