using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class ContactNumberApiResult
    {
        public string Type { get; set; }
        public string PhoneNumber { get; set; }

        public ContactNumberApiResult()
        {
        }
        public ContactNumberApiResult(ContactNumber _number)
        {
            Type = _number.Type.PhoneNumberTypeName;
            PhoneNumber = _number.PhoneNumber;
        }
    }
}
