namespace ModelTracKer.Dto
{
    public class TrackerOutputDto
    {
        public int Id { get; set; }
        public int Tracker_id { get; set; }
        public string Tracker_Name { get; set; }
        public string Client_Name { get; set; }
        public double Investment { get; set; }
        public bool GenAiAdoptation { get; set; }

        public string GenAiToolName { get; set; }
        public string ReasonForNoGenAiAdoptationName { get; set; }

        public List<string> OppAccelerators { get; set; }
        public List<string> OppMicroservices { get; set; }
        public List<string> OppCompetitions { get; set; }
    }
}
