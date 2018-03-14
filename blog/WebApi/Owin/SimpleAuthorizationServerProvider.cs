using Database;
using Database.Repository;
using Microsoft.Owin.Security.OAuth;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Owin
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
          
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
        
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            using (MiniBlog db = new MiniBlog())
            {
                GenericRepository<User> _user = new GenericRepository<User>(db);
                User user = _user.Search(c => c.Email == context.UserName && c.Password == context.Password).FirstOrDefault();
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                else
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("sub", context.UserName));
                    identity.AddClaim(new Claim("role", "user"));
                    context.Validated(identity);
                    //string[] role = new string[1] { "user" };
                    //GenericPrincipal MyPrincipal = new GenericPrincipal(identity, role);
                    //IPrincipal Identity = (IPrincipal)MyPrincipal;
                    //HttpContext.Current.User.Identity.Name = identity.Name;
                }
            }
        }
    }
}