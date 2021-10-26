using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models
{
    public class Score
    {
        [Required]
        public int StudentId { get; set; }
        
        public Student Student { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public  Subject Subject { get; set; }

        public double? MidScore { get; set; }
        public double? FinalScore { get; set; }
    }
}
