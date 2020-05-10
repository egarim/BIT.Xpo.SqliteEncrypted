using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class Customer : XPObject
    {
        public Customer(Session session) : base(session)
        { }


        decimal maxCredit;
        bool inactive;
        DateTime createdOn;
        string address;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }

        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }

        public bool Inactive
        {
            get => inactive;
            set => SetPropertyValue(nameof(Inactive), ref inactive, value);
        }
        
        public decimal MaxCredit
        {
            get => maxCredit;
            set => SetPropertyValue(nameof(MaxCredit), ref maxCredit, value);
        }

    }
}
