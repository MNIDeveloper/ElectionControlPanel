using ElectionApiFramework.BLL;
using ElectionApiFramework.Interfaces;
using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ElectionApiFramework.Controllers
{
    
    public class CandidateController : ApiController
    {
        private AdminBLL admin = new AdminBLL();

        public Boolean AddCandidate(Candidate candidate)
        {
            return admin.AddCandidate(candidate);
        }
       
        public Boolean DeleteCandidate(int CandidateID)
        {
            return admin.DeleteCandidate(CandidateID);
        }
 
        public Boolean UpdateCandidate(Candidate candidate)
        {
            return admin.UpdateCandidate(candidate);
        }
       
        //public IHttpActionResult GetCandidates()
        //{
        //    var candidates = admin.GetCandidatesForElection();
         //   if(candidates == null)
         //   {
          //      return NotFound();
           // } 
           // return Ok(candidates);
        //}

        public IHttpActionResult GetDisplayCandidates()
        {
            var candidates = admin.GetCandidatesDisplay(); 
            if (candidates == null)
            {
                return NotFound();
            }
            return Ok(candidates);
        }
    }
}
