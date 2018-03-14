using AutoMapper;
using blog.Models;
using DTO;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using blog.ApiResponse.Interface;

namespace blog.Controllers
{
    public class TagsController : Controller
    {
        IApiResponse _iapiResponse;
        IMapper _imapper;
        public TagsController(IApiResponse iapiResponse, IMapper imapper)
        {
            _iapiResponse = iapiResponse;
            _imapper = imapper;
        }

        [AllowAnonymous]
        public ActionResult Details(int? id,string slug,int? page = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pagenumber = (page ?? 1) - 1;
            int totalCount = _iapiResponse.Get<int>("tags/"+id);
            List<PostDTO> listPostDTO = _iapiResponse.Get<List<PostDTO>>("tags/"+id.GetValueOrDefault()+"?slug="+slug+"&page="+ pagenumber);
            List<PostVM.PostIndexVM> listIndexPostVM = _imapper.Map<List<PostDTO>, List<PostVM.PostIndexVM>>(listPostDTO);
            ViewBag.NameTag = slug;
            if (!listIndexPostVM.Any())
            {
                return HttpNotFound();
            }
            IPagedList<PostVM.PostIndexVM> postIndex = new StaticPagedList<PostVM.PostIndexVM>(listIndexPostVM, pagenumber + 1, 5, totalCount);
            return View(postIndex);
        }


        [AllowAnonymous]
        public PartialViewResult GetRandomTag()
        {
            List<TagDTO> listTagDTO = _iapiResponse.Get<List<TagDTO>>("tags");
            List<TagVM> listTagVM = _imapper.Map<List<TagDTO>, List<TagVM>>(listTagDTO);
            if(!listTagDTO.Any())
            {
                TagVM tagVM = new TagVM();
                tagVM.Name = "Nothing";
                tagVM.SlugTag = "Nothing";
                tagVM.Frequency = 0;
                listTagVM.Add(tagVM);
            }

            return PartialView("_Widget", listTagVM.ToList());
        }

        [AllowAnonymous]
        public JsonResult AutoCompleteTag(string value)
        {
            List<TagDTO> listTagDTO = _iapiResponse.Get<List<TagDTO>>("tags?value="+value);
            List<TagVM> listTagVM = _imapper.Map<List<TagDTO>, List<TagVM>>(listTagDTO);
            return Json(listTagVM, JsonRequestBehavior.AllowGet);
        }
    }
}
