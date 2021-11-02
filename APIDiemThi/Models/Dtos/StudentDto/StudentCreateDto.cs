using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.StudentDto
{
    public class StudentCreateDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int TypeStudent { get; set; }
        public int ClassesId { get; set; }
    }
}
