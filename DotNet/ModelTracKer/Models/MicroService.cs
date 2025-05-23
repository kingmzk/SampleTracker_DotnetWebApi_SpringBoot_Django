using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("microservice")]
    public class MicroService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<opp_microservice> OppMicroservice { get; set; }
    }
}
