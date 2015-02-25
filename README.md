##### About Dyysh
Dyysh is an application to publish images to the web from PC.


##### This is server side portion of Dyysh
ASP.NET MVC 5 application to manage uploads from PC clients and to view/delete uploaded images.


##### Key Features (relative to Controllers)
* Authorize clients using RSA CSP (RequestKeyController, AuthorizeController)
  * gives clients server public RSA key if requested
  * receives user's credentials (encrypted with server public RSA key) and his RSA public key
  * sends back user GUID (encrypted with client public RSA key) which is added to every upload
  * all sensitive data transfers is always encrypted
* Upload images (UploadController)
  * receives byte data by HTTP POST from client
  * data consists of encrypted user GUID (with server public RSA key) and non-encrypted image with thumbnail
  * saves file if user has available space and responses with image link to client
* View images and manage user gallery (GalleryController)
  * users can manage only their own images
  * users can select and delete their images (AJAX-based)
  * gallery has pages (AJAX-based)
  * all images have short links where it is resized to match browser dimensions
  
##### Other Features
* Links to images are alphanumeric (alphabet of 62 symbols)
* ASP.NET Identity UserManager and Application DB context stored in the Owin context (accessible throughout application)
* MySql database, which is populated thanks to Entity Framework Code First model and MySql Connector/.NET
* Application stores Dyysh desktop app and xml file for automatic updates (latest version and path to setup)
