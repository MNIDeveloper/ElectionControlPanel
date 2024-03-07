using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionApiFramework.Interfaces
{
    public interface IElection
    {
        Boolean Login(int VotersID, string Pin);
        Person GetPerson(int VotersID);
        Boolean AddVotes(Election election);
    }
}
