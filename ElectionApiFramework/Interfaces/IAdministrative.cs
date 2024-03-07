using ElectionApiFramework.Models;
using ElectionApiFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElectionApiFramework.Interfaces
{
    public interface IAdministrative
    {
        #region User
        Boolean UserLogin(string Username, string Password);
        User GetUser(string Username);
        Boolean AddUser(User user);
        Boolean UpdateUser(User user);       
        Boolean DeleteUser(int userID);
        List<User> GetUsers();
        User GetUserById(int id);
        #endregion
        #region Village
        Boolean AddVillage(Village village);
        Boolean DeleteVillage(int villageID);
        Boolean UpdateVillage(Village village);
        List<Village> GetVillages();
        Village GetVillageById(int id);
        Boolean VillageExist(string villageName);
        #endregion
        #region Parish
        Boolean AddParish(Parish parish);
        Boolean DeleteParish(int parishID);
        Boolean UpdateParish(Parish parish);
        List<Parish> GetParishes();
        Parish GetParishById(int id);
        Boolean ParishExist(string parishName);
        #endregion
        #region Party
        Boolean AddParty(Party party);
        Boolean DeleteParty(int partyID);
        Boolean UpdateParty(Party party);
        Party GetPartyById(int id);
        List<Party> GetParties();
        Boolean PartyExist(string partyName);
        #endregion
        #region Constituancy
        Boolean AddConstituancy(Constituancy constituancy);
        Boolean DeleteConstituancy(int constituancyID);
        Boolean UpdateConstituancy(Constituancy constituancy);
        List<Constituancy> GetConstituancies();
        Constituancy GetConstituancyById(int id);
        Boolean ConstituancyExist(string constituancyName);
        #endregion
        #region Address
        Boolean AddAddress(Address address);
        PersonResult AddAddressLocal(Address address);
        Boolean DeleteAddress(int addressID);
        Boolean UpdateAddress(Address address);
        List<Address> GetAddresses(Person person);
        Address GetAddressById(int id);
        List<AddressDetails> GetAddressesForDetails(Person person);
        #endregion
        #region Candidate
        Boolean AddCandidate(Candidate candidate);
        Boolean DeleteCandidate(int CandidateID);
        Boolean UpdateCandidate(Candidate candidate);
        List<Candidate> GetCandidatesForElection();
        Candidate GetCandidateById(int id);
        List<CandidateDisplay> GetCandidatesDisplay();
        #endregion
        #region Person
        Boolean AddPerson(Person person);
        PersonResult AddPersonLocal(Person person);
        Boolean DeletePerson(int PersonID);
        Boolean UpdatePerson(Person person);
        Boolean AddPersonQRCode(int PersonId, string QrCode);
        Person GetPersonById(int id);
        List<Person> GetPersonByFirstAndLastName(string Fname, string Lname);
        #endregion
        #region ModelToViewModels
        Person VMtoModelPerson(VoterPerson vM);
        Person VoterVMtoModelPerson(VoterVM vM);
        Address VMtoModelAddress(VoterPerson vM);
        VoterVM PersontoVM(Person person);
        CandidatePerson VoterToCandidate(Person person);
        Candidate vMToCandidate(CandidatePerson Vcandidate);
        CandidateDFED GetCandidateDFEDbyID(int ID);
        Candidate CandidateDFEDtoCandidate(CandidateDFED c);
        VotersIdModel GetVotersId(int PersonID);
        VotersIdModel VotersIDFix(VotersIdModel vM);
        #endregion
        #region Reports
        List<ElectionResult> GetElectionResults();
        List<PartyByVillages> GetCrossTab();
        ElectionSummary GetElectionSummary();
        void StartElection();
        void CloseElection();
        #endregion
    }
}
