using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.ScoreDto
{
    public class ScoreUpdateDto
    {
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }

        public double? MidScore { get; set; }
        public double? FinalScore { get; set; }
    }
}
