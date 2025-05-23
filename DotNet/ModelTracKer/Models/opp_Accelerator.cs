using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("opp_accelerator")]
    public class opp_Accelerator
    {
        [Key]
        public int Id { get; set; }

        public int TrackerId { get; set; }

        [ForeignKey("TrackerId")]
        public Tracker trackers { get; set; }


        public int AcceleratorId { get; set; }

        [ForeignKey("AcceleratorId")]
        public Accelerator accelerators { get; set; }
    }
}
