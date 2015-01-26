using DyyshWebApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DyyshWebApplication.Controllers
{
    public class GalleryController : Controller
    {
        [Route("Gallery")]
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            if (Request.Cookies["TipsWasShown"] == null || Request.Cookies["TipsWasShown"].Value == "false")
            {
                Response.Cookies.Add(new HttpCookie("TipsWasShown") { Expires = DateTime.Today.AddYears(10), Value = "true" });
                ViewBag.ShowTips = true;
            }
            else
            {
                ViewBag.ShowTips = false;
            }
            var currentUserId = User.Identity.GetUserId();
            
            IPagedList<Image> imageCollection;

            var context = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            // Get all images from this user
            var user = context.Users.FirstOrDefault<ApplicationUser>(u => u.Id == currentUserId);
                
            var reversedCollection = user.Images.Reverse();
            imageCollection = reversedCollection.ToPagedList(page, ServerConstants.ImagesPerPage);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Images", imageCollection);
            }

            // ViewBag.UsedSpaceInfo = GetUsedSpaceInfo(currentUserId);

            return View(imageCollection);
        }

        [Route("{id}")]
        public ActionResult ShowImage(string id)
        {
            if (id != null)
            {
            var context = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            
                try
                {
                    var decimalId = (Int32)UrlConvert.ToInt64(id);
                    var image = context.Images.FirstOrDefault<Image>(img => img.Id == decimalId) ?? null;
                    return View(image);
                }
                catch (ArgumentException) { return View(); }

            }
            else
                return View();
        }

        [HttpPost]
        public JsonResult DeleteImages(String[] images)
        {
            if (images == null || images.Length == 0) { return Json("You have not selected any items"); }

            var currentUserId = User.Identity.GetUserId();

            var context = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            {
                var user = context.Users.FirstOrDefault<ApplicationUser>(u => u.Id == currentUserId);
                context.Entry(user).State = System.Data.Entity.EntityState.Modified;

                var currentVolume = user.FileSize_Total;
                int emptiedVolume = 0;

                foreach (var complexString in images)
                {
                    var splittedString = complexString.Split('/');
                    var imageId = Convert.ToInt32(splittedString[0]);
                    var fileSize = Convert.ToInt32(splittedString[1]);

                    var image = new Image { Id = imageId };
                    context.Images.Attach(image);

                    emptiedVolume += fileSize;

                    context.Images.Remove(image);

                }

                var resultedVolume = currentVolume - emptiedVolume;
                user.FileSize_Total = resultedVolume;
                context.SaveChanges();

            }
            return Json("Images deleted successfully.");
        }
        
        private string GetUsedSpaceInfo(string userId)
        {
            var user = HttpContext.GetOwinContext().Get<ApplicationUserManager>().FindById(userId);
            
            double totalSpace_inMegaBytes = (double)ServerConstants.UserStorageSpace / 1024 / 1024;
            double usedSpace_inMegaBytes = (double)user.FileSize_Total / 1024 / 1024;
            double usedSpace_inPercents = usedSpace_inMegaBytes * 100 / totalSpace_inMegaBytes;
            int usedSpace_inPercents_Rounded = (int)Math.Round(usedSpace_inPercents);

            // "Used 14% of storage space (14 MB / 100 MB)"
            string outputMessage = "Used " + usedSpace_inPercents_Rounded + "% of storage space (" 
                                    + usedSpace_inMegaBytes + " MB / " + totalSpace_inMegaBytes + " MB)";
            return outputMessage;
        }
	}
}