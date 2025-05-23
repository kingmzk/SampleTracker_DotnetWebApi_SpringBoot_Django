using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("opp_microservice")]
    public class opp_microservice
    {
        [Key]
        public int Id { get; set; }

        public int TrackerId { get; set; }

        [ForeignKey("TrackerId")]
        public Tracker trackers { get; set; }

        public int MicroserviceId { get; set; }

        [ForeignKey("MicroserviceId")]
        public MicroService microservice { get; set; }
    }
}
