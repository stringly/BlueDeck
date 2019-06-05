using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class PhoneNumberTypeApiResult
    {
        public int PhoneNumberTypeId { get; set; }
        public string PhoneNumberTypeName { get; set; }

        public PhoneNumberTypeApiResult(PhoneNumberType _phoneNumberType)
        {
            PhoneNumberTypeId = (Int32)_phoneNumberType.PhoneNumberTypeId;
            PhoneNumberTypeName = _phoneNumberType.PhoneNumberTypeName;
        }
    }
}
