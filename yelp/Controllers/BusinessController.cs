using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
// my using statements
using System.Linq;
using yelp.Models;
using yelp.Factory;
using yelp.ActionFilters;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace yelp.Controllers
{
    public class BusinessController : Controller
    {
        // ########## ROUTES ##########
        //  /biz/new
        //  /biz/create
        //  /biz/category/create
        //  /biz/subcategory/create
        //  /biz/{biz_id}/bizProps/new
        //  /biz/{biz_id}/bizProps/create
        //  /biz/{biz_id}
        //  /biz/{biz_id}/edit
        //  /biz/{biz_id}/update
        //  /biz/{biz_id}/delete
        //  /biz/{biz_id}/destroy
        //  /biz/cat/create
        //  /biz/subcat/create
        //  /biz/{biz_id}/biz_photos
        //  /biz/{biz_id}/biz_photos/upload
        //  /biz/{biz_id}/biz_photos/destroy
        // ########## ROUTES ##########

        private const string LOGGED_IN_ID = "LoggedIn_Id";
        private const string LOGGED_IN_USERNAME = "LoggedIn_Username";
        private const string LOGGED_IN_FIRSTNAME = "LoggedIn_FirstName";

        private YelpContext _context;
        private IHostingEnvironment _env;

        // Check user login status
        private bool checkLogStatus()
        {
            int? currentUserId = HttpContext.Session.GetInt32(LOGGED_IN_ID);
            if (currentUserId == null)
            {
                TempData["UserError"] = "You must be logged in!";
                return false;
            }
            else
            {
                return true;
            }
        }

        public BusinessController(YelpContext context, IHostingEnvironment env)
        {
            // Entity Framework connections
            _context = context;
            _env = env;
        }

        private void AddBusinessNameError(int BizId)
        {
            // the business name already exists
            string key = "Name";
            string errorMessage = "The business name you provided already exists. Please confirm your business does not already exist, or choose another name to proceed. Reference URL: localhost/biz/" + BizId;
            ModelState.AddModelError(key, errorMessage);
            return;
        }

        private void AddBusPropsError(int BizId)
        {
            // the business already has a 1:1 properties relationship
            string key = "creditcards";
            string errorMessage = "This business already has a set of business properties assigned. Reference URL: localhost/biz/" + BizId;
            ModelState.AddModelError(key, errorMessage);
            return;
        }

        private void AddBusinessImgError()
        {
            // the business image name is not in a proper format
            string key = "ImageLink";
            string errorMessage = "The image you uploaded is not in the proper format. Please try again.";
            ModelState.AddModelError(key, errorMessage);
            return;
        }

        private void AddAlreadyExistsError(string _field)
        {
            // the entry already exists
            string key = _field;
            string errorMessage = "The " + _field + " you entered already exists. Please try again.";
            ModelState.AddModelError(key, errorMessage);
            return;
        }



        // GET: /biz/new
        [HttpGet]
        [Route("/biz/new")]
        [ImportModelState]
        public IActionResult NewBiz()
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            List<BusCategory> Categories = _context.Categories.OrderBy(cat => cat.Category).ToList();
            List<BusCategoryType>CategoryTypes = _context.CategoryTypes.OrderBy(cat => cat.CategoryType).ToList();

            ViewBag.Categories = Categories;
            ViewBag.CategoryTypes = CategoryTypes;
            return View("NewBusiness");
        }

        // POST: /biz/create
        [HttpPost("UploadFiles")]
        [Route("/biz/create")]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBiz(NewBizViewModel MainVM, IFormFile ImageFile)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            int BizId = 0;
            BusinessViewModel bizView = MainVM.BizVM;
            try
            {
                if (ModelState.IsValid)
                {
                    // Confirm the business does not already have a record
                    try
                    {
                        Business CheckBusiness = _context.Businesses.SingleOrDefault(biz => biz.Name == bizView.Name);
                        if (CheckBusiness != null)
                        {
                            // the business name already has an existing record; throw an error
                            AddBusinessNameError(CheckBusiness.BusinessId);
                            return RedirectToAction("NewBiz");
                        }
                    }
                    catch (Exception ex)
                    {
                        // there were not any existing businesses with the same name
                        // proceed with code
                    }
                    Business newBiz = new Business(bizView);
                    _context.Businesses.Add(newBiz);
                    _context.SaveChanges();
                    _context.Entry(newBiz).GetDatabaseValues();
                    BizId = newBiz.BusinessId;
                }
                else
                {
                    return RedirectToAction("NewBiz");
                }
            }
            catch (Exception ex)
            {
                // there were errors in model validation
                return RedirectToAction("NewBiz");
            }

            // the new business saved correctly -- save the image to a folder with the biz ID
            // and update the business record with the link
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(ImageFile.ContentDisposition);
                string _fileName = parsedContentDisposition.FileName.Value.Trim();
                string _fileExtension = Path.GetExtension(_fileName);
                BizImageLinkImportModel newFileNameVM = new BizImageLinkImportModel();
                newFileNameVM.FileName = _fileName;
                newFileNameVM.FileExtension = _fileExtension;
                if (TryValidateModel(newFileNameVM))
                {
                    // filename is in valid format
                    var uploadDir = _env.WebRootPath + $@"\img\Biz\{BizId}";
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploadDir, _fileName), FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    // filename is not in valid format
                    AddBusinessImgError();
                    return RedirectToAction("NewBiz");
                }
            }
            
            // the business and image were added correctly
            return RedirectToAction("NewBizProperties", BizId);
        }

        // POST: /biz/category/create
        [HttpPost]
        [Route("/biz/category/create")]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public IActionResult NewBizCategory(NewBizViewModel MainVM)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (TryValidateModel(MainVM.CategoryVM))
            {
                string _category = Constants.UppercaseFirst(MainVM.CategoryVM.Category);
                // confirm the category does not already exist
                try
                {
                    BusCategory CheckCategory = _context.Categories.SingleOrDefault(cat => cat.Category == _category);
                    if (CheckCategory != null)
                    {
                        // the category already has an existing record; throw an error
                        AddAlreadyExistsError("BizVM.CategoryId");
                        return RedirectToAction("NewBiz");
                    }
                }
                catch (Exception ex)
                {
                    // there were not any matching entries
                }
                BusCategory NewCategory = new BusCategory();
                NewCategory.Category = _category;
                _context.Categories.Add(NewCategory);
                _context.SaveChanges();
            }
            return RedirectToAction("NewBiz");
        }

        // POST: /biz/subcategory/create
        [HttpPost]
        [Route("/biz/subcategory/create")]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public IActionResult NewBizCategoryType(NewBizViewModel MainVM)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (TryValidateModel(MainVM.CategoryTypeVM))
            {
                string _categorytype = Constants.UppercaseFirst(MainVM.CategoryTypeVM.CategoryType);
                // confirm the category does not already exist
                try
                {
                    BusCategoryType CheckCategoryType = _context.CategoryTypes.SingleOrDefault(cat => cat.CategoryType == _categorytype);
                    if (CheckCategoryType != null)
                    {
                        // the category already has an existing record; throw an error
                        AddAlreadyExistsError("BizVM.CategoryTypeId");
                        return RedirectToAction("NewBiz");
                    }
                }
                catch (Exception ex)
                {
                    // there were not any matching entries
                }
                BusCategoryType NewCategoryType = new BusCategoryType();
                NewCategoryType.CategoryType = _categorytype;
                NewCategoryType.CategoryId = MainVM.CategoryTypeVM.CategoryId;
                _context.CategoryTypes.Add(NewCategoryType);
                _context.SaveChanges();
            }
            return RedirectToAction("NewBiz");
        }


        // GET: /biz/{biz_id}/bizProps/new
        [HttpGet]
        [Route("/biz/{biz_id}/bizProps/new")]
        [ImportModelState]
        public IActionResult NewBizProperties(int biz_id)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            Business CurrBiz = _context.Businesses
                .Where(biz => biz.BusinessId == biz_id)
                .Include(biz => biz.Category)
                .Include(biz => biz.CategoryType)
                .Include(biz => biz.BusinessProperty)
                .SingleOrDefault();
            ViewBag.Biz = CurrBiz;
            return View();
        }

        // POST: /biz/{biz_id}/bizProps/create
        [HttpPost]
        [Route("/biz/{biz_id}/bizProps/create")]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBizProperties(BizPropertiesViewModel BizPropsVM, int biz_id)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Confirm the business does not already have a 1:1 properties record
                try
                {
                    BusProperties CheckProperties = _context.Properties.SingleOrDefault(biz => biz.BusinessId == biz_id);
                    if (CheckProperties != null)
                    {
                        // the business already has an existing properties record; throw an error
                        AddBusPropsError(biz_id);
                        return RedirectToAction("NewBizProperties");
                    }
                }
                catch (Exception ex)
                {
                    // there were not any existing businesses with the same name
                    // proceed with code
                }
                BusProperties newBizProperty = new BusProperties(BizPropsVM);
                newBizProperty.BusinessId = biz_id;
                _context.Properties.Add(newBizProperty);
                _context.SaveChanges();
            }
            else
            {
                return RedirectToAction("NewBizProperties");
            }
            return RedirectToAction("ViewBiz", biz_id);
        }

        // GET: /biz/{biz_id}
        [HttpGet]
        [Route("/biz/{biz_id}")]
        public IActionResult ViewBiz(int biz_id)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            Business CurrBiz = _context.Businesses
                .Where(biz => biz.BusinessId == biz_id)
                .Include(biz => biz.Category)
                .Include(biz => biz.CategoryType)
                .Include(biz => biz.BusinessProperty)
                .SingleOrDefault();

            ViewBag.Biz = CurrBiz;
            return View("ViewBusiness");
        }





        // ########## ROUTES ##########
        //  /biz/{biz_id}
        //  /biz/{biz_id}/edit
        //  /biz/{biz_id}/update
        //  /biz/{biz_id}/delete
        //  /biz/{biz_id}/destroy
        //  /biz/{biz_id}/biz_photos
        //  /biz/{biz_id}/biz_photos/upload
        //  /biz/{biz_id}/biz_photos/destroy
        // ########## ROUTES ##########








    }
}