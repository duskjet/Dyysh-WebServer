using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace DyyshWebApplication
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString FormatFileSize(this HtmlHelper htmlHelper, int bytes)
        {
            if (bytes < 500 * 1024)
            {
                var kiloByteValue = ByteConvert.BytesToKiloBytes(bytes);
                var roundedValue = Math.Round(kiloByteValue);

                var formattedValue = roundedValue.ToString() + " KB";

                return MvcHtmlString.Create(formattedValue.ToString());
            }
            else
            {
                var megaByteValue = ByteConvert.BytesToMegaBytes(bytes);
                var roundedValue = Math.Round(megaByteValue, 2);

                var formattedValue = roundedValue.ToString() + " MB";

                return MvcHtmlString.Create(formattedValue.ToString());
            }
        }

        public static MvcHtmlString GetUsedSpaceInfo(this HtmlHelper htmlHelper, string userId)
        {
            return MvcHtmlString.Create
                (HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>().GetUsedSpaceInfo(userId));
        }
    }
}