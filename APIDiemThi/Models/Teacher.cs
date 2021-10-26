using APIDiemThi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models
{
    public class Teacher
    {
        [Key]
        [Required]
        public int TeacherId { get; set; }
        public double Salary { get; set; }


        public Users User { get; set; }
        
        public ICollection<Subject> Subjects { get; set; }
    }
}
