using DTO;
using Services.Interface;
using System.Net.Http;
using System.Web.Http;
namespace WebApi.Controllers
{
    
    public class CommentsController : ApiController
    {
        private ICommentService _icomment;
        public CommentsController(ICommentService icomment)
        {
            _icomment = icomment;
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage CreateComments(CommentDTO comment)
        {
            return Request.CreateResponse(_icomment.IsCreateSuccessful(comment));
        }

        [Authorize]
        [HttpDelete]
        public HttpResponseMessage DeleteComments(int id,string email)
        {
            return Request.CreateResponse(_icomment.IsDeleteSuccessful(id,email));
        }
    }
}
