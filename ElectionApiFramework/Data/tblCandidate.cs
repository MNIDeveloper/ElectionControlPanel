//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectionApiFramework.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblCandidate
    {
        public int CandidateID { get; set; }
        public int PersonID { get; set; }
        public int PartyID { get; set; }
        public int Constituancy { get; set; }
        public string CandidateImage { get; set; }
        public string CandidateImageWeb { get; set; }
        public bool IsCurrent { get; set; }
    }
}
