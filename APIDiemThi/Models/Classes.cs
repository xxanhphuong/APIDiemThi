using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Models
{
    public class Classes
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        

        public ICollection<Student> Students { get; set; }

        [ForeignKey("Major")]
        [Required]
        public int MajorId { get; set; }
        public Major Major { get; set; }
    }
}
