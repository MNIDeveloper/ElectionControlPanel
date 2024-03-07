namespace ElectionApiFramework.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }

        public int PersonId { get; set; }

        public int PartyId { get; set; }

        public int Constituancy { get; set; }

        public string CandidateImage { get; set; }
        public string CandidateImageWeb { get; set; }

        public bool IsCurrent { get; set; }
    }
}
