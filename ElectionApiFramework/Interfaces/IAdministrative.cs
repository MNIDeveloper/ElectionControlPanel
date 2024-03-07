using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElectionApiFramework.Interfaces
{
    public interface IAdministrative
    {
        Boolean UserLogin(string Username, string Password);
        User GetUser(string Username);
        Boolean AddUser(User user);
        Boolean UpdateUser(User user);
        Boolean DeleteUser(int userID);
        Boolean AddVillage(Village village);
        Boolean DeleteVillage(int villageID);
        Boolean UpdateVillage(Village village);
        List<Village> GetVillages();
        Boolean AddParish(Parish parish);
         Boolean DeleteParish(int parishID);
        Boolean UpdateParish(Parish parish);
        List<Parish> GetParishes();
        Boolean AddParty(Party party);
        Boolean DeleteParty(int partyID);
        Boolean UpdateParty(Party party);
        List<Party> GetParties();
        Boolean AddConstituancy(Constituancy constituancy);
        Boolean DeleteConstituancy(int constituancyID);
        Boolean UpdateConstituancy(Constituancy constituancy);
        List<Constituancy> GetConstituancies();
        Boolean AddAddress(Address address);
        Boolean DeleteAddress(int addressID);
        Boolean UpdateAddress(Address address);
        List<Address> GetAddresses(Person person);
        Boolean AddCandidate(Candidate candidate);
        Boolean DeleteCandidate(int CandidateID);
        Boolean UpdateCandidate(Candidate candidate);
        List<Candidate> GetCandidatesForElection();
        Boolean AddPerson(Person person);
        Boolean DeletePerson(int PersonID);
        Boolean UpdatePerson(Person person);
        Person GetPersonById(int p);
    }
}
