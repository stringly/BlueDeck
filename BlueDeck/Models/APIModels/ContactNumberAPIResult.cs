namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class object representing a <see cref="ContactNumber"/> in a WebAPI response.
    /// </summary>
    public class ContactNumberApiResult
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The phone number type, which is derived from the name of the <see cref="ContactNumber"/> <see cref="PhoneNumberType"/>
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumberApiResult"/> class.
        /// </summary>
        public ContactNumberApiResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumberApiResult"/> class.
        /// </summary>
        /// <param name="_number">The number.</param>
        public ContactNumberApiResult(ContactNumber _number)
        {
            Type = _number.Type.PhoneNumberTypeName;
            PhoneNumber = _number.PhoneNumber;
        }
    }
}
