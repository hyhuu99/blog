using AutoMapper;
using DTO;
using Services.Interface;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserService _iuser;
        public UsersController(IUserService iuser)
        {
            _iuser = iuser;
        }


        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Register(UserDTO user)
        {
            int? result = _iuser.Register(user);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetUser(string value)
        {
            UserDTO userDTO = _iuser.GetUserDTO(value);
            return Request.CreateResponse(HttpStatusCode.OK, userDTO);
        }

        [Authorize]
        [HttpPut]
        public HttpResponseMessage Update(UserDTO user)
        {
            bool result = _iuser.IsUpdateSuccessful(user);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
