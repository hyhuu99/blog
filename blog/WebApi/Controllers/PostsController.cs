using AutoMapper;
using DTO;
using Services.Interface;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace WebApi.Controllers
{
    public class PostsController : ApiController
    {
        private IPostService _ipost;
        public PostsController(IPostService ipost)
        {
            _ipost = ipost;
        }

        [HttpGet]
        public HttpResponseMessage FindPosts(int id,string slug)
        {
            PostDTO postDTO = _ipost.FindPost(id,slug);

            return Request.CreateResponse(HttpStatusCode.OK, postDTO);
        }

        [HttpGet]
        public HttpResponseMessage IsMySelf(int id, string email)
        {
            bool result = _ipost.IsMySelft(email,id);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Authorize]
        [HttpDelete]
        public HttpResponseMessage DeletePost(int id, string email)
        {
            bool result = _ipost.IsDeleteSuccessful(email,id);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage AutoCompleSearch(string value)
        {
            List<PostDTO> listPostDTO = _ipost.AutoCompleSearch(value);
            
            return Request.CreateResponse(HttpStatusCode.OK, listPostDTO);
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage CreatePost(PostDTO post)
        {
            bool result = _ipost.IsCreateSuccessful(post);
            string name= RequestContext.Principal.Identity.Name;

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Authorize]
        [HttpPut]
        public HttpResponseMessage UpdatePost(PostDTO post)
        {
            int result = _ipost.IsUpdateSuccessful(post);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage PersonalPage(int? id,int page,string email)
        {
            int itemsPerPage = 5;
            List<PostDTO> listPostDTO = new List<PostDTO>();
            if (id !=null)
            {
                listPostDTO = _ipost.GetPersonalPage(id.GetValueOrDefault(), null, page, itemsPerPage);
            }
            else
            {
                listPostDTO = _ipost.GetPersonalPage(0, email, page, itemsPerPage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, listPostDTO);
        }

        [HttpGet]
        public HttpResponseMessage CountPostInUser(string email)
        {
            return Request.CreateResponse(_ipost.GetCountPostInUser(email));
        }
    }
}
