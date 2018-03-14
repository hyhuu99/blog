using AutoMapper;
using blog.ApiResponse.Interface;
using blog.Code;
using blog.Models;
using DTO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Utility.AnotherFunction;
using Utility.String;

namespace blog.Controllers
{
    public class CommentsController : Controller
    {
        IApiResponse _iapiResponse;
        IMapper _imapper;
        public CommentsController(IApiResponse iapiResponse, IMapper imapper)
        {
            _iapiResponse = iapiResponse;
            _imapper = imapper;
        }

        [AllowAnonymous]
        public PartialViewResult CommentPartial(PostVM.PostDetailVM postVM)
        {
            List<CommentVM> listCommentVM = new List<CommentVM>();        
            if (postVM.ListComment.Any())
            {
                CheckStateComment(postVM.ListComment.ToList());
                return PartialView("_CommentPartial", postVM.ListComment.AsEnumerable());
            }
            else
            {
                CommentVM commentVM = new CommentVM();
                commentVM.PostId = postVM.PostId;
                listCommentVM.Add(commentVM);
            }
            return PartialView("_CommentPartial",listCommentVM.AsEnumerable());
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,CommentContent")] CommentVM comment)
        {
            string url = TempData["Url"] as string;
            if (ModelState.IsValid)
            {
                CommentDTO commentDTO = _imapper.Map<CommentVM, CommentDTO>(comment);
                commentDTO.Email= User.Identity.GetUserName();
                int? result = ConvertString.ConvertToInt(_iapiResponse.Post<CommentDTO>("comments",commentDTO));
                UserDTO user = _iapiResponse.Get<UserDTO>("users?value=" + User.Identity.GetUserName());
                comment.DisplayName = user.DisplayName;
                comment.CreateDate = DateTime.Now; 
                comment.ImageUser = ImageFunction.ConvertImage(user.ImageUser);
                if (result == null)
                {
                        ModelState.AddModelError("", "Can not sent your comment");
                }            
                return PartialView("_CommentsPartial", comment);
            }
            return RedirectToAction("Details", "Posts", new { id=comment.PostId,slug = url });
        }

        [Authorize]
        public JsonResult Delete(int value)
        {
            int result = _iapiResponse.Delete("comments/"+value+"?email="+User.Identity.GetUserName());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void CheckStateComment(List<CommentVM> listComment)
        {
            string email = User.Identity.GetUserName();
            if(!string.IsNullOrEmpty(email))
            foreach(CommentVM commentVM in listComment)
            {
                if (commentVM.Email.Equals(email))
                    commentVM.IsMySelf = true;
            }
        }

    }
}
