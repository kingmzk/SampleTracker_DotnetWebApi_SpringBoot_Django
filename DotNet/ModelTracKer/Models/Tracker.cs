using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTracKer.Models
{
    [Table("tracker")]
    public class Tracker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Tracker_id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Tracker_Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Client_Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public double Investment { get; set; }

        [Required]
        public bool GenAiAdoptation {  get; set; }


        public int GenAiTool_Id { get; set; }

        [ForeignKey("GenAiTool_Id")]
        public GenAiTool? GenAiTool { get; set; }


        public int ReasonForNoGenAiAdoptation_Id { get; set; }

        [ForeignKey("ReasonForNoGenAiAdoptation_Id")]
        public ReasonForNoGenAiAdoptation? ReasonForNoGenAiAdoptation { get; set; }


        public ICollection<opp_Accelerator> oppAccelerators { get; set; }

        public ICollection<opp_microservice> oppMicroservices { get; set; }

        public ICollection<opp_competition> oppCompetitions { get; set; }
    }
}
