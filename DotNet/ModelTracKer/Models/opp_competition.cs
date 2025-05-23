using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("opp_competition")]
    public class opp_competition
    {
        [Key]
        public int Id { get; set; }

        public int TrackerId { get; set; }

        [ForeignKey("TrackerId")]
        public Tracker tracker { get; set; }

        public int CompetitionId { get; set; }

        [ForeignKey("CompetitionId")]
        public Competition competition { get; set; }
    }
}
