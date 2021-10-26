using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.TeacherDto
{
    public class TeacherUpdateDto
    {
        public int TeacherId { get; set; }
        public double Salary { get; set; }
    }
}
