using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace APIDiemThi.Models.Dtos.ClassesDto
{
    public class ClassesViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public MajorDto.MajorViewDto Major { get; set; }
    }
}
