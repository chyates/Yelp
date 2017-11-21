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
    public class ReviewController : Controller
    {
        // ########## ROUTES ##########
        //  /search/all
        //  /search/category={category_id}
        //  /search/category={category_id}&subcategory={subcategory_id}
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

        public ReviewController(YelpContext context)
        {
            // Entity Framework connections
            _context = context;
        }

        [HttpGet]
        [Route("/review/new")]
        public IActionResult NewReview()
        {
            return View();
        }

    }
}