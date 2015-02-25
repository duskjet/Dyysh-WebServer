using DyyshWebApplication.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace DyyshWebApplication.Controllers
{
    [Route("api/Upload")]
    public class UploadController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            var result = new HttpResponseMessage();
            string fileUrl;

            // Get form data from POST message
            var formData = await Request.Content.ReadAsMultipartAsync();

            var imageBytes = formData.Contents[0].ReadAsByteArrayAsync().Result;
            var thumbBytes = formData.Contents[1].ReadAsByteArrayAsync().Result;
            var guidBytes = formData.Contents[2].ReadAsByteArrayAsync().Result;
            var fileExt = formData.Contents[3].ReadAsStringAsync().Result;

            // Decrypt received GUID
            var cspParams = new CspParameters
            {
                KeyContainerName = "serverContainer",
                Flags = CspProviderFlags.UseArchivableKey |
                        CspProviderFlags.UseMachineKeyStore
            };
            var serverRsa = new RSACryptoServiceProvider(2048, cspParams);

            var decryptedGuid = serverRsa.Decrypt(guidBytes, true);
            var userGuid = Encoding.Unicode.GetString(decryptedGuid);

            // Identify the user by GUID
            var usermanager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await usermanager.FindByIdAsync(userGuid);
            var usedSpace = user.FileSize_Total;

            // Check if uploader is allowed to store this image
            if (user != null)
                if (usedSpace < ServerConstants.UserStorageSpace)
                {
                    // Save image and thumbnail to disk and DB
                    var context = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();

                    try
                    {
                        var image = new Image
                        {
                            Owner = user,
                            FileSize = imageBytes.Length,
                            Path = "",
                            ThumbnailPath = ""
                        };
                        user.FileSize_Total += image.FileSize;

                        context.Images.Add(image);
                        context.SaveChanges();

                        var fileName = UrlConvert.ToString((long)image.Id) + "." + fileExt;
                        var folderName = UrlConvert.ToStringCatalog((long)image.Id);

                        var virtualPath = "~/Images/" + folderName + "/";
                        var globalPath = System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
                        var thumbPath = globalPath + "Thumbs\\";

                        Directory.CreateDirectory(globalPath);
                        Directory.CreateDirectory(thumbPath);

                        var imageFilePath = globalPath + fileName;
                        var thumbFilePath = thumbPath + fileName;

                        File.WriteAllBytes(imageFilePath, imageBytes);
                        File.WriteAllBytes(thumbFilePath, thumbBytes);

                        image.Path = "~/Images/" + folderName + "/" + fileName;
                        image.ThumbnailPath = "~/Images/" + folderName + "/Thumbs/" + fileName;
                        fileUrl = "/Images/" + folderName + "/" + fileName;

                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new HttpException(e.Message);
                    }

                }
                else
                {
                    // Return "no disk space left"
                    result.StatusCode = HttpStatusCode.InternalServerError;
                    result.ReasonPhrase = "There is no disk space left for this account.";
                    return result;
                }
            else
            {
                // Return "user not found"
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.ReasonPhrase = "Specified user was not found.";
                return result;
            }

            // Return link for image to uploader

            result.StatusCode = HttpStatusCode.OK;
            result.Content = new StringContent(fileUrl, Encoding.Unicode);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
    
}
