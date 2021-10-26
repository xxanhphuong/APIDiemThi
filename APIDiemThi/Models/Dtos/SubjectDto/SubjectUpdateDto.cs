using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.SubjectDto
{
    public class SubjectUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Credit { get; set; }
        [Required]
        public double MidScoreRatio { get; set; }
        [Required]
        public double FinalScoreRatio { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
