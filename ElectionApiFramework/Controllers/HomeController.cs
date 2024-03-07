using ElectionApiFramework.BLL;
using ElectionApiFramework.Helpers;
using ElectionApiFramework.Interfaces;
using ElectionApiFramework.Models;
using ElectionApiFramework.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;
using ZXing.QrCode.Internal;

namespace ElectionApiFramework.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IAdministrative admin = new AdminBLL();
        #region Index 
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public ActionResult Index(string txtUsername, string txtPassword)
        {
            var result = admin.UserLogin(txtUsername, txtPassword);
            if(result)
            {
                Session["UserCred"] = admin.GetUser(txtUsername);
                return RedirectToAction("Main", "Home");
            }
            else
            {
                ViewBag.result = 2;
                ViewBag.Msg = "Login Unsuccessful";
                return View();
            }
            
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return View("Index");
        }
        
        public ActionResult Main()
        {
            if ((Session["UserCred"] as User) != null)
            {
                ViewBag.Title = "Main Page";
                ViewBag.result = Session["token"];
                ViewBag.Msg = "Login Successful";
                Session["token"] = 0;
                return View();
            }
            else
            {
                return View("Index");
            }
            
        }
        #endregion
        #region Voter
        public ActionResult Voter()
        {

            if ((Session["UserCred"] as User) != null)
            {
                VoterPerson vM = new VoterPerson();
                vM.AddressFlag = false;
                vM.villages = new List<Village>();
                vM.parishes = new List<Parish>();
                vM.constituencies = new List<Constituancy>();
                try
                {
                    int v = (int)Session["token"];
                    if (v == 2)
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "Voter & Address Registration Successful";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Welcome to Voter Registraion";
                    ex.Message.ToString();
                    return View(vM);
                }
                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult VoterSave(VoterPerson vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                if (ModelState.IsValid)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    string filepath;
                    ViewBag.result = 1;
                    vM.AddressFlag = true;
                    vM.villages = admin.GetVillages();
                    vM.parishes = admin.GetParishes();
                    vM.constituencies = admin.GetConstituancies();
                    vM.RegDate = DateTime.Now;
                    vM.VFlag = false;
                    var voter = new Person();
                    voter = admin.VMtoModelPerson(vM);
                    if(vM.ImageData != null)
                    {
                        filepath = path + "Images\\VoterImages\\" + vM.ImageData.FileName;
                        voter.PersonImage = "\\Images\\VoterImages\\" + vM.ImageData.FileName;
                        vM.ImageData.SaveAs(filepath);
                    }
                    else
                    {
                        voter.PersonImage = "none";
                    }                    
                    voter.QrCode = "none";
                    var x = admin.AddPersonLocal(voter);
                    //Thread.Sleep(5000);
                    if (x.Result)
                    {
                        var z = admin.GetPersonById(x.Id);
                        BarcodeGen QR = new BarcodeGen();
                        var Y = QR.GenerateQRcode(path, x.Id, voter.Pin);
                        if (Y)
                        {
                            z.QrCode = "\\Images\\QRCodes\\" + x.Id.ToString() + ".jpg";
                            admin.AddPersonQRCode(z.PersonId,z.QrCode);
                        }
                        vM.PersonId = x.Id;
                        vM.addressObject = new Address();
                        vM.addressObject.PersonId = x.Id;
                        Session["RegisteredVoter"] = z;
                    }
                    
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Successfully Registered";
                    return View("Voter", vM);
                }
                else
                {
                    vM.AddressFlag = false;
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Registration Unsuccessful";
                    return View("Voter", vM);
                }

            }
            else
            {
                return View("Index");
            }

        }
        public ActionResult AddressEdit(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var vM = new VoterVM();
                var address = admin.GetAddressById((int)id);
                var voter = admin.GetPersonById(address.PersonId);
                var Addresses = admin.GetAddressesForDetails(voter);
                vM = admin.PersontoVM(voter);
                vM.AddressModify = address;
                vM.VoterAddresses = Addresses;
                vM.villages = admin.GetVillages();
                vM.parishes = admin.GetParishes();
                vM.constituencies = admin.GetConstituancies();
                vM.addressDetails = new AddressDetails();
                ViewBag.result = 4;
                return View("VoterDetails", vM);

            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult VoterAddressDelete(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var vM = new VoterVM();
                var address = admin.GetAddressById((int)id);
                var voter = admin.GetPersonById(address.PersonId);
                var Addresses = admin.GetAddressesForDetails(voter);
                vM = admin.PersontoVM(voter);
                vM.AddressModify = address;
                vM.VoterAddresses = Addresses;
                vM.villages = admin.GetVillages();
                vM.parishes = admin.GetParishes();
                vM.constituencies = admin.GetConstituancies();
                vM.addressDetails = (from u in Addresses where u.AddressId == (int)id select u).FirstOrDefault();
                ViewBag.result = 3;
                return View("VoterDetails", vM);

            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult VoterAddressDelete(VoterVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var address = admin.GetAddressById(vM.addressDetails.AddressId);
                var result = admin.DeleteAddress(address.AddressId);
                if (result)
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Address Successfully Deleted";
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Address Deletion Unsuccessful";
                }
                var voter = admin.GetPersonById(vM.addressDetails.PersonId);
                var Addresses = admin.GetAddressesForDetails(voter);
                vM = admin.PersontoVM(voter);
                vM.VoterAddresses = Addresses;
                vM.NewAddress = new Address();
                vM.NewAddress.PersonId = voter.PersonId;
                vM.villages = admin.GetVillages(); ;
                vM.parishes = admin.GetParishes();
                vM.constituencies = admin.GetConstituancies();
                ModelState.Clear();
                return View("VoterDetails", vM);

            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult VoterUpdate(VoterVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                if (ModelState.IsValid)
                {
                    var voter = new Person();
                    voter = admin.VoterVMtoModelPerson(vM);
                    var Addresses = admin.GetAddressesForDetails(voter);
                    var ImgName = voter.PersonImage;
                    ImgName = ImgName.Replace("\\Images\\VoterImages\\", "");
                    var a = false;
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    try
                    {
                        if (vM.ImageData.FileName != null)
                        {
                            a = true;
                        }
                        else
                        {
                            a = false;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        a = false;
                    }
                    if (a)
                    {
                        bool res = vM.ImageData.FileName.Equals(ImgName);
                        if (res)
                        {

                        }
                        else
                        {
                            string filepath;
                            filepath = path + "Images\\VoterImages\\" + vM.ImageData.FileName;
                            voter.PersonImage = "\\Images\\VoterImages\\" + vM.ImageData.FileName;
                            vM.ImageData.SaveAs(filepath);
                        }
                    }
                    var x = admin.GetPersonById(vM.PersonId);
                    if (vM.Pin != x.Pin)
                    {
                        BarcodeGen QR = new BarcodeGen();
                        var Y = QR.UpdateQRCode(path, vM.PersonId, vM.Pin);
                        if (Y)
                        {
                            voter.QrCode = "\\Images\\QRCodes\\" + vM.PersonId.ToString() + ".jpg";
                        }
                    }
                    else
                    {
                        voter.QrCode = x.QrCode;
                    }

                    var result = admin.UpdatePerson(voter);
                    vM.VoterAddresses = Addresses;
                    vM.villages = admin.GetVillages();
                    vM.parishes = admin.GetParishes();
                    vM.constituencies = admin.GetConstituancies();
                    if (result)
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "Voter Successfully Updated";
                        return View("VoterDetails", vM);
                    }
                    else
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "Voter Update Unsuccessful";
                        return View("VoterDetails", vM);
                    }

                }
                else
                {
                    var voter = new Person();
                    voter = admin.VoterVMtoModelPerson(vM);
                    var Addresses = admin.GetAddressesForDetails(voter);
                    vM.VoterAddresses = Addresses;
                    vM.villages = admin.GetVillages();
                    vM.parishes = admin.GetParishes();
                    vM.constituencies = admin.GetConstituancies();
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Update Unsuccessful";
                    return View("VoterDetails", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult VoterSearch(string txtFname, string txtLname)
        {
            if ((Session["UserCred"] as User) != null)
            {
                VoterPerson vM = new VoterPerson();
                var voters = admin.GetPersonByFirstAndLastName(txtFname, txtLname);
                if (voters.Count > 0)
                {
                    vM.VoterList = voters;
                    vM.AddressFlag = false;
                    vM.villages = new List<Village>();
                    vM.parishes = new List<Parish>();
                    vM.constituencies = new List<Constituancy>();
                    return View("Voter", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Not Found";
                    vM.VoterList = voters;
                    vM.AddressFlag = false;
                    vM.villages = new List<Village>();
                    vM.parishes = new List<Parish>();
                    vM.constituencies = new List<Constituancy>();
                    return View("Voter", vM);
                }
            }
            else
            {
                return View("Index");
            }

        }
        public ActionResult AddressSave(VoterPerson vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var address = new Address();
                var voter = new Person();
                address = admin.VMtoModelAddress(vM);
                var a = admin.AddAddressLocal(address);
                if (a.Result)
                {
                    voter = (Person)Session["RegisteredVoter"];
                    voter.PersonId = address.PersonId;
                    voter.Address = a.Id;
                    admin.UpdatePerson(voter);
                }
                vM = new VoterPerson();
                vM.AddressFlag = false;
                vM.villages = new List<Village>();
                vM.parishes = new List<Parish>();
                vM.constituencies = new List<Constituancy>();
                Session["token"] = 2;
                //return View("Voter",vM);
                return RedirectToAction("Voter", "Home");
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult AddVoterAddress(VoterVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var voter = new Person();
                voter = admin.GetPersonById(vM.NewAddress.PersonId);
                var a = admin.AddAddressLocal(vM.NewAddress);
                if (a.Result)
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Address Successfully Added";
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Address Addition Unsuccesful";
                }
                voter.Address = a.Id;
                admin.UpdatePerson(voter);
                var x = admin.PersontoVM(voter);
                x.VoterAddresses = admin.GetAddressesForDetails(voter);
                x.NewAddress = new Address();
                x.villages = admin.GetVillages();
                x.parishes = admin.GetParishes();
                x.constituencies = admin.GetConstituancies();
                ModelState.Clear();
                return View("VoterDetails", x);
            }
            else
            {
                return View("Index");
            }

        }
        public ActionResult AddressUpdate(VoterVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var voter = new Person();
                voter = admin.GetPersonById(vM.AddressModify.PersonId);
                var a = admin.UpdateAddress(vM.AddressModify);
                if (a)
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Address Successfully Updated";
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Address Update Unsuccesful";
                }
                var x = admin.PersontoVM(voter);
                x.VoterAddresses = admin.GetAddressesForDetails(voter);
                x.NewAddress = new Address();
                x.villages = admin.GetVillages();
                x.parishes = admin.GetParishes();
                x.constituencies = admin.GetConstituancies();
                ModelState.Clear();
                return View("VoterDetails", x);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult VoterDetails(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                VoterVM vM = new VoterVM();
                var voter = admin.GetPersonById((int)id);
                var Addresses = admin.GetAddressesForDetails(voter);
                vM = admin.PersontoVM(voter);
                vM.VoterAddresses = Addresses;
                vM.NewAddress = new Address();
                vM.NewAddress.PersonId = voter.PersonId;
                vM.villages = admin.GetVillages(); ;
                vM.parishes = admin.GetParishes();
                vM.constituencies = admin.GetConstituancies();
                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult Votersid(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var vM = admin.GetVotersId((int)id);
                if (vM.MissingData)
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Data Missing... Please enusre all voter information is populated";
                }
                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }
        #endregion
        #region Candidate
        public ActionResult Candidate()
        {

            if ((Session["UserCred"] as User) != null)
            {
                CandidatePerson vM = new CandidatePerson();
                vM.constituencies = admin.GetConstituancies();
                vM.parties = admin.GetParties();
                //ViewBag.result = Session["token"];
                //ViewBag.Msg = Session["Msg"];

                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult CandidateSearch(string txtFname, string txtLname)
        {
            if ((Session["UserCred"] as User) != null)
            {
                CandidatePerson vM = new CandidatePerson();
                var voters = admin.GetPersonByFirstAndLastName(txtFname, txtLname);
                if (voters.Count > 0)
                {
                    vM.constituencies = admin.GetConstituancies();
                    vM.parties = admin.GetParties();
                    vM.VoterList = voters;
                    return View("Candidate", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Voter Not Found";
                    vM.constituencies = admin.GetConstituancies();
                    vM.parties = admin.GetParties();
                    vM.VoterList = voters;
                    return View("Candidate", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult CandidateSelect(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                CandidatePerson vM = new CandidatePerson();
                var voter = admin.GetPersonById((int)id);
                vM = admin.VoterToCandidate(voter);
                vM.constituencies = admin.GetConstituancies();
                vM.parties = admin.GetParties();
                return View("Candidate", vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult CandidateSave(CandidatePerson vM)
        {

            if ((Session["UserCred"] as User) != null)
            {
                if (ModelState.IsValid)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    string filepath;
                    filepath = path + "Images\\CandidateImages\\" + vM.ImageData.FileName;
                    vM.CandidateImage = "\\Images\\CandidateImages\\" + vM.ImageData.FileName;
                    vM.CandidateImageWeb = "http://mnideveloper-001-site1.btempurl.com/Images/CandidateImages/" + vM.ImageData.FileName;
                    vM.ImageData.SaveAs(filepath);
                    var candidate = admin.vMToCandidate(vM);
                    var result = admin.AddCandidate(candidate);
                    if (result)
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "Candidate Sucessfully Added";
                    }
                    else
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "Candidate Addition Unsuccessful";
                    }
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Candidate Addition Unsuccessful";
                }
                vM.constituencies = admin.GetConstituancies();
                vM.parties = admin.GetParties();
                return View("Candidate", vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult CandidateDetails()
        {
            if ((Session["UserCred"] as User) != null)
            {
                CandidateDetailsVM vM = new CandidateDetailsVM();
                vM.candidates = admin.GetCandidatesDisplay();
                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult CandidateEdit(int? id)
        {

            if ((Session["UserCred"] as User) != null)
            {
                CandidateDetailsVM vM = new CandidateDetailsVM();
                vM.candidates = admin.GetCandidatesDisplay();
                vM.candidateEdit = admin.GetCandidateDFEDbyID((int)id);
                vM.parties = admin.GetParties();
                vM.constituencies = admin.GetConstituancies();
                ViewBag.result = 4;
                return View("CandidateDetails", vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult CandidateEdit(CandidateDetailsVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var candidate = admin.CandidateDFEDtoCandidate(vM.candidateEdit);
                var ImgName = candidate.CandidateImage;
                ImgName = ImgName.Replace("\\Images\\CandidateImages\\", "");
                var a = false;
                try
                {
                    if (vM.candidateEdit.ImageData.FileName != null)
                    {
                        a = true;
                    }
                    else
                    {
                        a = false;
                    }
                }
                catch (NullReferenceException)
                {
                    a = false;
                }

                if (a)
                {
                    bool res = vM.candidateEdit.ImageData.FileName.Equals(ImgName);
                    if (res)
                    {

                    }
                    else
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        string filepath;
                        filepath = path + "Images\\CandidateImages\\" + vM.candidateEdit.ImageData.FileName;
                        candidate.CandidateImage = "\\Images\\CandidateImages\\" + vM.candidateEdit.ImageData.FileName;
                        candidate.CandidateImageWeb = "http://mnideveloper-001-site1.btempurl.com/Images/CandidateImages/" + vM.candidateEdit.ImageData.FileName;
                        vM.candidateEdit.ImageData.SaveAs(filepath);
                    }
                }
                var result = admin.UpdateCandidate(candidate);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Candidate Successfully Updated";
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Candidate Update Unsuccessful";
                }
                vM.candidates = admin.GetCandidatesDisplay();
                vM.parties = admin.GetParties();
                vM.constituencies = admin.GetConstituancies();
                return View("CandidateDetails", vM);
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult CandidateDelete(int? id)
        {

            if ((Session["UserCred"] as User) != null)
            {
                CandidateDetailsVM vM = new CandidateDetailsVM();
                vM.candidates = admin.GetCandidatesDisplay();
                vM.candidateDelete = admin.GetCandidateDFEDbyID((int)id);
                vM.parties = admin.GetParties();
                vM.constituencies = admin.GetConstituancies();
                ViewBag.result = 3;
                return View("CandidateDetails", vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult CandidateDelete(CandidateDetailsVM vM)
        {

            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.DeleteCandidate(vM.candidateDelete.CandidateId);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Candidate Successfully Deleted";
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Candidate Delete Unsuccessful";
                }
                vM.candidates = admin.GetCandidatesDisplay();
                vM.parties = admin.GetParties();
                vM.constituencies = admin.GetConstituancies();
                return View("CandidateDetails", vM);
            }
            else
            {
                return View("Index");
            }
        }
        #endregion
        #region Constituency
        public ActionResult Constituency()
        {
            if ((Session["UserCred"] as User) != null)
            {
                List<Constituancy> constituencies = new List<Constituancy>();
                constituencies = admin.GetConstituancies();
                ViewData["Constituencies"] = constituencies;
                return View();
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult Constituency(string txtConstituencyName)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var exist = admin.ConstituancyExist(txtConstituencyName);
                if (exist)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Constituency Already Exist";
                    List<Constituancy> constituencies = new List<Constituancy>();
                    constituencies = admin.GetConstituancies();
                    ViewData["Constituencies"] = constituencies;
                    return View();
                }
                else
                {
                    Constituancy constituency = new Constituancy();
                    constituency.ConstituancyName = txtConstituencyName;
                    var result = admin.AddConstituancy(constituency);
                    if (result)
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Constituency Succesfully Added";
                        List<Constituancy> constituencies = new List<Constituancy>();
                        constituencies = admin.GetConstituancies();
                        ViewData["Constituencies"] = constituencies;
                        return View();

                    }
                    else
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Constituency Not Added";
                        List<Constituancy> constituencies = new List<Constituancy>();
                        constituencies = admin.GetConstituancies();
                        ViewData["Constituencies"] = constituencies;
                        return View();
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult ConstituencyEdit(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                Constituancy constituancy = admin.GetConstituancyById((int)id);
                if (constituancy == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    List<Constituancy> constituencies = new List<Constituancy>();
                    constituencies = admin.GetConstituancies();
                    ViewData["Constituencies"] = constituencies;
                    ViewBag.result = 4;

                    return View("Constituency", constituancy);
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult ConstituencyEdit(Constituancy constituancy)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.UpdateConstituancy(constituancy);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Constituency Succesfully Updated";
                    List<Constituancy> constituencies = new List<Constituancy>();
                    constituencies = admin.GetConstituancies();
                    ViewData["Constituencies"] = constituencies;
                    return View("Constituency");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Constituency Not Updated";
                    List<Constituancy> constituencies = new List<Constituancy>();
                    constituencies = admin.GetConstituancies();
                    ViewData["Constituencies"] = constituencies;
                    return View("Constituency");
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult ConstituencyDelete(int id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                ViewBag.result = 3;
                var constituancy = admin.GetConstituancyById(id);
                List<Constituancy> constituencies = new List<Constituancy>();
                constituencies = admin.GetConstituancies();
                ViewData["Constituencies"] = constituencies;
                return View("Constituency", constituancy);
            }
            else
            {
                return View("Index");
            }

        }
        [HttpPost]
        public ActionResult ConstituencyDelete(Constituancy constituancy)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.DeleteConstituancy(constituancy.ConstituancyId);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Constituency Deleted";
                    List<Constituancy> constituencies = new List<Constituancy>();
                    constituencies = admin.GetConstituancies();
                    ViewData["Constituencies"] = constituencies;
                    return View("Constituency");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Constituency Not Deleted";
                    List<Constituancy> constituencies = new List<Constituancy>();
                    constituencies = admin.GetConstituancies();
                    ViewData["Constituencies"] = constituencies;
                    return View("Constituency");
                }
            }
            else
            {
                return View("Index");
            }
        }
        #endregion
        #region Parish
        public ActionResult Parish()
        {
            if ((Session["UserCred"] as User) != null)
            {
                List<Parish> parishes = new List<Parish>();
                parishes = admin.GetParishes();
                ViewData["Parishes"] = parishes;
                return View();
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult ParishEdit(int? id)
        {

            Parish parish = admin.GetParishById((int)id);
            if (parish == null)
            {
                return HttpNotFound();
            }
            else
            {
                List<Parish> parishes = new List<Parish>();
                parishes = admin.GetParishes();
                ViewData["Parishes"] = parishes;
                ViewBag.result = 4;

                return View("Parish", parish);
            }
        }
        [HttpPost]
        public ActionResult ParishEdit(Parish parish)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.UpdateParish(parish);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Parish Succesfully Updated";
                    List<Parish> parishes = new List<Parish>();
                    parishes = admin.GetParishes();
                    ViewData["Parishes"] = parishes;
                    return View("Parish");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Parish Not Updated";
                    List<Parish> parishes = new List<Parish>();
                    parishes = admin.GetParishes();
                    ViewData["Parishes"] = parishes;
                    return View("Parish");
                }
            }
            else
            {
                return View("Index");
            }

        }

        [HttpPost]
        public ActionResult ParishDelete(Parish parish)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.DeleteParish(parish.ParishId);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Parish Deleted";
                    List<Parish> parishes = new List<Parish>();
                    parishes = admin.GetParishes();
                    ViewData["Parishes"] = parishes;
                    return View("Parish");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Parish Not Deleted";
                    List<Parish> parishes = new List<Parish>();
                    parishes = admin.GetParishes();
                    ViewData["Parishes"] = parishes;
                    return View("Parish");
                }
            }
            else
            {
                return View("Index");
            }
        }
    
        public ActionResult ParishDelete(int id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                ViewBag.result = 3;
                var parish = admin.GetParishById(id);
                List<Parish> parishes = new List<Parish>();
                parishes = admin.GetParishes();
                ViewData["Parishes"] = parishes;
                return View("Parish", parish);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Parish(string txtParishName)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var exist = admin.ParishExist(txtParishName);
                if (exist)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Parish Already Exist";
                    List<Parish> parishes = new List<Parish>();
                    parishes = admin.GetParishes();
                    ViewData["Parishes"] = parishes;
                    return View();
                }
                else
                {
                    Parish parish = new Parish();
                    parish.ParishName = txtParishName;
                    var result = admin.AddParish(parish);
                    if (result)
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Parish Succesfully Added";
                        List<Parish> parishes = new List<Parish>();
                        parishes = admin.GetParishes();
                        ViewData["Parishes"] = parishes;
                        return View();

                    }
                    else
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Parish Not Added";
                        List<Parish> parishes = new List<Parish>();
                        parishes = admin.GetParishes();
                        ViewData["Parishes"] = parishes;
                        return View();
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }

        #endregion       
        #region Village
        //initial Village Call
        public ActionResult Village()
        {
            if ((Session["UserCred"] as User) != null)
            {
                List<Village> villages = new List<Village>();
                villages = admin.GetVillages();
                ViewData["Villages"] = villages;
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        // Village Edit call
        public ActionResult VillageEdit(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                Village village = admin.GetVillageById((int)id);
                if (village == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    List<Village> villages = new List<Village>();
                    villages = admin.GetVillages();
                    ViewData["Villages"] = villages;
                    ViewBag.result = 4;

                    return View("Village", village);
                }
            }
            else
            {
                return View("Index");
            }
        }
        //Village update call
        [HttpPost]
        public ActionResult VillageEdit(Village village)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.UpdateVillage(village);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Village Succesfully Updated";
                    List<Village> villages = new List<Village>();
                    villages = admin.GetVillages();
                    ViewData["Villages"] = villages;
                    return View("Village");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Village Not Updated";
                    List<Village> villages = new List<Village>();
                    villages = admin.GetVillages();
                    ViewData["Villages"] = villages;
                    return View();
                }
            }
            else
            {
                return View("Index");
            }

        }
        //Village create call
        [HttpPost]
        public ActionResult Village(string txtVillageName) 
        {
            if ((Session["UserCred"] as User) != null)
            {
                var exist = admin.VillageExist(txtVillageName);
                if (exist)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Village Already Exist";
                    List<Village> villages = new List<Village>();
                    villages = admin.GetVillages();
                    ViewData["Villages"] = villages;
                    return View();
                }
                else
                {
                    Village village = new Village();
                    village.VillageName = txtVillageName;
                    var result = admin.AddVillage(village);
                    if (result)
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Village Succesfully Added";
                        List<Village> villages = new List<Village>();
                        villages = admin.GetVillages();
                        ViewData["Villages"] = villages;
                        return View();

                    }
                    else
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Village Not Added";
                        List<Village> villages = new List<Village>();
                        villages = admin.GetVillages();
                        ViewData["Villages"] = villages;
                        return View();
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }
        //Village Delete call 
        public ActionResult VillageDelete(int id) 
        {
            if ((Session["UserCred"] as User) != null)
            {
                ViewBag.result = 3;
                var village = admin.GetVillageById(id);
                List<Village> villages = new List<Village>();
                villages = admin.GetVillages();
                ViewData["Villages"] = villages;
                return View("Village", village);
            }
            else
            {
                return View("Index");
            }

        }
        //Actual Village Delete Confirmation Call
        [HttpPost]
        public ActionResult VillageDelete(Village village)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.DeleteVillage(village.VillageId);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Village Deleted";
                    List<Village> villages = new List<Village>();
                    villages = admin.GetVillages();
                    ViewData["Villages"] = villages;
                    return View("Village");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Village Not Deleted";
                    List<Village> villages = new List<Village>();
                    villages = admin.GetVillages();
                    ViewData["Villages"] = villages;
                    return View("Village");
                }
            }
            else
            {
                return View("Index");
            }
        }
        #endregion
        #region Party

        public ActionResult Party()
        {
            if ((Session["UserCred"] as User) != null)
            {
                List<Party> parties = new List<Party>();
                parties = admin.GetParties();
                ViewData["Parties"] = parties;
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Party(string txtPartyName)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var exist = admin.PartyExist(txtPartyName);
                if (exist)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Party Already Exist";
                    List<Party> parties = new List<Party>();
                    parties = admin.GetParties();
                    ViewData["Parties"] = parties;
                    return View();
                }
                else
                {
                    Party party = new Party();
                    party.PartyName = txtPartyName;
                    var result = admin.AddParty(party);
                    if (result)
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Party Succesfully Added";
                        List<Party> parties = new List<Party>();
                        parties = admin.GetParties();
                        ViewData["Parties"] = parties;
                        return View();

                    }
                    else
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "Party Not Added";
                        List<Party> parties = new List<Party>();
                        parties = admin.GetParties();
                        ViewData["Parties"] = parties;
                        return View();
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult PartyDelete(int id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                ViewBag.result = 3;
                var party = admin.GetPartyById(id);
                List<Party> parties = new List<Party>();
                parties = admin.GetParties();
                ViewData["Parties"] = parties;
                return View("Party", party);
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult PartyDelete(Party party)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.DeleteParty(party.PartyId);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Party Deleted";
                    List<Party> parties = new List<Party>();
                    parties = admin.GetParties();
                    ViewData["Parties"] = parties;
                    return View("Party");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Party Not Deleted";
                    List<Party> parties = new List<Party>();
                    parties = admin.GetParties();
                    ViewData["Parties"] = parties;
                    return View("Party");
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult PartyEdit(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                Party party = admin.GetPartyById((int)id);
                if (party == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    List<Party> parties = new List<Party>();
                    parties = admin.GetParties();
                    ViewData["Parties"] = parties;
                    ViewBag.result = 4;

                    return View("Party", party);
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult PartyEdit(Party party)
        {
            if ((Session["UserCred"] as User) != null)
            {
                var result = admin.UpdateParty(party);
                if (result)
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Party Succesfully Updated";
                    List<Party> parties = new List<Party>();
                    parties = admin.GetParties();
                    ViewData["Parties"] = parties;
                    return View("Party");
                }
                else
                {
                    ViewBag.result = 5;
                    ViewBag.Msg = "Party Not Updated";
                    List<Party> parties = new List<Party>();
                    parties = admin.GetParties();
                    ViewData["Parties"] = parties;
                    return View("Party");
                }
            }
            else
            {
                return View("Index");
            }

        }
        #endregion
        #region Users

        public ActionResult Users()
        {
            if ((Session["UserCred"] as User) != null)
            {
                UserVM vM = new UserVM();
                vM.users = admin.GetUsers();

                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult UserSave(UserVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                if ((Session["UserCred"] as User).Position == 1)
                {
                    User valid = new User();
                    //UserVM vM = new UserVM();
                    if (ModelState.IsValid)
                    {
                        valid = vM.AddUser;
                        var x = admin.AddUser(valid);
                        if (x)
                        {
                            ViewBag.result = 1;
                            ViewBag.Msg = "User Successfully Added";
                        }
                        else
                        {
                            ViewBag.result = 1;
                            ViewBag.Msg = "User Registration Unsuccessful";
                        }
                    }
                    else
                    {
                        ViewBag.result = 6;
                    }

                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Not Authorised to perform task only Administrators ";
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult UserEdit(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                UserVM vM = new UserVM();
                if ((Session["UserCred"] as User).Position == 1)
                {

                    vM.EditUser = admin.GetUserById((int)id);
                    vM.users = admin.GetUsers();
                    ViewBag.result = 4;
                    return View("Users", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Not Authorised to perform task only Administrators ";
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult UserEdit(UserVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                if ((Session["UserCred"] as User).Position == 1)
                {
                    var user = vM.EditUser;
                    var result = admin.UpdateUser(user);
                    if (result)
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "User Successfully Updated";
                    }
                    else
                    {
                        ViewBag.result = 5;
                        ViewBag.Msg = "User Update Unsuccessful";
                    }
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Not Authorised to perform task only Administrators ";
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult UserDelete(int? id)
        {
            if ((Session["UserCred"] as User) != null)
            {
                UserVM vM = new UserVM();
                if ((Session["UserCred"] as User).Position == 1)
                {

                    vM.DeleteUser = admin.GetUserById((int)id);
                    vM.users = admin.GetUsers();
                    ViewBag.result = 3;
                    return View("Users", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Not Authorised to perform task only Administrators ";
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult UserDelete(UserVM vM)
        {
            if ((Session["UserCred"] as User) != null)
            {
                if ((Session["UserCred"] as User).Position == 1)
                {
                    var user = vM.DeleteUser;
                    var result = admin.DeleteUser(user.UserId);
                    if (result)
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "User Successfully Deleted";
                    }
                    else
                    {
                        ViewBag.result = 1;
                        ViewBag.Msg = "User Delete Unsuccessful";
                    }
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
                else
                {
                    ViewBag.result = 1;
                    ViewBag.Msg = "Not Authorised to perform task only Administrators ";
                    vM.users = admin.GetUsers();
                    return View("Users", vM);
                }
            }
            else
            {
                return View("Index");
            }
        }
        #endregion
        #region Report
        public ActionResult Reports()
        {
            if ((Session["UserCred"] as User) != null)
            {
                ReportsVM vM = new ReportsVM();
                vM.Results = admin.GetElectionResults();
                foreach (var item in vM.Results)
                {
                    vM.TotalVotes += item.Votes;
                }
                foreach (var item in vM.Results)
                {
                    decimal x = Convert.ToDecimal(item.Votes);
                    decimal y = Convert.ToDecimal(vM.TotalVotes);
                    item.Percentage = ((x / y) * 100);
                    vM.TotalPercentage += item.Percentage;
                }
                vM.ByVillageResults = admin.GetCrossTab();
                vM.Summary = admin.GetElectionSummary();
                decimal a = Convert.ToDecimal(vM.TotalVotes);
                decimal b = Convert.ToDecimal(vM.Summary.Candidate1);
                decimal c = Convert.ToDecimal(vM.Summary.Candidate2);
                decimal d = Convert.ToDecimal(vM.Summary.Candidate3);
                decimal e = Convert.ToDecimal(vM.Summary.Candidate4);
                decimal f = Convert.ToDecimal(vM.Summary.Candidate5);
                decimal g = Convert.ToDecimal(vM.Summary.Candidate6);
                decimal h = Convert.ToDecimal(vM.Summary.Candidate7);
                decimal i = Convert.ToDecimal(vM.Summary.Candidate8);
                decimal j = Convert.ToDecimal(vM.Summary.Candidate9);
                decimal k = Convert.ToDecimal(vM.Summary.VoterTurnOut);
                decimal l = Convert.ToDecimal(vM.Summary.RegVoter);
                vM.Candidate1Percentage = ((b / a) * 100);
                vM.Candidate2Percentage = ((c / a) * 100);
                vM.Candidate3Percentage = ((d / a) * 100);
                vM.Candidate4Percentage = ((e / a) * 100);
                vM.Candidate5Percentage = ((f / a) * 100);
                vM.Candidate6Percentage = ((g / a) * 100);
                vM.Candidate7Percentage = ((h / a) * 100);
                vM.Candidate8Percentage = ((i / a) * 100);
                vM.Candidate9Percentage = ((j / a) * 100);
                vM.VoterTurnOutPercentage = ((k / l) * 100);
                return View(vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult StartElection()
        {
            if ((Session["UserCred"] as User) != null)
            {
                admin.StartElection();
                ReportsVM vM = new ReportsVM();
                vM.Results = admin.GetElectionResults();
                foreach (var item in vM.Results)
                {
                    vM.TotalVotes += item.Votes;
                }
                foreach (var item in vM.Results)
                {
                    decimal x = Convert.ToDecimal(item.Votes);
                    decimal y = Convert.ToDecimal(vM.TotalVotes);
                    item.Percentage = ((x / y) * 100);
                    vM.TotalPercentage += item.Percentage;
                }
                vM.ByVillageResults = admin.GetCrossTab();
                vM.Summary = admin.GetElectionSummary();
                decimal a = Convert.ToDecimal(vM.TotalVotes);
                decimal b = Convert.ToDecimal(vM.Summary.Candidate1);
                decimal c = Convert.ToDecimal(vM.Summary.Candidate2);
                decimal d = Convert.ToDecimal(vM.Summary.Candidate3);
                decimal e = Convert.ToDecimal(vM.Summary.Candidate4);
                decimal f = Convert.ToDecimal(vM.Summary.Candidate5);
                decimal g = Convert.ToDecimal(vM.Summary.Candidate6);
                decimal h = Convert.ToDecimal(vM.Summary.Candidate7);
                decimal i = Convert.ToDecimal(vM.Summary.Candidate8);
                decimal j = Convert.ToDecimal(vM.Summary.Candidate9);
                decimal k = Convert.ToDecimal(vM.Summary.VoterTurnOut);
                decimal l = Convert.ToDecimal(vM.Summary.RegVoter);
                vM.Candidate1Percentage = ((b / a) * 100);
                vM.Candidate2Percentage = ((c / a) * 100);
                vM.Candidate3Percentage = ((d / a) * 100);
                vM.Candidate4Percentage = ((e / a) * 100);
                vM.Candidate5Percentage = ((f / a) * 100);
                vM.Candidate6Percentage = ((g / a) * 100);
                vM.Candidate7Percentage = ((h / a) * 100);
                vM.Candidate8Percentage = ((i / a) * 100);
                vM.Candidate9Percentage = ((j / a) * 100);
                vM.VoterTurnOutPercentage = ((k / l) * 100);
                ViewBag.result = 1;
                ViewBag.Msg = "The Election was Successfully Started";
                return View("Reports",vM);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult StopElection()
        {
            if ((Session["UserCred"] as User) != null)
            {
                admin.CloseElection();
                ReportsVM vM = new ReportsVM();
                vM.Results = admin.GetElectionResults();
                foreach (var item in vM.Results)
                {
                    vM.TotalVotes += item.Votes;
                }
                foreach (var item in vM.Results)
                {
                    decimal x = Convert.ToDecimal(item.Votes);
                    decimal y = Convert.ToDecimal(vM.TotalVotes);
                    item.Percentage = ((x / y) * 100);
                    vM.TotalPercentage += item.Percentage;
                }
                vM.ByVillageResults = admin.GetCrossTab();
                vM.Summary = admin.GetElectionSummary();
                decimal a = Convert.ToDecimal(vM.TotalVotes);
                decimal b = Convert.ToDecimal(vM.Summary.Candidate1);
                decimal c = Convert.ToDecimal(vM.Summary.Candidate2);
                decimal d = Convert.ToDecimal(vM.Summary.Candidate3);
                decimal e = Convert.ToDecimal(vM.Summary.Candidate4);
                decimal f = Convert.ToDecimal(vM.Summary.Candidate5);
                decimal g = Convert.ToDecimal(vM.Summary.Candidate6);
                decimal h = Convert.ToDecimal(vM.Summary.Candidate7);
                decimal i = Convert.ToDecimal(vM.Summary.Candidate8);
                decimal j = Convert.ToDecimal(vM.Summary.Candidate9);
                decimal k = Convert.ToDecimal(vM.Summary.VoterTurnOut);
                decimal l = Convert.ToDecimal(vM.Summary.RegVoter);
                vM.Candidate1Percentage = ((b / a) * 100);
                vM.Candidate2Percentage = ((c / a) * 100);
                vM.Candidate3Percentage = ((d / a) * 100);
                vM.Candidate4Percentage = ((e / a) * 100);
                vM.Candidate5Percentage = ((f / a) * 100);
                vM.Candidate6Percentage = ((g / a) * 100);
                vM.Candidate7Percentage = ((h / a) * 100);
                vM.Candidate8Percentage = ((i / a) * 100);
                vM.Candidate9Percentage = ((j / a) * 100);
                vM.VoterTurnOutPercentage = ((k / l) * 100);
                ViewBag.result = 1;
                ViewBag.Msg = "The Election has Successfully come to a close";
                return View("Reports", vM);
            }
            else
            {
                return View("Index");
            }
        }
        #endregion
    }
}
