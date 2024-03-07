using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionApiFramework.ViewModels
{
    public class CandidateDetailsVM
    {
        public List<CandidateDisplay> candidates = new List<CandidateDisplay>();
        public CandidateDFED candidateEdit {  get; set; } = new CandidateDFED();
        public CandidateDFED candidateDelete { get; set; } = new CandidateDFED();

        public List<Party> parties = new List<Party>();

        public List<Constituancy> constituencies = new List<Constituancy>();
    }
}
