using System;

namespace ElectionApiFramework.Models
{
    public class AddressDetails
    {
        public int AddressId { get; set; }

        public string Street { get; set; }

        public string Village { get; set; }

        public string Parish { get; set; }

        public string Postcode { get; set; }

        public string Constituancy { get; set; }

        public bool IsCurrent { get; set; }

        public int PersonId { get; set; }
        public Nullable<System.DateTime> DateMoved { get; set; }
    }
}
