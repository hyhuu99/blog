using AutoMapper;
using blog.ApiResponse.Interface;
using blog.Code;
using blog.Models;
using DTO;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Utility.AnotherFunction;
using Utility.String;

namespace blog.Controllers
{
    public class PostsController : Controller
    {
        IMapper _imapper;
        IApiResponse _iapiResponse;


        public PostsController( IApiResponse iapiResponse, IMapper imapper)
        {
            _iapiResponse = iapiResponse;
            _imapper = imapper;
        }
        [AllowAnonymous]
        public ActionResult Details(int? id, string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostDTO postDTO = _iapiResponse.Get<PostDTO>("posts/"+id+"?slug="+slug);
            PostVM.PostDetailVM postVM = _imapper.Map<PostDTO, PostVM.PostDetailVM>(postDTO);
            postVM.ListComment = _imapper.Map<List<CommentDTO>, List<CommentVM>>(postDTO.ListCommentDTO);
            postVM.IsMySelf = _iapiResponse.Get<bool>("posts/" + id + "?email=" + User.Identity.GetUserName());
            TempData["Url"] = slug;
            if (postVM == null)
            {
                return HttpNotFound();
            }
            return View(postVM);
        }


        [Authorize]
        public ActionResult Delete(int? id, string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int result = _iapiResponse.Delete("posts/"+id+"?email="+User.Identity.GetUserName());
            if (result== (int)StatusCode.SUCCESS)
                return RedirectToAction("Index", "Home");
            else
            {
                if (result == (int)StatusCode.UNAUTHORIZED)
                    ModelState.AddModelError("", "You need login again to delete your post");
                else
                    ModelState.AddModelError("", "Delete post is fail");
                return RedirectToAction("Details", "Posts", new { id = id, slug = slug });
            }
        }

        [Authorize]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "post")] PostVM.PostCreateVM post, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                post.Email = User.Identity.GetUserName();
                PostDTO postDTO = new PostDTO();
                postDTO = _imapper.Map<PostVM.PostCreateVM, PostDTO>(post);
                if (file == null)
                {
                    if (ConvertString.ConvertToBool(_iapiResponse.Post<PostDTO>("posts",postDTO)))
                        return RedirectToAction("Index", "Home");
                }
                else if (file != null && ImageFunction.IsImage(file.InputStream))
                {
                    Image resizeImage = ImageFunction.ScaleImage(Image.FromStream(file.InputStream), (int)StatusCode.MAX_WIDTH, (int)StatusCode.MAX_HEIGHT);
                    postDTO.Image = ImageFunction.imageToByteArray(resizeImage);
                    if (ConvertString.ConvertToBool(_iapiResponse.Post<PostDTO>("posts", postDTO)))
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Image is not correct file format");
                    return View(post);
                }
            }
            ModelState.AddModelError("", "Can't upload your post");
            return View(post);
        }


        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id, string slug)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool IsMySelf = _iapiResponse.Get<bool>("posts/" + id + "?email=" + User.Identity.GetUserName());
            if (!IsMySelf)
                return RedirectToAction("Index", "Home");
            PostDTO postDTO = _iapiResponse.Get<PostDTO>("posts/" + id + "?slug=" + slug); 
            PostVM.PostCreateVM postVM = _imapper.Map<PostDTO, PostVM.PostCreateVM>(postDTO);
            TempData["SlugPost"] = slug;
            if (postDTO == null)
            {
                return HttpNotFound();
            }
            return View(postVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "post")] PostVM.PostCreateVM post, HttpPostedFileBase file)
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                post.Email = User.Identity.GetUserName();
                PostDTO postDTO = new PostDTO();
                postDTO = _imapper.Map<PostVM.PostCreateVM, PostDTO>(post);
                if (file == null)
                {
                    postDTO.Image = post.Image;
                    result = ConvertString.ConvertToInt(_iapiResponse.Put<PostDTO>("posts",postDTO)).GetValueOrDefault();
                    if (result == (int)StatusCode.SUCCESS)
                        return RedirectToAction("Details", "Posts", new { id = post.PostId, slug = TempData["SlugPost"] });
                }
                else if (file != null && ImageFunction.IsImage(file.InputStream))
                {
                    Image resizeImage = ImageFunction.ScaleImage(Image.FromStream(file.InputStream), (int)StatusCode.MAX_WIDTH, (int)StatusCode.MAX_HEIGHT);
                    postDTO.Image = ImageFunction.imageToByteArray(resizeImage);
                    result = ConvertString.ConvertToInt(_iapiResponse.Put<PostDTO>("posts", postDTO)).GetValueOrDefault();
                    if (result == (int)StatusCode.SUCCESS)
                        return RedirectToAction("Details", "Posts", new { id = post.PostId, slug = TempData["SlugPost"] });
                }
                else
                {
                    ModelState.AddModelError("", "Image is not correct file format");
                    return View(post);
                }
                if (result == (int)StatusCode.TITLEWASCHANGED || result == (int)StatusCode.CONTENTWASCHANGED || result == (int)StatusCode.TAGWASCHANGED)
                {
                    ModelState.AddModelError("", "You need back to home page and refresh page again to edit");
                    return View(post);
                }
            }
            ModelState.AddModelError("", "Can't upload your post");
            return View(post);
        }

        [AllowAnonymous]
        public JsonResult AutoCompleSearch(string value)
        {
            List<PostDTO> listPostDTO = new List<PostDTO>();
            listPostDTO = _iapiResponse.Get<List<PostDTO>>("posts?value="+value);
            List<PostVM.PostIndexVM> listPostVM = _imapper.Map<List<PostDTO>, List<PostVM.PostIndexVM>>(listPostDTO);
            return Json(listPostVM, JsonRequestBehavior.AllowGet);           
        }
        [AllowAnonymous]
        public ActionResult PersonalPage(int? id, int? page = 1)
        {
            int pagenumber = (page ?? 1) - 1;
            int totalCount = _iapiResponse.Get<int>("posts?email="+User.Identity.GetUserName());
            List<PostDTO> listPostDTO = _iapiResponse.Get<List<PostDTO>>("posts?id=" + id + "&page=" + pagenumber + "&email=" + User.Identity.GetUserName());         
            List<PostVM.PostIndexVM> listIndexPostVM = _imapper.Map<List<PostDTO>, List<PostVM.PostIndexVM>>(listPostDTO);
            CheckStateUser(listIndexPostVM);
            ViewBag.DisplayName = listPostDTO[0].AuthorName;
            IPagedList<PostVM.PostIndexVM> postIndex = new StaticPagedList<PostVM.PostIndexVM>(listIndexPostVM, pagenumber + 1, 5, totalCount);
            return View(postIndex);
        }

        private void CheckStateUser(List<PostVM.PostIndexVM> listIndexPostVM)
        {
            string email = User.Identity.GetUserName();
            if (!string.IsNullOrEmpty(email))
                foreach (PostVM.PostIndexVM postVM in listIndexPostVM)
                {
                    if (postVM.Email.Equals(email))
                        postVM.IsMySelf = true;
                }
        }
    }
}
