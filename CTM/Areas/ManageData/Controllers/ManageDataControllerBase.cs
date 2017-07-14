using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Areas.ManageData.ViewModels;
using CTM.Areas.Search.ViewModels;
using CTM.Codes.Helpers;
using CTM.Controllers;
using CTM.Models;
using System.Net;

namespace CTM.Areas.ManageData.Controllers
{
    public abstract class ManageDataControllerBase : BaseController
    {
        #region Fields

        private static CultureInfo culture = new CultureInfo("zh-CN");

        #endregion

        #region MVC GET & POST 

        public ActionResult CreateGet()
        {
            var createViewModel = CreateViewModel();
            return PartialView("_CreatePartial", createViewModel);
        }

        public async Task<ActionResult> CreatePost<T>(ICreate createViewModel) where T : class
        {
            if (ModelState.IsValid)
            {
                var entity = await ConvertCreateViewModelToEntity(createViewModel);
                await DbManager.Add((T)entity);
                await DbManager.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public async Task<ActionResult> EditGet(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = await GetEntityById(id);
            if (entity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditGetViewBag(entity);

            return PartialView("_EditPartial", entity);
        }

        public async Task<ActionResult> EditPost<T>(IModel entity) where T : class
        {
            if (ModelState.IsValid)
            {
                await DbManager.Update((T)entity);
                await DbManager.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public async Task<ActionResult> DeleteGet(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity =await  GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            return PartialView("_DeletePartial",entity);
        }

        public async Task<ActionResult> DeleteConfirmedPost<T>(string id) where T : class
        {
            try
            {
                await DbManager.Remove<T>(id);
                await DbManager.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public ActionResult UploadGet()
        {
            var uploadModelView= UploadViewModel();

            return PartialView("_UploadPartial", uploadModelView);
        }

        public async Task<ActionResult> UploadPost<T>(IUpload uploadViewModel)
        {
            // Get EXCEL File
            var fileBase = Request.Files.Get(0);
            if (fileBase != null && fileBase.ContentLength > 0)
            {
                // Check file type
                if (ExcelHelper.CheckIsExcel(fileBase))
                {
                    // Save to TABLE GenerateUploadRecord
                    await DbManager.Add(GenerateUploadRecord(uploadViewModel));

                    var entity = GetEntityListFromExcel(fileBase.InputStream, uploadViewModel);

                    // Save to database
                    if (entity.Any())
                    {
                        DbManager.AddRange(entity as List<T>);
                    }

                    await DbManager.GetContext().SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

                    return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Get entities from excel stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="uploadViewModel"></param>
        /// <returns></returns>
        protected List<IModel> GetEntityListFromExcel(Stream stream, IUpload uploadViewModel)
        {
            var uploadTemplateList = GetUploadTemplateListFromExcel(stream);
            return ConvertUploadTemplateToEntity(uploadTemplateList, uploadViewModel);
        }

        /// <summary>
        /// Get UploadTemplate model list from excel stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected List<IUploadTemplate> GetUploadTemplateListFromExcel(Stream stream)
        {
            var listUploadTemplate = new List<IUploadTemplate>();
            var colCount = ModelHelper.GetDisplayPropertyCount(listUploadTemplate.FirstOrDefault()?.GetType());
            var listReadFromExcel = ExcelHelper.ReadFromExcel(stream, colCount);
            listReadFromExcel.ForEach(o =>
            {
                var uploadTemplate = GenerateUploadTemplate(o);
                listUploadTemplate.Add(uploadTemplate);
            });

            return listUploadTemplate;
        }

        /// <summary>
        /// Get entity Json by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetEntityJson(string id)
        {
            var entity = await GetEntityById(id);
            var searchResult = ConvertEntityToSearchResult(entity);
            var result = ModelHelper.GetDisplayPropertyNameAndValues(searchResult);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download Template
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected ActionResult DownloadTemplate<T>()
        {
            // Cabin crew names 
            var listDb = DbManager.CabinCrews.ToList();
            var list = listDb.Where(c => c.IsResigned == false)
                .Select(c => new { c.ID, c.Name })
                .OrderBy(o => o.Name, StringComparer.Create(culture, false))
                .ToList();

            // Create upload template models
            var uploadTemplateModels = new List<UploadTemplateBase>();
            list.ForEach(o =>
            {
                uploadTemplateModels.Add(new UploadTemplateBase() { CabinCrewID = o.ID, CabinCrewName = o.Name });
            });

            // Header
            var cabinCrewHeader = ModelHelper<UploadTemplateBase>.GetAllProperties();
            var otherHeader = ModelHelper<T>.GetAllProperties();
            otherHeader.RemoveRange(otherHeader.Count - cabinCrewHeader.Count, cabinCrewHeader.Count);
            var header = cabinCrewHeader.Concat(otherHeader).ToList();
            var stream = ExcelHelper.GenerateExcel(CTMLocalizationLib.Resources.ConstViews.DownloadTemplate, header,
                uploadTemplateModels);

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                CTMLocalizationLib.Resources.ConstViews.DownloadTemplate + ".xlsx");
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract Task<Model> GetEntityById(string id);

        /// <summary>
        /// CreateViewModel
        /// </summary>
        /// <returns></returns>
        protected abstract ICreate CreateViewModel();

        /// <summary>
        /// ViewBag in Edit page
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void EditGetViewBag(Model entity);

        /// <summary>
        /// Upload View Model
        /// </summary>
        /// <returns></returns>
        protected abstract IUpload UploadViewModel();

        /// <summary>
        /// Generate a UploadRecord entity
        /// </summary>
        /// <param name="uploadViewModel"></param>
        /// <returns></returns>
        protected abstract UploadRecord GenerateUploadRecord(IUpload uploadViewModel);

        /// <summary>
        /// Get UploadTemplate from string list
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        protected abstract IUploadTemplate GenerateUploadTemplate(List<string> strList);

        /// <summary>
        /// Get entity list from UploadTemplate
        /// </summary>
        /// <param name="listUploadTemplate"></param>
        /// <param name="uploadViewModel"></param>
        /// <returns></returns>
        protected abstract List<IModel> ConvertUploadTemplateToEntity(List<IUploadTemplate> listUploadTemplate,
            IUpload uploadViewModel);

        /// <summary>
        /// Convert Create view model to entity
        /// </summary>
        /// <param name="iCreate"></param>
        /// <returns></returns>
        protected abstract Task<IModel> ConvertCreateViewModelToEntity(ICreate iCreate);

        /// <summary>
        /// Convert entity to SearchResult model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract ISearchResultModel ConvertEntityToSearchResult(IModel entity);
        #endregion
    }
}
