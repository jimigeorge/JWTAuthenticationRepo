# ASP.NetWebAPIandMVC
This is a JWT Authentication Project using ASP.Net WebAPI server and MVC Client application.

Use Cases:

1. Using the Client Application, allow Users to Register and Login to view protected resources on the server. 
2. Prevent anonymous users from viewing secured pages.
3. Once the user is logged in successfully, the system should not ask for credentials when he/she tries to view a secured page.
4. Allow the user to log out.

Front-End - Using AP.Net MVC6
JsonWebTokenAuthentication.Client

. The user can register with UserName, Password , Confirm Password. 
. The user can Login with Username and Password and upon successful authentication, receives a signed JWT and saves the JWT in a cookie.
. This token is then attached with the Authentication Header (Bearer {your token}) with each request to the secured resources. 
. The client application stores this token in a cookie and access this token and uses it until its valid.
. There is an expiration property attached with the JWT (In this case 30 minuts)
. The User Can View Orders, Create, Update and Delete Orders as long as the cookie is valid.
. The User has an option to Logout. On Logout , the cookie that holds the JWT gets deleted.  


Back-End - Using Web API2, OWIN Middleware, ASP.Net Identity System - Created using .Net Framework 4.6.1, Visual Studio 2017(Community Edition)
JsonWebTokenAuthentication.API

. The Server API uses Token based approach to authenticate the users.
. The server validates the user credentials and issues a signed JWT token if credentials are valid.
. The token has an expiration period set and will not be valid after the period.

Implemented a Web API DeligatingHandler TokenValidationHandler whch validates the token is valid or not.

NuGet packages Used:
Back-End 

. Microsoft.Owin version="4.0.0" 
. Microsoft.Owin.Cors version="4.0.0" 
. Microsoft.Owin.Host.SystemWeb version="4.0.0"
. Microsoft.Owin.Security version="4.0.0" 
. Microsoft.Owin.Security.Cookies version="4.0.0" 
. Microsoft.Owin.Security.Jwt version="4.0.0" 
. Microsoft.Owin.Security.OAuth version="4.0.0" 
. Microsoft.IdentityModel.JsonWebTokens" version="5.3.0" 
. Microsoft.IdentityModel.Logging" version="5.3.0"
. Microsoft.IdentityModel.Tokens" version="5.3.0"

Front-End

. Microsoft.AspNet.Web.Optimization version="1.1.3" 
. Microsoft.AspNet.WebApi.Client version="5.2.6" 
. Microsoft.AspNet.WebPages version="3.2.3" 
. Newtonsoft.Json version="6.0.4" 
. System.Net.Http version="4.3.4" 

Please refer Package.Config file for missing packages/versions

