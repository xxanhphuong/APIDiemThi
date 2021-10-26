using APIDiemThi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models
{
    public class Student
    {
        
        [Key]
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int TypeStudent { get; set; }

        public Users User { get; set; }

        [ForeignKey("Classes")]
        [Required]
        public int ClassesId { get; set; }
        public Classes Classes { get; set; }

        
        public ICollection<Score> Scores { get; set; }
    }
}
