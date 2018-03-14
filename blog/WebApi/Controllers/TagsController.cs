using DTO;
using Services.Interface;
using System.Net.Http;
using System.Web.Http;
namespace WebApi.Controllers
{
    public class TagsController : ApiController
    {
        private ITagService _itag;
        private IPostService _ipost;
        public TagsController(ITagService itag, IPostService ipost)
        {
            _itag = itag;
            _ipost = ipost;
        }

        [HttpGet]
        public HttpResponseMessage GetDetailsTag(int id,string slug,int page)
        {
            int itemsPerPage = 5;
            return Request.CreateResponse(_itag.FindFormTag(id,slug,page,itemsPerPage));
        }

        [HttpGet]
        public HttpResponseMessage GetCount(int id)
        {
            return Request.CreateResponse(_itag.CountTags(id));
        }

        [HttpGet]
        public HttpResponseMessage GetRandom()
        {
            return Request.CreateResponse(_itag.GetRamdom());
        }
        [HttpGet]
        public HttpResponseMessage AutoComplete(string value)
        {
            return Request.CreateResponse(_itag.AutoCompleteTag(value));
        }
    }
}
