using DTO;
using Services.Interface;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
namespace WebApi.Controllers
{
    public class HomeController : ApiController
    {
        private IPostService _ipost;
        public HomeController(IPostService ipost)
        {
            _ipost = ipost;
        }

        [HttpGet]
        public HttpResponseMessage GetTotalCount()
        {
            return Request.CreateResponse(_ipost.GetTotalCount());
        }

        [HttpGet]
        public HttpResponseMessage GetIndex(int page)
        {
            int itemsPerPage = 5;
            List<PostDTO> listPostDTO = _ipost.LoadPosts(page, itemsPerPage);
            return Request.CreateResponse(listPostDTO);
        }
    }
}
