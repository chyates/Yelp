using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
// my using statements
using System.Linq;
using yelp.Models;
using yelp.Factory;
using yelp.ActionFilters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace yelp.Controllers
{
    public class SearchController : Controller
    {
        // ########## ROUTES ##########
        //  /search/all
        //  /search/category={category_id}
        //  /search/category={category_id}&subcategory={subcategory_id}
        //  /search/city={city}
        //  /search/city={city}&category={category_id}
        //  /search/city={city}&subcategory={subcategory_id}
        // ########## ROUTES ##########

        private const string LOGGED_IN_ID = "LoggedIn_Id";
        private const string LOGGED_IN_USERNAME = "LoggedIn_Username";
        private const string LOGGED_IN_FIRSTNAME = "LoggedIn_FirstName";

        private YelpContext _context;


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

        public SearchController(YelpContext context)
        {
            // Entity Framework connections
            _context = context;
        }

        [HttpGet]
        [Route("/search/city")]
        public IActionResult InnerCityMainSearch()
        {
            if(checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);
                // List<Business> businessResults = _context.Businesses..ToList();
                List<Business> businessLocations = _context.Businesses.OrderBy(b => b.City).Distinct().ToList();

                ViewBag.Locations = businessLocations;
                ViewBag.User = currentUser;
                // ViewBag.Results = businessResults;
                return View();
            }
        }

        [HttpPost]
        [Route("/search/{city}")]
        public IActionResult CitySearch (string city)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.City.Contains(city)).ToList();
            if(businessResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that city. Try again?";
                return RedirectToAction("InnerCityMainSearch");
            }
            else 
            {   
               ViewBag.Results = businessResults;
               string formatCity = city.Replace(" ", "_");
               return RedirectToAction("InnerCitySearchResults", new {formatCity = formatCity});
            }
        }

        [HttpGet]
        [Route("/search/city/{formatCity}")]
        public IActionResult InnerCitySearchResults(string formatCity)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.City.Contains(formatCity.Replace("_", " "))).ToList();
            List<Business> businessLocations = _context.Businesses.OrderBy(b => b.City).Distinct().ToList();
            int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
            User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);

            ViewBag.User = currentUser;
            ViewBag.Locations = businessLocations;
            if(businessResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that city. Try again?";
                return View();
            }
            else 
            {   
                ViewBag.Results = businessResults;
                return View();
            }
        }

        [HttpGet]
        [Route("/search/category")]
        public IActionResult InnerCatSearch(string search)
        {
            if(checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);
                ViewBag.User = currentUser;
                
                List<Business> businessResults = _context.Businesses.Where(b => b.CategoryType.CategoryType.Contains(search)).ToList();
                // List<Business> businessLocations = _context.Businesses.OrderBy(b => b.City).Distinct().ToList();
                List<BusCategory> businessCats = _context.Categories.OrderBy(c => c.Category).Distinct().ToList();
                if(businessResults.Count < 1)
                {
                    ViewBag.noResults = "No establishments were found in that category. Try again?";
                    return View();
                }
                else 
                {
                    ViewBag.Cats = businessCats;
                    ViewBag.Results = businessResults;
                    return View();
                }
            }
        }

        [HttpPost]
        [Route("/search/{category}")]
        public IActionResult CatSearch (string category)
        {
            // List<Business> businessResults = _context.Businesses.Where(b => b.City.Contains(city)).ToList();
            List<BusCategory> catResults = _context.Categories.Where(c => c.Category.Contains(category)).ToList();
            if(catResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that city. Try again?";
                return RedirectToAction("InnerCatMainSearch");
            }
            else 
            {   
               ViewBag.Results = catResults;
               string formatCat = category.Replace(" ", "_");
               return RedirectToAction("InnerCitySearchResults", new {formatCat = formatCat});
            }
        }

        [HttpGet]
        [Route("/search/category/{formatCat}")]
        public IActionResult InnerCatSearchResults(string formatCat)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.Category.Category.Contains(formatCat.Replace("_", " "))).ToList();
            List<BusCategory> businessCats = _context.Categories.OrderBy(c => c.Category).Distinct().ToList();
            // List<Business> businessLocations = _context.Businesses.OrderBy(b => b.City).Distinct().ToList();
            int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
            User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);

            ViewBag.User = currentUser;
            // ViewBag.Locations = businessLocations;
            if(businessResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that city. Try again?";
                return View();
            }
            else 
            {   
                ViewBag.Cats = businessCats;
                ViewBag.Results = businessResults;
                return View();
            }
        }

        // [HttpPost]
        // [Route("/search/category")]
        // public IActionResult CatSearch (string category)
        // {   
        //     List<Business> businessResults = _context.Businesses.Where(b => b.CategoryType.CategoryType.Contains(category)).ToList();

        //     if(businessResults.Count < 1)
        //     {
        //         ViewBag.noResults = "No establishments were found in that category. Try again?";
        //         return RedirectToAction("InnerCatSearch");
        //     }
        //     else 
        //     {
        //         ViewBag.Results = businessResults;
        //         return RedirectToAction("InnerCatSearch");
        //     }
        // }

        [HttpGet]
        [Route("/search/all")]
        public IActionResult SearchAll ()
        {
            if(checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);
                List<BusCategory> allCats = _context.Categories.OrderBy(c => c.Category).Distinct().ToList();
                List<Business> allLocs = _context.Businesses.GroupBy(b => b.State).Select(b => b.FirstOrDefault()).ToList();

                ViewBag.AllCats = allCats;
                ViewBag.AllLocs = allLocs;
                ViewBag.User = currentUser;

                return View();
            }
        }
    }
}