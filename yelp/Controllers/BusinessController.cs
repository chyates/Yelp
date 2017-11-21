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
    public class BusinessController : Controller
    {
        // ########## ROUTES ##########
        //  /biz/new
        //  /biz/create
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

        public BusinessController(YelpContext context)
        {
            // Entity Framework connections
            _context = context;
        }

        // GET: /biz/{biz_id}
        [HttpGet]
        [Route("/biz/{biz_id}")]
        [ImportModelState]
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
                .SingleOrDefault();


            ViewBag.Biz = CurrBiz;
            return View("ViewBusiness");
        }





        // ########## ROUTES ##########
        //  /biz/new
        //  /biz/create
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








    }
}