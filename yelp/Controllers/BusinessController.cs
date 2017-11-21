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

            ViewBag.Categories = Categories;
            return View("NewBusiness");
        }

        // POST: /biz/create
        [HttpGet]
        [Route("/biz/")]
        [ExportModelState]
        public IActionResult CreateBiz(BusinessViewModel bizView)
        {
            if (checkLogStatus() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            // if (TryValidateModel(userVM.registerVM))
            // {
                // // model validated correctly --> success
                // // confirm that a user does not exist with the selected username
                // try
                // {
                //     // Dapper connection commands
                //     // User testUser = userFactory.FindByUsername(userVM.registerVM.Username);

                //     // Entity PostGres Code First command
                //     User testUser = _context.Users.SingleOrDefault(user => user.Email == userVM.registerVM.Email);
                //     if (testUser != null)
                //     {
                //         // the username currently exists in the database
                //         string key = "Username";
                //         string errorMessage = "This username already exists. Please select another or login.";
                //         ModelState.AddModelError(key, errorMessage);
                //         TempData["errors"] = true;
                //         return RedirectToAction("Index");
                //     }
                // }
                // catch
                // {
                //     // if username was not found - do nothing and proceed
                // }
                // // confirm that a user does not exist with the selected email
                // try
                // {
                //     // Dapper connection commands
                //     // User testUser = userFactory.FindByEmail(userVM.registerVM.Email);

                //     // Entity PostGres Code First command
                //     User testUser = _context.Users.SingleOrDefault(user => user.Email == userVM.registerVM.Email);
                //     if (testUser != null)
                //     {
                //         // the email currently exists in the database
                //         string key = "Email";
                //         string errorMessage = "This email address already exists. Please select another or login.";
                //         ModelState.AddModelError(key, errorMessage);
                //         TempData["errors"] = true;
                //         return RedirectToAction("Index");
                //     }
                // }
                // catch
                // {
                //     // if email was not found - do nothing and proceed
                // }
                // // Dapper factory command
                // // userFactory.Add(userVM.registerVM);

                // // Entity PostGres Code First command
                // User NewUser = new User(userVM.registerVM);

                // // generate a 128-bit salt using a secure PRNG
                // byte[] newSalt = new byte[128 / 8];
                // using (var rng = RandomNumberGenerator.Create())
                // {
                //     rng.GetBytes(newSalt);
                // }
                // string newSaltString = Convert.ToBase64String(newSalt);
                // NewUser.Salt = newSaltString;
                // // hash password
                // string SaltedPasswd = NewUser.Password + newSaltString;
                // PasswordHasher<User> Hasher = new PasswordHasher<User>();
                // NewUser.Password = Hasher.HashPassword(NewUser, SaltedPasswd);

                // _context.Users.Add(NewUser);
                // _context.SaveChanges();
                // string userSerialized = JsonConvert.SerializeObject(userVM.registerVM);
                // TempData["user"] = (string)userSerialized;

                // store user id, first name, and username in session
                // run query to gather id number generated by the database
                // Dapper connection command
                // User NewUser = userFactory.FindByUsername(userVM.registerVM.Username);

                // Entity PostGres Code First command
                // User UserFromDb = _context.Users.SingleOrDefault(user => user.Email == userVM.registerVM.Email);
                // HttpContext.Session.SetString(LOGGED_IN_FIRSTNAME, UserFromDb.FirstName);

                return View("ViewBusiness");
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