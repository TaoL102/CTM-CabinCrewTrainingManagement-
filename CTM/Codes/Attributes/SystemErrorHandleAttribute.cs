using System;
using System.Web.Mvc;

namespace CTM.Codes.Attributes
{
    public class SystemErrorHandleAttribute : HandleErrorAttribute
    {

        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }



        public override void OnException(ExceptionContext filterContext)
        {

            // Save to local file
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Log\Exception.txt";
            System.IO.File.AppendAllText(path, filterContext.Exception.ToString());


            //if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            //{
            //    return;
            //}

            //// if the request is AJAX return JSON else view.
            //if (IsAjax(filterContext))
            //{
            //    //Because its a exception raised after ajax invocation
            //    //Lets return Json
            //    filterContext.Result = new JsonResult()
            //    {
            //        Data = filterContext.Exception.Message,
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    };

            //   var test= new HttpStatusCodeResult(HttpStatusCode.BadRequest,filterContext.Exception.Message);

            //    filterContext.Result = test;
            //    filterContext.ExceptionHandled = true;
            //    filterContext.HttpContext.Response.Clear();
            //}
            //else
            //{
            //Normal Exception
            //So let it handle by its default ways.

            //  }

            base.OnException(filterContext);


               

        }
    }
}