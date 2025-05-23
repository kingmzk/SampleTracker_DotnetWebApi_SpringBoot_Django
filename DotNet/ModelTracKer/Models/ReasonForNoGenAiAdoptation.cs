using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{

    [Table("ReasonForNoGenAiAdoptation")]
    public class ReasonForNoGenAiAdoptation
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string? Name { get; set; }

        public bool? isDisabled { get; set; } = false;

        public ICollection<Tracker> Trackers { get; set; }
    }
}
