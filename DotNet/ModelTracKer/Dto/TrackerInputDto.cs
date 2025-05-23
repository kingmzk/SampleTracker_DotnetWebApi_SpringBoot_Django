namespace ModelTracKer.Dto
{
    public class TrackerInputDto
    {
        public int Tracker_id { get; set; }
        public string Tracker_Name { get; set; }
        public string Client_Name { get; set; }
        public double Investment { get; set; }
        public bool GenAiAdoptation { get; set; }
        public int GenAiTool_Id { get; set; }
        public int ReasonForNoGenAiAdoptation_Id { get; set; }

        public List<int> OppAcceleratorIds { get; set; }
        public List<int> OppMicroserviceIds { get; set; }
        public List<int> OppCompetitionIds { get; set; }
    }
}
