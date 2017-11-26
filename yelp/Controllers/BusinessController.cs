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
using Microsoft.Extensions.Options;
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
        private readonly IOptions<GoogleMapSettings> _googlemaps;

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

        public BusinessController(
            YelpContext context, 
            IHostingEnvironment env,
            IOptions<GoogleMapSettings> googlemapsettings
        )
        {
            // Entity Framework connections
            _context = context;
            _env = env;
            _googlemaps = googlemapsettings;
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

        private void AddBusHoursError(int BizId)
        {
            // the business already has a 1:1 hours relationship
            string key = "AlwaysOpen";
            string errorMessage = "This business already has a set of business hours assigned. Reference URL: localhost/biz/" + BizId;
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
        public async Task<IActionResult> CreateBiz(NewBizViewModel MainVM)
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
            
            if (bizView.ImageFiles != null)
            {
                // full path to the file in the temp location
                var filePath = Path.GetTempFileName();

                if (bizView.ImageFiles.Length > 0)
                {
                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(bizView.ImageFiles.ContentDisposition);
                    string _fileName = parsedContentDisposition.FileName.Value.Trim();
                    string _fileExtension = Path.GetExtension(_fileName);
                    BizImageLinkImportModel newFileNameVM = new BizImageLinkImportModel();
                    newFileNameVM.FileName = _fileName;
                    newFileNameVM.FileExtension = _fileExtension;
                    var uploadDir = _env.WebRootPath + $@"\img\Biz\{BizId}";
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploadDir, bizView.ImageFiles.FileName), FileMode.Create))
                    {
                        await bizView.ImageFiles.CopyToAsync(fileStream);
                    }
                    // save image link to the database
                    Business UpdateBiz = _context.Businesses.SingleOrDefault(biz => biz.BusinessId == BizId);
                    string ImagePath = $@"/img/Biz/{BizId}/" + bizView.ImageFiles.FileName;
                    ImagePath = ImagePath.Replace("\"", "");
                    ImagePath = ImagePath.Replace("'", "");
                    UpdateBiz.ImageLink = ImagePath;
                    _context.SaveChanges();
                }

            }
            // the business and image were added correctly
            return RedirectToAction("NewBizProperties", new { biz_id = BizId } );
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
                        // the business already has an existing properties record;
                        // update the record instead of creating a new one
                        BusProperties updateBizProperty = new BusProperties(BizPropsVM);
                        CheckProperties.alcohol = updateBizProperty.alcohol;
                        CheckProperties.ambience = updateBizProperty.ambience;
                        CheckProperties.bikeparking = updateBizProperty.bikeparking;
                        CheckProperties.ByApointOnly = updateBizProperty.ByApointOnly;
                        CheckProperties.caters = updateBizProperty.caters;
                        CheckProperties.creditcards = updateBizProperty.creditcards;
                        CheckProperties.delivery = updateBizProperty.delivery;
                        CheckProperties.goodforTimeOfDay = updateBizProperty.goodforTimeOfDay;
                        CheckProperties.groupfriendly = updateBizProperty.groupfriendly;
                        CheckProperties.kidfriendly = updateBizProperty.kidfriendly;
                        CheckProperties.outdoor = updateBizProperty.outdoor;
                        CheckProperties.parkwhere = updateBizProperty.parkwhere;
                        CheckProperties.price = updateBizProperty.price;
                        CheckProperties.reservations = updateBizProperty.reservations;
                        CheckProperties.takeout = updateBizProperty.takeout;
                        CheckProperties.waiter = updateBizProperty.waiter;
                        CheckProperties.wheelchair = updateBizProperty.wheelchair;
                        CheckProperties.wifi = updateBizProperty.wifi;
                        CheckProperties.UpdatedAt = DateTime.Now;
                        _context.SaveChanges();

                        return RedirectToAction("NewBizHours", new { biz_id = biz_id });
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
            return RedirectToAction("NewBizHours", new { biz_id = biz_id });
        }

        // GET: /biz/{biz_id}/bizHours/new
        [HttpGet]
        [Route("/biz/{biz_id}/bizHours/new")]
        [ImportModelState]
        public IActionResult NewBizHours(int biz_id)
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

        // POST: /biz/{biz_id}/bizHours/create
        [HttpPost]
        [Route("/biz/{biz_id}/bizHours/create")]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBizHours(BusHoursViewModel NewBizHrsVM, int biz_id)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Confirm the business does not already have a 1:1 hours record
                try
                {
                    BusHours CheckHours = _context.Hours.SingleOrDefault(biz => biz.BusinessId == biz_id);
                    if (CheckHours != null)
                    {
                        // the business already has an existing hours record
                        // update the record instead of creating a new one
                        BusHours updateBizHours = new BusHours(NewBizHrsVM);
                        CheckHours.AlwaysOpen = updateBizHours.AlwaysOpen;
                        CheckHours.FriClosed = updateBizHours.FriClosed;
                        CheckHours.FriCloseTime = updateBizHours.FriCloseTime;
                        CheckHours.FriOpenTime = updateBizHours.FriOpenTime;
                        CheckHours.MonClosed = updateBizHours.MonClosed;
                        CheckHours.MonCloseTime = updateBizHours.MonCloseTime;
                        CheckHours.MonOpenTime = updateBizHours.MonOpenTime;
                        CheckHours.SatClosed = updateBizHours.SatClosed;
                        CheckHours.SatCloseTime = updateBizHours.SatCloseTime;
                        CheckHours.SatOpenTime = updateBizHours.SatOpenTime;
                        CheckHours.SunClosed = updateBizHours.SunClosed;
                        CheckHours.SunCloseTime = updateBizHours.SunCloseTime;
                        CheckHours.SunOpenTime = updateBizHours.SunOpenTime;
                        CheckHours.ThuClosed = updateBizHours.ThuClosed;
                        CheckHours.ThuCloseTime = updateBizHours.ThuCloseTime;
                        CheckHours.ThuOpenTime = updateBizHours.ThuOpenTime;
                        CheckHours.TueClosed = updateBizHours.TueClosed;
                        CheckHours.TueCloseTime = updateBizHours.TueCloseTime;
                        CheckHours.TueOpenTime = updateBizHours.TueOpenTime;
                        CheckHours.WedClosed = updateBizHours.WedClosed;
                        CheckHours.WedCloseTime = updateBizHours.WedCloseTime;
                        CheckHours.WedOpenTime = updateBizHours.WedOpenTime;
                        CheckHours.UpdatedAt = DateTime.Now;
                        _context.SaveChanges();
                        return RedirectToAction("ViewBiz", new { biz_id = biz_id });

                        // AddBusHoursError(biz_id);
                        // return RedirectToAction("NewBizHours");
                    }
                }
                catch (Exception ex)
                {
                    // there were not any existing businesses with the same name
                    // proceed with code
                }
                BusHours newBizHours = new BusHours(NewBizHrsVM);
                newBizHours.BusinessId = biz_id;
                _context.Hours.Add(newBizHours);
                _context.SaveChanges();
            }
            else
            {
                return RedirectToAction("NewBizHours");
            }
            return RedirectToAction("ViewBiz", new { biz_id = biz_id });
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
                .Include(biz => biz.BusinessHours)
                .Include(biz => biz.BusinessProperty)
                .SingleOrDefault();
            List<Review> ReturnedReviews = _context.Reviews
                .Where(rev => rev.BusinessId == biz_id)
                .Include(rev => rev.user)
                .ToList();
            double AvgRating = 0.0;
            int CountRatings = 0;
            if (ReturnedReviews.Count() > 0)
            {
                AvgRating = ReturnedReviews
                    .Select(rev => rev.Rating)
                    .Average();
                AvgRating = Math.Round(AvgRating, 2);
                CountRatings = ReturnedReviews
                    .Select(rev => rev.Rating)
                    .Count();
            }

            DateTime today = DateTime.Now;
            DateTime _opentime = new DateTime();
            DateTime _closetime = new DateTime();
            string DayOfWeek = today.ToString("ddd");
            bool todayClosed = false;
            switch (DayOfWeek)
            {
                case "Mon":
                    if (CurrBiz.BusinessHours.MonClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.MonOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.MonCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
                case "Tue":
                    if (CurrBiz.BusinessHours.TueClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.TueOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.TueCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
                case "Wed":
                    if (CurrBiz.BusinessHours.WedClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.WedOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.WedCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
                case "Thu":
                    if (CurrBiz.BusinessHours.ThuClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.ThuOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.ThuCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
                case "Fri":
                    if (CurrBiz.BusinessHours.FriClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.FriOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.FriCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
                case "Sat":
                    if (CurrBiz.BusinessHours.SatClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.SatOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.SatCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
                case "Sun":
                    if (CurrBiz.BusinessHours.SunClosed)
                    { todayClosed = true; }
                    else
                    {
                        try
                        {
                            _opentime = (DateTime)CurrBiz.BusinessHours.SunOpenTime;
                            _closetime = (DateTime)CurrBiz.BusinessHours.SunCloseTime;
                        }
                        catch
                        {
                            todayClosed = true;
                        }
                    }
                    break;
            }
            ViewBag.DayOfWeek = DayOfWeek;
            ViewBag.todayClosed = todayClosed;
            ViewBag.todayOpenTime = _opentime.ToString("t");
            ViewBag.todayCloseTime = _closetime.ToString("t");
            if (
                (TimeSpan.Compare(today.TimeOfDay, _opentime.TimeOfDay) > 0) &&
                (TimeSpan.Compare(today.TimeOfDay, _closetime.TimeOfDay) < 0)
            )
            {
                ViewBag.isOpenCloseClass = "text-success";
                ViewBag.isOpenClose = "Open";
            }
            else
            {
                ViewBag.isOpenCloseClass = "text-danger";
                ViewBag.isOpenClose = "Closed";
            }

            string priceDollar = "";
            switch (CurrBiz.BusinessProperty.price)
            {
                case 1:
                    priceDollar = "$";
                    break;
                case 2:
                    priceDollar = "$$";
                    break;
                case 3:
                    priceDollar = "$$$";
                    break;
                case 4:
                    priceDollar = "$$$$";
                    break;
            }
            ViewBag.priceDollar = priceDollar;

            string MapAddress = CurrBiz.Address + ", " + CurrBiz.City + ", " + CurrBiz.State + " " + CurrBiz.ZipCode;
            ViewBag.MapAddress = MapAddress;
            ViewBag.GoogleApiKey = _googlemaps.Value.KeyString;
            ViewBag.Biz = CurrBiz;
            ViewBag.Reviews = ReturnedReviews;
            ViewBag.AvgRating = AvgRating;
            ViewBag.CountRatings = CountRatings;
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