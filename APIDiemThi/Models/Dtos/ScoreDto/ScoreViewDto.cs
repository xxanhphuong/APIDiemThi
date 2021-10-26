using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models.Dtos.ScoreDto
{
    public class ScoreViewDto
    {
        public int StudentId { get; set; }
        public SubjectDto.SubjectViewDto Subject { get; set; }

        public double? MidScore { get; set; }
        public double? FinalScore { get; set; }
    }
}
