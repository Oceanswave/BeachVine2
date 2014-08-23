using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SixConcepts.BeachVine.DomainModel
{
    public class ZillowPropertyData
    {
        public string Id
        {
            get;
            set;
        }

        public string ListingId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Id of the property as defined on Zillow.com
        /// </summary>
        public string ZillowPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value returned by Zillow.
        /// </summary>
        public string ZillowSearchResultXml
        {
            get;
            set;
        }
    }
}