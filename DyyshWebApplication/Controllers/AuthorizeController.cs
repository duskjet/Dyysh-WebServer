using Microsoft.AspNet.Identity.Owin;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace DyyshWebApplication.Controllers
{
    [Route("api/Authorize")]
    public class AuthorizeController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            // Received data from client: 0 for username, 1 for password, 2 for user rsa key
            var formData = await Request.Content.ReadAsMultipartAsync();
            var usernameBytes = formData.Contents[0].ReadAsByteArrayAsync().Result;
            var passwordBytes = formData.Contents[1].ReadAsByteArrayAsync().Result;

            // Create new RSA service provider with server key
            var cspParams = new CspParameters
            {
                KeyContainerName = "serverContainer",
                Flags = CspProviderFlags.UseArchivableKey |
                        CspProviderFlags.UseMachineKeyStore
            };
            var serverRsa = new RSACryptoServiceProvider(2048, cspParams);

            // Get string data from received form for ASP.NET Identity Provider
            var usernameString = Encoding.Unicode.GetString(
                serverRsa.Decrypt(usernameBytes, true));
            var passwordString = Encoding.Unicode.GetString(
                serverRsa.Decrypt(passwordBytes, true));

            var usermanager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await usermanager.FindAsync(usernameString, passwordString);

            if (user != null)
            {
                // Create new RSA service provider with received user key
                var clientRsa = new RSACryptoServiceProvider(2048);

                var rsaKey = formData.Contents[2].ReadAsByteArrayAsync().Result;
                clientRsa.ImportCspBlob(rsaKey);

                var guidBytes = Encoding.Unicode.GetBytes(user.Id);
                byte[] encryptedGuid = clientRsa.Encrypt(guidBytes, true);

                // Create new response message and put encrypted Guid in it
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(encryptedGuid);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return result;
            }

        }
    }
}
