using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("GenAiTool")]
    public class GenAiTool
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        public bool? isDisabled { get; set; } = false;

        public ICollection<Tracker> trackers { get; set; }
    }
}
