using DyyshWebApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DyyshWebApplication
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            return manager;
        }

        public string GetUsedSpaceInfo(string userId)
        {
            //var user = context.Users.FirstOrDefault(u => u.Id == userId);
            var user = this.FindById(userId);

            double totalSpace_inMegaBytes = ByteConvert.BytesToMegaBytes(ServerConstants.UserStorageSpace);
            double usedSpace_inMegaBytes = Math.Round( ByteConvert.BytesToMegaBytes(user.FileSize_Total), 2);
            int usedSpace_inPercents = (int)Math.Round( usedSpace_inMegaBytes * 100.0 / totalSpace_inMegaBytes );

            // "Used 14% of storage space (14 MB / 100 MB)"
            string outputMessage = "Used " + usedSpace_inPercents + "% of storage space ("
                                    + usedSpace_inMegaBytes + " MB / " + totalSpace_inMegaBytes + " MB)";
            return outputMessage;
            
        }
    }

}