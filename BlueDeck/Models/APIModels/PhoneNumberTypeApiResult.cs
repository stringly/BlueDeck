using System;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents a <see cref="PhoneNumberType"/> enumeration used in a WebAPI response.
    /// </summary>
    public class PhoneNumberTypeApiResult
    {
        /// <summary>
        /// Gets or sets the phone number type identifier.
        /// </summary>
        /// <remarks>
        /// This corresponds to the <see cref="PhoneNumberType.PhoneNumberTypeId"/> field.
        /// </remarks>
        /// <value>
        /// The phone number type identifier.
        /// </value>
        public int PhoneNumberTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the phone number type.
        /// </summary>
        /// <remarks>
        /// This correspondes to the <see cref="PhoneNumberType.PhoneNumberTypeName"/> field.
        /// </remarks>
        /// <value>
        /// The name of the phone number type.
        /// </value>
        public string PhoneNumberTypeName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberTypeApiResult"/> class.
        /// </summary>
        /// <param name="_phoneNumberType">A <see cref="PhoneNumberType"/> object.</param>
        public PhoneNumberTypeApiResult(PhoneNumberType _phoneNumberType)
        {
            PhoneNumberTypeId = (Int32)_phoneNumberType.PhoneNumberTypeId;
            PhoneNumberTypeName = _phoneNumberType.PhoneNumberTypeName;
        }
    }
}
