using AutoMapper;
using ElectionApiFramework.Data;
using ElectionApiFramework.Helpers;
using ElectionApiFramework.Interfaces;
using ElectionApiFramework.Models;
using ElectionApiFramework.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace ElectionApiFramework.BLL
{
    public class AdminBLL :IAdministrative
    {
        private readonly ElectionEntities db = new ElectionEntities();
        private readonly Mapper _mapper;

        public AdminBLL()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(AutoMappingProfiles));
            });
            _mapper = new Mapper(configuration);
        }

        #region User
        public Boolean UserLogin(string Username, string Password)
        {
            try
            {
                var x = (from u in db.tblUsers
                         where u.Email == Username
                         where u.Password == Password
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }

        public User GetUser(string Username)
        {
            User user = new User();
            var x = (from u in db.tblUsers
                     where u.Email == Username
                     select u).FirstOrDefault();
            if (x != null)
            {
                var data = _mapper.Map<tblUser, User>(x);
                return data;
            }
            else
            {
                user.UserId = 0;
            }
            return user;
        }
        public Boolean AddUser(User user)
        {
            try
            {
                var initialusercount = db.tblUsers.Count();
                var data = _mapper.Map<User, tblUser>(user);
                db.tblUsers.Add(data);
                db.SaveChanges();
                var afterusercount = db.tblUsers.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean DeleteUser(int userID)
        {
            try
            {
                var initialusercount = db.tblUsers.Count();
                var data = db.tblUsers.Find(userID);
                if (data != null)
                {
                    db.tblUsers.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tblUsers.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateUser(User user)
        {
            try
            {
                var dbUser = db.tblUsers.Find(user.UserId);
                dbUser.Fname = user.Fname.Trim(); 
                dbUser.Lname = user.Lname.Trim();
                dbUser.Email = user.Email.Trim();
                dbUser.Password = user.Password.Trim();
                dbUser.Position = (int)user.Position;
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
        public List<User> GetUsers()
        {
            List<User> Users = new List<User>();
            var users = db.tblUsers.ToList();
            foreach (var user in users)
            {
                var data = _mapper.Map<tblUser, User>(user);
                Users.Add(data);
            }
            return Users;
        }
        public User GetUserById(int id)
        {
            User user = new User();
            try
            {
                var dbUser = db.tblUsers.Find(id);
                user = _mapper.Map<tblUser, User>(dbUser);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return user;
                }
            }
            catch (NullReferenceException)
            {
                return user;
            }
            catch (InvalidOperationException)
            {
                return user;
            }
            catch (DbEntityValidationException)
            {
                return user;
            }

        }

        #endregion
        #region Village
        public Boolean AddVillage(Village village)
        {
            try
            {
                var initialusercount = db.tlkpVillages.Count();
                var data = _mapper.Map<Village, tlkpVillage>(village);
                db.tlkpVillages.Add(data);
                db.SaveChanges();
                var afterusercount = db.tlkpVillages.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean DeleteVillage(int villageID)
        {
            try
            {
                var initialusercount = db.tlkpVillages.Count();
                var data = db.tlkpVillages.Find(villageID);
                if (data != null)
                {
                    db.tlkpVillages.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tlkpVillages.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateVillage(Village village)
        {
            try
            {
                var dbVillage = db.tlkpVillages.Find(village.VillageId);
                dbVillage.VillageName = village.VillageName.Trim();
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
        public List<Village> GetVillages()
        {
            List<Village> Villages = new List<Village>();
            var villages = db.tlkpVillages.ToList();
            foreach (var village in villages)
            {
                var data = _mapper.Map<tlkpVillage, Village>(village);
                Villages.Add(data);
            }
            return Villages;
        }
        public Village GetVillageById(int id) 
        {
            Village village = new Village();
            try
            {
                var dbVillage = db.tlkpVillages.Find(id);
                village = _mapper.Map<tlkpVillage, Village>(dbVillage);
                if (village != null)
                {
                    return village;
                }
                else
                {
                    return village;
                }
            }
            catch (NullReferenceException) 
            { 
                return village;
            }catch (InvalidOperationException)
            {
                return village;
            }
            catch (DbEntityValidationException)
            {
                return village;
            }

        }
        public Boolean VillageExist(string villageName)
        {
            try
            {
                var x = (from u in db.tlkpVillages
                         where u.VillageName == villageName                        
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        #endregion
        #region Constituancy
        public Boolean AddConstituancy(Constituancy constituancy)
        {
            try
            {
                var initialusercount = db.tlkpConstituancies.Count();
                var data = _mapper.Map<Constituancy, tlkpConstituancy>(constituancy);
                db.tlkpConstituancies.Add(data);
                db.SaveChanges();
                var afterusercount = db.tlkpConstituancies.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean DeleteConstituancy(int constituancyID)
        {
            try
            {
                var initialusercount = db.tlkpConstituancies.Count();
                var data = db.tlkpConstituancies.Find(constituancyID);
                if (data != null)
                {
                    db.tlkpConstituancies.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tlkpConstituancies.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateConstituancy(Constituancy constituancy)
        {
            try
            {
                var dbConstituancy = db.tlkpConstituancies.Find(constituancy.ConstituancyId);
                dbConstituancy.ConstituancyName = constituancy.ConstituancyName.Trim();
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
        public List<Constituancy> GetConstituancies()
        {
            List<Constituancy> Constituancies = new List<Constituancy>();
            var constituancies = db.tlkpConstituancies.ToList();
            foreach (var constituancy in constituancies)
            {
                var data = _mapper.Map<tlkpConstituancy, Constituancy>(constituancy);
                Constituancies.Add(data);
            }
            return Constituancies;
        }
        public Constituancy GetConstituancyById(int id)
        {
            Constituancy constituancy = new Constituancy();
            try
            {
                var dbConstituancy = db.tlkpConstituancies.Find(id);
                constituancy = _mapper.Map<tlkpConstituancy, Constituancy>(dbConstituancy);
                if (constituancy != null)
                {
                    return constituancy;
                }
                else
                {
                    return constituancy;
                }
            }
            catch (NullReferenceException)
            {
                return constituancy;
            }
            catch (InvalidOperationException)
            {
                return constituancy;
            }
            catch (DbEntityValidationException)
            {
                return constituancy;
            }

        }
        public Boolean ConstituancyExist(string constituancyName)
        {
            try
            {
                var x = (from u in db.tlkpConstituancies
                         where u.ConstituancyName == constituancyName
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        #endregion
        #region Address
        public Boolean AddAddress(Address address)
        {
            try
            {
                var initialusercount = db.tblAddresses.Count();
                var data = _mapper.Map<Address, tblAddress>(address);
                db.tblAddresses.Add(data);
                db.SaveChanges();               
                var afterusercount = db.tblAddresses.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public PersonResult AddAddressLocal(Address address)
        {
            PersonResult result = new PersonResult();
            try
            {
                var initialusercount = db.tblAddresses.Count();
                var data = _mapper.Map<Address, tblAddress>(address);
                db.tblAddresses.Add(data);
                db.SaveChanges();
                var id = data.AddressID;
                var afterusercount = db.tblAddresses.Count();
                if (afterusercount > initialusercount)
                {
                    result.Id = id;
                    result.Result = true;
                    return result;
                }
                else
                {
                    result.Id = 0;
                    result.Result = false;
                    return result;
                }
            }
            catch (NullReferenceException)
            {
                result.Id = 0;
                result.Result = false;
                return result;
            }
            catch (InvalidOperationException)
            {
                result.Id = 0;
                result.Result = false;
                return result;
            }
            catch (DbEntityValidationException)
            {
                result.Id = 0;
                result.Result = false;
                return result;
            }
        }
        public Boolean DeleteAddress(int addressID)
        {
            try
            {
                var initialusercount = db.tblAddresses.Count();
                var data = db.tblAddresses.Find(addressID);
                if (data != null)
                {
                    db.tblAddresses.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tblAddresses.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateAddress(Address address)
        {
            try
            {
                var dbAddress = db.tblAddresses.Find(address.AddressId);
                dbAddress.AddressID = address.AddressId;
                dbAddress.Street = address.Street.Trim();
                dbAddress.Village = address.Village;
                dbAddress.Parish = address.Parish;
                dbAddress.Postcode = address.Postcode.Trim();
                dbAddress.Constituancy = address.Constituancy;
                dbAddress.IsCurrent = address.IsCurrent;
                dbAddress.PersonID = address.PersonId;
                dbAddress.DateMoved = address.DateMoved;
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
        public List<Address> GetAddresses(Person person)
        {
            List<Address> Addresses = new List<Address>();
            var addresses = (from a in db.tblAddresses
                             where a.PersonID == person.PersonId
                             select a).ToList();
            foreach (var address in addresses)
            {
                var data = _mapper.Map<tblAddress, Address>(address);
                Addresses.Add(data);
            }
            return Addresses;
        }
        public Address GetAddressById(int id)
        {
            Address address = new Address();
            try
            {
                var dbAddress = db.tblAddresses.Find(id);
                address = _mapper.Map<tblAddress, Address>(dbAddress);
                if (address != null)
                {
                    return address;
                }
                else
                {
                    return address;
                }
            }
            catch (NullReferenceException)
            {
                return address;
            }
            catch (InvalidOperationException)
            {
                return address;
            }
            catch (DbEntityValidationException)
            {
                return address;
            }

        }
        public List<AddressDetails> GetAddressesForDetails(Person person)
        {
            List<AddressDetails> Addresses = new List<AddressDetails>();
            var addresses = from a in db.tblAddresses
                            join b in db.tlkpVillages on a.Village equals b.VillageID
                            join c in db.tlkpParishes on a.Parish equals c.ParishID
                            join d in db.tlkpConstituancies on a.Constituancy equals d.ConstituancyID
                            where a.PersonID == person.PersonId
                            select new
                            {
                                a.AddressID,
                                a.Street,
                                b.VillageName,
                                c.ParishName,
                                a.Postcode,
                                d.ConstituancyName,
                                a.IsCurrent,
                                a.PersonID,
                                a.DateMoved
                            };
            foreach (var address in addresses)
            {
                AddressDetails data = new AddressDetails();
                data.AddressId = address.AddressID;
                data.Street = address.Street;
                data.Village = address.VillageName;
                data.Parish = address.ParishName;
                data.Postcode = address.Postcode;
                data.Constituancy = address.ConstituancyName;
                data.IsCurrent = address.IsCurrent;
                data.PersonId = address.PersonID;
                data.DateMoved = address.DateMoved;
                Addresses.Add(data);
            }
            return Addresses;
        }

        #endregion
        #region Candidate
        public Boolean AddCandidate(Candidate candidate)
        {
            try
            {
                var initialusercount = db.tblCandidates.Count();
                var data = _mapper.Map<Candidate, tblCandidate>(candidate);
                db.tblCandidates.Add(data);
                db.SaveChanges();
                var afterusercount = db.tblCandidates.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean DeleteCandidate(int CandidateID)
        {
            try
            {
                var initialusercount = db.tblCandidates.Count();
                var data = db.tblCandidates.Find(CandidateID);
                if (data != null)
                {
                    db.tblCandidates.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tblCandidates.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateCandidate(Candidate candidate)
        {
            try
            {
                var dbCandidate = db.tblCandidates.Find(candidate.CandidateId); 
                dbCandidate.PersonID = candidate.PersonId;
                dbCandidate.PartyID = candidate.PartyId;
                dbCandidate.Constituancy = candidate.Constituancy;
                dbCandidate.CandidateImage = candidate.CandidateImage.Trim();
                dbCandidate.CandidateImageWeb = candidate.CandidateImageWeb.Trim();
                dbCandidate.IsCurrent = candidate.IsCurrent;
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
        public List<Candidate> GetCandidatesForElection()
        {
            List<Candidate> Candidates = new List<Candidate>();
            var candidates = (from c in db.tblCandidates
                              where c.IsCurrent == true
                              select c).ToList();
            foreach (var Candidate in candidates)
            {
                var data = _mapper.Map<tblCandidate, Candidate>(Candidate);
                Candidates.Add(data);
            }
            return Candidates;
        }
        public Candidate GetCandidateById(int id)
        {
            Candidate candidate = new Candidate();
            try
            {
                var dbCandidate = db.tblCandidates.Find(id);
                candidate = _mapper.Map<tblCandidate, Candidate>(dbCandidate);
                if (candidate != null)
                {
                    return candidate;
                }
                else
                {
                    return candidate;
                }
            }
            catch (NullReferenceException)
            {
                return candidate;
            }
            catch (InvalidOperationException)
            {
                return candidate;
            }
            catch (DbEntityValidationException)
            {
                return candidate;
            }

        }
        public List<CandidateDisplay> GetCandidatesDisplay() 
        { 
            List<CandidateDisplay> candidates = new List<CandidateDisplay>();
            var Candidates = from a in db.tblCandidates
                             join b in db.tblPersons on a.PersonID equals b.PersonID
                             join c in db.tlkpConstituancies on a.Constituancy equals c.ConstituancyID
                             join d in db.tlkpParties on a.PartyID equals d.PartyID
                             select new
                             {
                                 FullName = b.FirstName.Trim() + " " + b.LastName.Trim(),
                                 a.CandidateID,
                                 b.PersonID,
                                 d.PartyName,
                                 c.ConstituancyName,
                                 a.CandidateImage,
                                 a.CandidateImageWeb,
                                 a.IsCurrent

                             };
            foreach (var candidate in Candidates) 
            { 
                CandidateDisplay data = new CandidateDisplay();
                data.FullName = candidate.FullName;
                data.CandidateId = candidate.CandidateID;
                data.PersonId = candidate.PersonID;
                data.Party = candidate.PartyName.Trim();
                data.Constituancy = candidate.ConstituancyName.Trim();
                data.CandidateImage = candidate.CandidateImage;
                data.CandidateImageWeb = candidate.CandidateImageWeb;
                data.IsCurrent = candidate.IsCurrent;
                candidates.Add(data);
            }
            return candidates;
        }

        #endregion
        #region Person
        public Boolean AddPerson(Person person)
        {
            try
            {
                var initialusercount = db.tblPersons.Count();
                var data = _mapper.Map<Person, tblPerson>(person);
                db.tblPersons.Add(data);
                db.SaveChanges();
                var afterusercount = db.tblPersons.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public PersonResult AddPersonLocal(Person person)
        {
            PersonResult result = new PersonResult();
            try
            {
                var initialusercount = db.tblPersons.Count();
                var data = _mapper.Map<Person, tblPerson>(person);
                db.tblPersons.Add(data);
                var a = string.IsNullOrWhiteSpace(data.MiddleName);
                if (a)
                {
                    data.MiddleName = "none";
                }
                else
                {
                    data.MiddleName = person.MiddleName;
                }
                var b = string.IsNullOrWhiteSpace(data.Alias);
                if (b)
                {
                    data.Alias = "none";
                }
                else
                {
                    data.Alias = person.Alias;
                }               
                db.SaveChanges();
                var pesonid = data.PersonID;
                var afterusercount = db.tblPersons.Count();
                if (afterusercount > initialusercount)
                {
                    result.Id = pesonid;
                    result.Result = true;
                    return result;
                }
                else
                {
                    result.Id = 0;
                    result.Result = false;
                    return result;
                    
                }
            }
            catch (NullReferenceException)
            {
                result.Id = 0;
                result.Result = false;
                return result;
            }
            catch (InvalidOperationException)
            {
                result.Id = 0;
                result.Result = false;
                return result;
            }
            catch (DbEntityValidationException)
            {
                result.Id = 0;
                result.Result = false;
                return result;
            }
        }
        public Boolean DeletePerson(int PersonID)
        {
            try
            {
                var initialusercount = db.tblPersons.Count();
                var data = db.tblPersons.Find(PersonID);
                if (data != null)
                {
                    db.tblPersons.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tblPersons.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdatePerson(Person person)
        {
            try
            {
                var dbPerson = db.tblPersons.Find(person.PersonId);
                //db.tblPersons.Attach(dbPerson);
                //db.Entry(dbPerson).State = EntityState.Modified;
                dbPerson.FirstName = person.FirstName.Trim();
                var a = string.IsNullOrWhiteSpace(person.MiddleName);
                if (a)
                {
                    dbPerson.MiddleName = "none";
                }
                else
                {
                    dbPerson.MiddleName = person.MiddleName.Trim();
                }
                dbPerson.MiddleName = person.MiddleName.Trim();
                dbPerson.LastName = person.LastName.Trim();
                dbPerson.Gender = person.Gender.Trim();
                dbPerson.DOB = person.Dob;
                var b = string.IsNullOrWhiteSpace(person.Alias);
                if (b)
                {
                    dbPerson.Alias = "none";
                }
                else
                {
                    dbPerson.Alias = person.Alias.Trim();
                }                
                dbPerson.Address = person.Address;
                dbPerson.RegDate = person.RegDate;
                dbPerson.vFlag = person.VFlag;
                dbPerson.Pin = person.Pin.Trim();
                dbPerson.SocialSecurity = person.SocialSecurity;
                dbPerson.PersonImage = person.PersonImage;
                dbPerson.QrCode = person.QrCode;               
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
        public Boolean AddPersonQRCode(int PersonId, string QrCode)
        {
            try
            {
                var dbPerson = db.tblPersons.Find(PersonId);
                dbPerson.QrCode = QrCode;
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
        public Person GetPersonById(int p)
        {
            Person person = new Person();
            var x = (from u in db.tblPersons
                     where u.PersonID == p
                     select u).FirstOrDefault();
            if (x != null)
            {
                var data = _mapper.Map<tblPerson, Person>(x);
                return data;
            }
            else
            {
                person.PersonId = 0;
            }
            return person;
        }
        public List<Person> GetPersonByFirstAndLastName(string Fname,string Lname)
        {
            List<Person> person = new List<Person>();
            var x = (from u in db.tblPersons
                     where u.FirstName == Fname && u.LastName == Lname  
                     select u).ToList();
            if (x.Count > 0)
            {
                foreach (var y in x)
                {
                    var data = _mapper.Map<tblPerson, Person>(y);
                    person.Add(data);
                }
                return person;
            }
            else
            {
                
            }
            return person;
        }
        #endregion
        #region Parish
        public Boolean AddParish(Parish parish)
        {
            try
            {
                var initialusercount = db.tlkpParishes.Count();
                var data = _mapper.Map<Parish, tlkpParish>(parish);
                db.tlkpParishes.Add(data);
                db.SaveChanges();
                var afterusercount = db.tlkpParishes.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean DeleteParish(int parishID)
        {
            try
            {
                var initialusercount = db.tlkpParishes.Count();
                var data = db.tlkpParishes.Find(parishID);
                if (data != null)
                {
                    db.tlkpParishes.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tlkpParishes.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateParish(Parish parish)
        {
            try
            {
                var dbParish = db.tlkpParishes.Find(parish.ParishId);
                dbParish.ParishName = parish.ParishName.Trim();                
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
        public List<Parish> GetParishes()
        {
            List<Parish> Parishes = new List<Parish>();
            var parishes = db.tlkpParishes.ToList();
            foreach (var parish in parishes)
            {
                var data = _mapper.Map<tlkpParish, Parish>(parish);
                Parishes.Add(data);
            }
            return Parishes;
        }
        public Parish GetParishById(int id)
        {
            Parish parish = new Parish();
            try
            {
                var dbParish = db.tlkpParishes.Find(id);
                parish = _mapper.Map<tlkpParish, Parish>(dbParish);
                if (parish != null)
                {
                    return parish;
                }
                else
                {
                    return parish;
                }
            }
            catch (NullReferenceException)
            {
                return parish;
            }
            catch (InvalidOperationException)
            {
                return parish;
            }
            catch (DbEntityValidationException)
            {
                return parish;
            }

        }
        public Boolean ParishExist(string parishName)
        {
            try
            {
                var x = (from u in db.tlkpParishes
                         where u.ParishName == parishName
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        #endregion
        #region Party
        public Boolean AddParty(Party party)
        {
            try
            {
                var initialusercount = db.tlkpParties.Count();
                var data = _mapper.Map<Party, tlkpParty>(party);
                db.tlkpParties.Add(data);
                db.SaveChanges();
                var afterusercount = db.tlkpParties.Count();
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean DeleteParty(int partyID)
        {
            try
            {
                var initialusercount = db.tlkpParties.Count();
                var data = db.tlkpParties.Find(partyID);
                if (data != null)
                {
                    db.tlkpParties.Remove(data);
                    db.SaveChanges();
                    var afterusercount = db.tlkpParties.Count();
                    if (afterusercount < initialusercount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        public Boolean UpdateParty(Party party)
        {
            try
            {
                var dbParty = db.tlkpParties.Find(party.PartyId);
                dbParty.PartyName = party.PartyName.Trim();                
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
            catch (DbEntityValidationException ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
        public Party GetPartyById(int id)
        {
            Party party = new Party();
            try
            {
                var dbParty = db.tlkpParties.Find(id);
                party = _mapper.Map<tlkpParty, Party>(dbParty);
                if (party != null)
                {
                    return party;
                }
                else
                {
                    return party;
                }
            }
            catch (NullReferenceException)
            {
                return party;
            }
            catch (InvalidOperationException)
            {
                return party;
            }
            catch (DbEntityValidationException)
            {
                return party;
            }

        }
        public List<Party> GetParties()
        {
            List<Party> Parties = new List<Party>();
            var parties = db.tlkpParties.ToList();
            foreach (var party in parties)
            {
                var data = _mapper.Map<tlkpParty, Party>(party);
                Parties.Add(data);
            }
            return Parties;
        }
        public Boolean PartyExist(string partyName)
        {
            try
            {
                var x = (from u in db.tlkpParties
                         where u.PartyName == partyName
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
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbEntityValidationException)
            {
                return false;
            }
        }
        #endregion
        #region ModeltoViewModel
        public Person VMtoModelPerson(VoterPerson vM)
        {
            Person voter = new Person();
            voter.PersonId = vM.PersonId;
            voter.FirstName = vM.FirstName;
            voter.MiddleName = vM.MiddleName;
            voter.LastName = vM.LastName;
            voter.Dob = vM.Dob;
            voter.Alias = vM.Alias;
            voter.RegDate = vM.RegDate;
            voter.VFlag = vM.VFlag;
            voter.Pin = vM.Pin;
            voter.SocialSecurity = vM.SocialSecurity;
            voter.Gender = vM.Gender;
            voter.PersonImage = vM.PersonImage;
            voter.Address = vM.Address;
            return voter;
        }
        public Person VoterVMtoModelPerson(VoterVM vM)
        {
            Person voter = new Person();
            voter.PersonId = vM.PersonId;
            voter.FirstName = vM.FirstName;
            voter.MiddleName = vM.MiddleName;
            voter.LastName = vM.LastName;
            voter.Dob = vM.Dob;
            voter.Alias = vM.Alias;
            voter.RegDate = vM.RegDate;
            voter.VFlag = vM.VFlag;
            voter.Pin = vM.Pin;
            voter.SocialSecurity = vM.SocialSecurity;
            voter.Gender = vM.Gender;
            voter.PersonImage = vM.PersonImage;
            voter.Address = vM.Address;
            return voter;
        }
        public Address VMtoModelAddress(VoterPerson vM)
        {
            Address address = new Address();
            address.Street = vM.addressObject.Street;
            address.Village = vM.addressObject.Village;
            address.Parish = vM.addressObject.Parish;
            address.Postcode = vM.addressObject.Postcode;
            address.Constituancy = vM.addressObject.Constituancy;
            address.DateMoved = vM.addressObject.DateMoved;
            address.PersonId = vM.addressObject.PersonId;
            address.IsCurrent = vM.addressObject.IsCurrent; 
            
            return address;
        }
        public VoterVM PersontoVM(Person person) 
        {
            VoterVM vM = new VoterVM();
            vM.PersonId = person.PersonId; 
            vM.FirstName = person.FirstName;
            vM.MiddleName = person.MiddleName;
            vM.LastName = person.LastName;
            vM.Dob = person.Dob.Date;
            vM.Alias = person.Alias;
            vM.Address = person.Address;
            vM.RegDate = person.RegDate;
            vM.VFlag = person.VFlag;
            vM.Pin = person.Pin;
            vM.SocialSecurity = person.SocialSecurity;
            vM.Gender = person.Gender.Trim();
            vM.PersonImage = person.PersonImage;
            return vM;
        }
        public CandidatePerson VoterToCandidate(Person person)
        {
            CandidatePerson vM = new CandidatePerson();
            vM.FullName = person.FirstName.Trim() + " " + person.LastName.Trim();
            vM.PersonId = person.PersonId;
            return vM;
        }
        public Candidate vMToCandidate(CandidatePerson Vcandidate) 
        {
            Candidate candidate = new Candidate();
            candidate.CandidateId = Vcandidate.CandidateId;
            candidate.PersonId = Vcandidate.PersonId;
            candidate.PartyId = Vcandidate.Party;
            candidate.Constituancy = Vcandidate.Constituancy;
            candidate.CandidateImage = Vcandidate.CandidateImage;
            candidate.CandidateImageWeb = Vcandidate.CandidateImageWeb;
            candidate.IsCurrent = Vcandidate.IsCurrent;
            return candidate;
        }
        public CandidateDFED GetCandidateDFEDbyID(int ID)
        {
            CandidateDFED candidate = new CandidateDFED();
            var c = GetCandidateById(ID);
            if (c != null) 
            {
                candidate.CandidateId = c.CandidateId;
                candidate.PersonId = c.PersonId;
                candidate.Party = c.PartyId;
                candidate.Constituancy = c.Constituancy;
                candidate.CandidateImage = c.CandidateImage;
                candidate.CandidateImageWeb = c.CandidateImageWeb;
                candidate.IsCurrent = c.IsCurrent;
                var p = GetPersonById(c.PersonId); 
                if (p != null) 
                {
                    candidate.FullName = p.FirstName.Trim() + " " +p.LastName.Trim();
                }
            }
            return candidate;
        }
        public Candidate CandidateDFEDtoCandidate(CandidateDFED c) 
        {
            Candidate candidate = new Candidate();
            candidate.CandidateId = c.CandidateId;
            candidate.PersonId = c.PersonId;
            candidate.PartyId = c.Party;
            candidate.Constituancy = c.Constituancy;
            candidate.CandidateImage = c.CandidateImage;
            candidate.CandidateImageWeb = c.CandidateImageWeb;
            candidate.IsCurrent = c.IsCurrent;
            return candidate;
        }
        public VotersIdModel GetVotersId(int PersonID)
        {
            VotersIdModel votersId = new VotersIdModel();
            try
            {            
                var voter = GetPersonById(PersonID);
                if (voter != null) 
                {
                    votersId.FullName = voter.FirstName.Trim() + " " + voter.LastName.Trim();
                    votersId.DOB = voter.Dob;
                    votersId.VotersID = voter.PersonId.ToString();
                    votersId.Pin = voter.Pin;
                    votersId.VoterImage = voter.PersonImage;
                    votersId.VoterBarcode = voter.QrCode;
                    var address = GetAddressById(voter.Address);
                    var village = GetVillageById(address.Village);
                    var constituency = GetConstituancyById(address.Constituancy);
                    votersId.Address = village.VillageName;
                    votersId.Constituency = constituency.ConstituancyName;
                    return votersId;
                }
                else
                {
                    votersId.FullName = "No Data Available";
                    votersId = VotersIDFix(votersId);
                    return votersId;          
                }
            }catch (Exception ex)
            {
                ex.Message.ToString();
                votersId = VotersIDFix(votersId);
                return votersId;
            }
        }
        public VotersIdModel VotersIDFix(VotersIdModel vM)
        {
            VotersIdModel vo = new VotersIdModel();
            vo.MissingData = false;
            var a = string.IsNullOrWhiteSpace(vM.FullName);
            if (a)
            {
               vo.FullName = "none";
               vo.MissingData = true;
            }
            else
            {
                vo.FullName = vM.FullName;
            }
            var b = string.IsNullOrWhiteSpace(vM.Address);
            if (b)
            {
                vo.Address = "none";
                vo.MissingData = true;
            }
            else
            {
                vo.Address = vM.Address;
            }
            var c = string.IsNullOrWhiteSpace(vM.Constituency);
            if (c)
            {
                vo.Constituency = "none";
                vo.MissingData = true;
            }
            else
            {
                vo.Constituency = vM.Constituency;
            }
            var d = string.IsNullOrWhiteSpace(vM.VotersID);
            if (d)
            {
                vo.VotersID = "none";
                vo.MissingData = true;
            }
            else
            {
                vo.VotersID = vM.VotersID;
            }
            var e = string.IsNullOrWhiteSpace(vM.Pin);
            if (e)
            {
                vo.Pin = "none";
                vo.MissingData = true;
            }
            else
            {
                vo.Pin = vM.Pin;
            }            
            if (vM.VoterImage == "none")
            {
                vo.VoterImage = "~/Images/Missing.png";
                vo.MissingData = true;
            }
            else
            {
                vo.VoterImage = vM.VoterImage;
            }           
            if (vM.VoterBarcode == "none")
            {
                vo.VoterBarcode = "~/Images/Missing.png";
                vo.MissingData = true;
            }
            else
            {
                vo.VoterBarcode = vM.VoterBarcode;
            }
               vo.DOB = vM.DOB;
            return vo;  
        }
        #endregion
        #region Reports
        public List<ElectionResult> GetElectionResults()
        {
            List<ElectionResult> results = new List<ElectionResult>();
            var res = db.ElectionTally();
            foreach( var r in res ) 
            {
                results.Add( r );
            }
            return results;
        }
        public List<PartyByVillages> GetCrossTab()
        {
            List<PartyByVillages> result = new List<PartyByVillages>();
            var res = db.PartyByVillages();
            foreach( var r in res ) 
            {
                result.Add( r );    
            }
            return result;
        }
        public ElectionSummary GetElectionSummary()
        { 
            ElectionSummary result  = new ElectionSummary();
            var res = db.ElectionSummary();
            var summary = res.FirstOrDefault();
            result.RegVoter = summary.RegVoter;
            result.VoterTurnOut = summary.VoterTurnOut;
            result.Candidate1 = summary.Candidate1;
            result.Candidate2 = summary.Candidate2; 
            result.Candidate3 = summary.Candidate3;
            result.Candidate4 = summary.Candidate4;
            result.Candidate5 = summary.Candidate5;
            result.Candidate6 = summary.Candidate6;
            result.Candidate7 = summary.Candidate7;
            result.Candidate8 = summary.Candidate8;
            result.Candidate9 = summary.Candidate9;
            return result;
        }
        public void StartElection()
        {
            db.StartElection();
        }
        public void CloseElection()
        {
            db.CloseElection();
        }
        #endregion
    }
}
