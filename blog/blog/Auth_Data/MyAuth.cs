using System;
using System.Web.Mvc;
namespace blog.Auth_Data
{
    public class MyAuth: FilterAttribute, IActionFilter
    {
        
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.ActionParameters.ContainsKey("id"))
            //{
            //    var id = filterContext.ActionParameters["id"] as Int32?;
            //    string email = filterContext.ActionParameters["slug"] as string;
            //    string email = Microsoft.AspNet.Identity.GetUserName();
            //    bool ok = _ipost.IsMySelft("abc", id.Value);
            //    if (ok == false)
            //    {
            //        filterContext.Result = new RedirectResult("~/home/index");
            //    }
            //}

        }
    }
}