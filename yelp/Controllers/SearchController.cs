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
        [Route("/search")]
        public IActionResult MainSearch(string search, string category)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);
                ViewBag.User = currentUser;
                
                if (search != null)
                {
                    if (category == "Name")
                    {
                        List<Business> NameSearch = _context.Businesses.Where(b => b.Name.ToLower().Contains(search.ToLower())).ToList();
                        return View(NameSearch);
                    }
                    if (category == "City")
                    {
                        List<Business> CitySearch = _context.Businesses.Where(b => b.City.ToLower().Contains(search.ToLower())).ToList();
                        return View(CitySearch);
                    }
                    if (category == "Category")
                    {
                        List<Business> CategorySearch = _context.Businesses.Where(b => b.Category.Category.ToLower() == search.ToLower()).ToList();
                        return View(CategorySearch);
                    }
                }

                // List<Business> model = _context.Businesses.ToList();
                return View();
            }
        }

        [HttpPost]
        [Route("/find")]
        public IActionResult Search(string searchContent, string categoryContent)
        {

            return RedirectToAction("MainSearch", new {search = searchContent, category = categoryContent});
        }

        [HttpGet]
        [Route("/biz/city/{city}")]
        public IActionResult CityList(string city)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int? currUserID = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User currentUser = _context.Users.SingleOrDefault(u => u.UserId == currUserID);
                // List<BusCategory> allCats = _context.Categories.OrderBy(c => c.Category).Distinct().ToList();
                List<Business> allLocs = _context.Businesses.Include(b => b.Category).Where(b => b.City == city).ToList();

                ViewBag.City = city;
                // ViewBag.AllCats = allCats;
                ViewBag.AllLocs = allLocs;
                ViewBag.User = currentUser;

                return View();
            }
        }
    }
}