using AutoMapper;
using ElectionApiFramework.Data;
using ElectionApiFramework.Interfaces;
using ElectionApiFramework.Models;
using ElectionApiFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace ElectionApiFramework.BLL
{
    public class ElectionBLL : IElection
    {
        private readonly ElectionEntities db =new ElectionEntities();
        private readonly Mapper _mapper;

        public ElectionBLL()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(AutoMappingProfiles));
            });
            _mapper = new Mapper(configuration);
        }
        
        public Boolean Login(int VotersID, string Pin)
        {
            try
            {
                var x = (from u in db.tblPersons
                         where u.PersonID == VotersID
                         where u.Pin == Pin
                         select u).FirstOrDefault();
                if (x is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Person GetPerson(int VotersID)
        {
            Person user = new Person();
            var x = (from u in db.tblPersons
                     where u.PersonID == VotersID
                     select u).FirstOrDefault();
            if (x != null)
            {
                var data = _mapper.Map<tblPerson, Person>(x);
                return data;
            }
            else
            {
                user.PersonId = 0;
            }
            return user;
        }
        public Boolean AddVotes(Election election)
        {
            try
            {
                var initialusercount = db.tblElections.Count();
                var data = _mapper.Map<Election, tblElection>(election);
                db.tblElections.Add(data);
                db.SaveChanges();
                var afterusercount = db.tblElections.Count();
                if (afterusercount > initialusercount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }

        }
        public Boolean LockOutVoter(int personID)
        {
            try
            {
                var dbPerson = db.tblPersons.Find(personID);
                dbPerson.vFlag = true;
                db.SaveChanges();
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }

        }
    }
}
