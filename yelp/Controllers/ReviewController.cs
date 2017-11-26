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
        [Route("/biz/{bizId}/review/new")]
        public IActionResult NewReview(int bizId)
        {   
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int? currUserId = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User thisUser = _context.Users.SingleOrDefault(u => u.UserId == (int)currUserId);

                Business thisBiz = _context.Businesses.SingleOrDefault(b => b.BusinessId == bizId);

                IEnumerable<int> ReviewOptions = Enumerable.Range(1,5);
                ViewBag.thisBiz = thisBiz;
                ViewBag.ReviewOptions = ReviewOptions;

                return View();
            }
        }

        [HttpPost]
        [Route("/biz/{bizId}/review/create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReview(ReviewViewModel model, int bizId)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int? currUserId = HttpContext.Session.GetInt32(LOGGED_IN_ID);
                User thisUser = _context.Users.SingleOrDefault(u => u.UserId == (int)currUserId);
                Business thisBiz = _context.Businesses.SingleOrDefault(b => b.BusinessId == bizId);
                if (ModelState.IsValid)
                {
                    Review newReview = new Review 
                    {
                        UserId = thisUser.UserId,
                        user = thisUser,
                        BusinessId = thisBiz.BusinessId,
                        Business = thisBiz,
                        Rating = model.Rating,
                        ReviewText = model.ReviewText,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    int biz_Id = bizId;
                    _context.Add(newReview);
                    _context.SaveChanges();
                    return RedirectToAction("ViewBiz", "Business", biz_Id);
                }
                return RedirectToAction("NewReview");
            }
        }
    }
}