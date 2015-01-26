using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DyyshWebApplication
{
    public static class ServerConstants
    {
        /// <summary>
        /// Number of images per page. Used by PagedList
        /// </summary>
        public const int ImagesPerPage = 50;

        /// <summary>
        /// Upload space for every user.
        /// </summary>
        public const int UserStorageSpace = 100 * 1024 * 1024; // resulting value in MegaBytes
    }
}