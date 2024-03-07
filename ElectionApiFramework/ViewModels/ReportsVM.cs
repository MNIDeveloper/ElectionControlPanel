using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionApiFramework.ViewModels
{
    public class ReportsVM
    {
        public int TotalVotes { get; set; } = 0;        
        public decimal TotalPercentage { get; set; } = 0;
        public List<ElectionResult> Results = new List<ElectionResult>();
        public List<PartyByVillages> ByVillageResults = new List<PartyByVillages>();
        public ElectionSummary Summary { get; set; }
        public decimal VoterTurnOutPercentage { get; set; } 
        public decimal Candidate1Percentage { get; set; }

        public decimal Candidate2Percentage { get;set; }
        public decimal Candidate3Percentage { get; set; }
        public decimal Candidate4Percentage { get; set; }
        public decimal Candidate5Percentage { get; set; }
        public decimal Candidate6Percentage { get; set; }
        public decimal Candidate7Percentage { get; set; }
        public decimal Candidate8Percentage { get; set; }
        public decimal Candidate9Percentage { get; set; }
    }
}
