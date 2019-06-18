using BlueDeck.Models.Enums;
using System;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents a <see cref="AppStatus"/> entity in a WebAPI result set.
    /// </summary>
    public class AppStatusApiResult
    {
        /// <summary>
        /// Gets or sets the application status identifier.
        /// </summary>
        /// <value>
        /// The application status identifier.
        /// </value>
        public int AppStatusId {get;set;}

        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>
        /// The name of the status.
        /// </value>
        public string StatusName {get;set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="AppStatusApiResult"/> class.
        /// </summary>
        /// <param name="_appStatus">An <see cref="AppStatus"/> class object</param>
        public AppStatusApiResult(AppStatus _appStatus)
        {
            AppStatusId = (Int32)_appStatus.AppStatusId;
            StatusName = _appStatus.StatusName;
        }
    }
}
