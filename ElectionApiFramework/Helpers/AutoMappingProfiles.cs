using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ElectionApiFramework.Data;
using ElectionApiFramework.Models;

namespace ElectionApiFramework.Helpers
{
    public class  AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateMap<tblUser, User>().ReverseMap();
            CreateMap<tblAddress, Address>().ReverseMap();
            CreateMap<tblCandidate, Candidate>().ReverseMap();
            CreateMap<tblElection, Election>().ReverseMap();
            CreateMap<tblPerson, Person>().ReverseMap();
            CreateMap<tlkpConstituancy, Constituancy>().ReverseMap();
            CreateMap<tlkpParish, Parish>().ReverseMap();
            CreateMap<tlkpParty, Party>().ReverseMap();
            CreateMap<tlkpVillage, Village>().ReverseMap();
        }
    }
}
