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
        public IActionResult InnerCitySearch(string search)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.City.Contains(search)).ToList();
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

        [HttpPost]
        [Route("/search/city")]
        public IActionResult CitySearch (string city)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.City.Contains(city)).ToList();
            if(businessResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that city. Try again?";
                return RedirectToAction("InnerCitySearch");
            }
            else 
            {
                ViewBag.Results = businessResults;
                return RedirectToAction("InnerCitySearch");
            }
        }

        [HttpGet]
        [Route("/search/category")]
        public IActionResult InnerCatSearch(string search)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.CategoryType.CategoryType.Contains(search)).ToList();
            if(businessResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that category. Try again?";
                return View();
            }
            else 
            {
                ViewBag.Results = businessResults;
                return View();
            }
        }

        [HttpPost]
        [Route("/search/category")]
        public IActionResult CatSearch (string category)
        {
            List<Business> businessResults = _context.Businesses.Where(b => b.CategoryType.CategoryType.Contains(category)).ToList();
            if(businessResults.Count < 1)
            {
                ViewBag.noResults = "No establishments were found in that category. Try again?";
                return RedirectToAction("InnerCatSearch");
            }
            else 
            {
                ViewBag.Results = businessResults;
                return RedirectToAction("InnerCatSearch");
            }
        }
    }
}