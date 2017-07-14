using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Areas.ManageData.ViewModels;
using CTM.Areas.ManageData.ViewModels.EnglishTests;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Models;
using Microsoft.AspNet.Identity;

namespace CTM.Areas.ManageData.Controllers
{
    public class EnglishTestsController : ManageDataControllerBase
    {
        #region MVC Get & Post

        /// <summary>
        /// Get : Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return CreateGet();
        }

        /// <summary>
        /// Post : Create
        /// </summary>
        /// <param name="createViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "CCName,Type,Grade,CategoryID,Date")] Create createViewModel)
        {
            return await CreatePost<EnglishTest>(createViewModel);
        }

        /// <summary>
        /// Get : Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            return await EditGet(id);
        }

        /// <summary>
        /// Post : Edit
        /// </summary>
        /// <param name="englishTest"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CabinCrewID,Type,Grade,CategoryID,Date")] EnglishTest englishTest)
        {
            return await EditPost<EnglishTest>(englishTest);
        }

        /// <summary>
        /// Get : Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(string id)
        {
            return await DeleteGet(id);
        }

        /// <summary>
        /// Post : Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            return  await DeleteConfirmedPost<EnglishTest>(id);
        }

        /// <summary>
        /// Get : Upload
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            return UploadGet();
        }

        /// <summary>
        /// Post : Upload
        /// </summary>
        /// <param name="uploadViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Upload(Upload uploadViewModel)
        {
            return await UploadPost<EnglishTest>(uploadViewModel);
        }

        /// <summary>
        /// Post : Download Template
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadTemplate()
        {
            return DownloadTemplate<UploadTemplate>();
        }

        #endregion

        #region Overridden Methods

        protected override async Task<Model> GetEntityById(string id)
        {
            return await DbManager.GetEntityAsync<EnglishTest>(id);
        }

        #region Create

        protected override ICreate CreateViewModel()
        {
            var createViewModel = new Create()
            {
                CategoryList = new SelectList(DbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.EnglishTest), "ID", "Name"),
            };
            return createViewModel;
        }

        #endregion

        #region Edit

        protected override void EditGetViewBag(Model entity)
        {
            ViewBag.CategoryID = new SelectList(
                DbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.EnglishTest), 
                "ID",
                "Name",
                ((EnglishTest)entity).CategoryID);
        }

        #endregion

        #region Upload

        protected override IUpload UploadViewModel()
        {
            var uploadModelView = new Upload()
            {
                CategoryList =
                    new SelectList(DbManager.Categories.Where(o => o.Type == SuperCategory.EnglishTest), "ID", "Name")
            };

            return uploadModelView;
        }

        protected override UploadRecord GenerateUploadRecord(IUpload iUpload)
        {
            var uploadViewModel = (Upload)iUpload;
            return new UploadRecord()
            {
                ID = uploadViewModel.UploadRecordID,
                CategoryID = uploadViewModel.CategoryID,
                DateTime = DateTime.UtcNow,
                ApplicationUserID = User.Identity.GetUserId(),
                IsWithdrawn = false,
            };
        }

        protected override IUploadTemplate GenerateUploadTemplate(List<string> strList)
        {
            var uploadTemplate = new UploadTemplate()
            {
                CabinCrewID = strList[0],
                CabinCrewName = strList[1],
                CabinAnnoucement = strList[2],
                SpokenSkill = strList[3]
            };
            return uploadTemplate;
        }

        protected override ISearchResultModel ConvertEntityToSearchResult(IModel entity)
        {
            var item = (EnglishTest)entity;
            var searchResult = new SearchResult()
            {
                ID = item.ID,
                CabinCrewID = item.CabinCrewID,
                CabinCrewName = item.CabinCrew.Name,
                CategoryName = item.Category.Name,
                Date = item.Date,
                Grade = item.Grade,
                Type = item.Type
            };
            return searchResult;
        }
        
        protected override List<IModel> ConvertUploadTemplateToEntity(List<IUploadTemplate> listUploadTemplate, IUpload uploadViewModel)
        {
            var list = new List<IModel>();

            // Extra data from upload view model
            var englishTestCategoryId = ((Upload)uploadViewModel).CategoryID;
            var uploadRecordId = ((Upload)uploadViewModel).UploadRecordID;
            var date = ((Upload)uploadViewModel).Date;

            listUploadTemplate.ForEach(o =>
            {
                var item = (UploadTemplate)o;

                // If cabin crew Id and name are null, ignore 
                if (!string.IsNullOrEmpty(item.CabinCrewID) && !string.IsNullOrEmpty(item.CabinCrewName))
                {
                    if (!string.IsNullOrEmpty(item.CabinAnnoucement))
                    {
                        list.Add(
                            // Announcement grade
                            new EnglishTest()
                            {
                                ID = Guid.NewGuid().ToString(),
                                CabinCrewID = item.CabinCrewID,
                                Grade = item.CabinAnnoucement,
                                Type = EnglishTestType.CabinAnnoucement,
                                Date = date.ToUniversalTime().Date,
                                CategoryID = englishTestCategoryId,
                                UploadRecordID = uploadRecordId,
                            }
                        );
                    }

                    if (!string.IsNullOrEmpty(item.SpokenSkill))
                    {
                        list.Add(
                            // Oral English grade
                            new EnglishTest()
                            {
                                ID = Guid.NewGuid().ToString(),
                                CabinCrewID = item.CabinCrewID,
                                Grade = item.SpokenSkill,
                                Type = EnglishTestType.SpokenSkill,
                                Date = date.ToUniversalTime().Date,
                                CategoryID = englishTestCategoryId,
                                UploadRecordID = uploadRecordId,
                            }
                        );
                    }
                }
            });

            return list;
        }

        protected override async Task<IModel> ConvertCreateViewModelToEntity(ICreate iCreate)
        {
            var createViewModel = (Create)iCreate;

            // Cabin Crew
            var cabinCrew = await DbManager.DbSet<CabinCrew>()
                .FirstOrDefaultAsync(o => o.Name.ToLower().Equals(createViewModel.CCName.Trim().ToLower()));
            var category = await DbManager.Categories.FirstOrDefaultAsync(o => o.ID.Equals(createViewModel.CategoryID));

            if (cabinCrew != null)
            {
                EnglishTest englishTest = new EnglishTest()
                {
                    CabinCrewID = cabinCrew.ID,
                    CabinCrew = cabinCrew,
                    CategoryID = createViewModel.CategoryID,
                    Category = category,
                    Type = createViewModel.Type,
                    Date = createViewModel.Date,
                    Grade = createViewModel.Grade
                };
                return englishTest;
            }
            return null;
        }

        #endregion
    }
    #endregion
}
