using AutoMapper;
using blog.ApiResponse.Interface;
using blog.Models;
using DTO;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;

namespace blog.Controllers
{
    public class HomeController : Controller
    {
        IApiResponse _iapiResponse;
        IMapper _imapper;
        public HomeController( IApiResponse iapiResponse, IMapper imapper)
        {
            _iapiResponse = iapiResponse;
            _imapper = imapper;
        }
        public ActionResult Index(int? page = 1)
        {
            int pagenumber = (page ?? 1) - 1;
            int totalCount = _iapiResponse.Get<int>("home");
            List<PostDTO> listPostDTO = _iapiResponse.Get<List<PostDTO>>("home?page="+pagenumber);
            List<PostVM.PostIndexVM> listIndexPostVM = _imapper.Map<List<PostDTO>, List<PostVM.PostIndexVM>>(listPostDTO);
            IPagedList<PostVM.PostIndexVM> postIndex = new StaticPagedList<PostVM.PostIndexVM>(listIndexPostVM, pagenumber + 1, 5, totalCount);         
            return View(postIndex);            
        }
    }
}