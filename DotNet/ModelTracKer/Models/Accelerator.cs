using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("acclerator")]
    public class Accelerator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<opp_Accelerator> OppAccelerators { get; set; }
    }
}
