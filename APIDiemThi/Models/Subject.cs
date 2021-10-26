using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Credit { get; set; }
        [Required]
        public double MidScoreRatio { get; set; }
        [Required]
        public double FinalScoreRatio { get; set; }
        

        [ForeignKey("Teacher")]
        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        
        public ICollection<Score> Scores { get; set; }
    }
}
