using System;

namespace ElectionApiFramework.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string Street { get; set; }

        public int Village { get; set; }

        public int Parish { get; set; }

        public string Postcode { get; set; }

        public int Constituancy { get; set; }

        public bool IsCurrent { get; set; }

        public int PersonId { get; set; }
        public Nullable<System.DateTime> DateMoved { get; set; }
    }
}
