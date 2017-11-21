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
    public class HomeController : Controller
    {
        // ########## ROUTES ##########
        //  /
        //  /(add_routes_guide)
        //  /
        // ########## ROUTES ##########

        // Dapper connections
        // private readonly UserFactory userFactory;
        // private readonly DbConnector _dbConnector;

        // Entity PostGres Code First connection
        private YelpContext _context;

        private const string LOGGED_IN_ID = "LoggedIn_Id";
        private const string LOGGED_IN_USERNAME = "LoggedIn_Username";
        private const string LOGGED_IN_FIRSTNAME = "LoggedIn_FirstName";

        private void AddLoginError()
        {
            // the email and password combination were not found
            string key = "login";
            string errorMessage = "The email and password combination you provided were not valid.";
            ModelState.AddModelError(key, errorMessage);
            return;
        }



        public HomeController(YelpContext context)
        {
            // Dapper framework connections
            // _dbConnector = connect;
            // userFactory = new UserFactory();

            // Entity Framework connections
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        [ImportModelState]
        public IActionResult Index()
        {
            List<Review> allReviews = _context.Reviews.Include(r => r.user).Take(3).ToList();
            List<Business> allBusinesses = _context.Businesses.Include(b => b.Category).Take(3).ToList();
            List<Business> businessLocations = _context.Businesses.OrderBy(b => b.City).Distinct().Take(3).ToList();

            ViewBag.Businesses = allBusinesses;
            ViewBag.Locations = businessLocations;
            ViewBag.RecentReviews = allReviews;
            return View();
        }












        // POST: /login
        [HttpPost]
        [Route("login")]
        [ExportModelState]
        public IActionResult Login(LoginRegFormModel userVM)
        {
            if (TryValidateModel(userVM.loginVM))
            {
                try
                {
                    // Dapper Factory command
                    // User logging_user = userFactory.FindByLogin(userVM.loginVM.Username, userVM.loginVM.Password);

                    // Entity PostGres Code First command
                    // retrieve user by submitted username
                    User logging_user = _context.Users.SingleOrDefault(user => user.Email == userVM.loginVM.Email);
                    // salt the submitted password and hash
                    string SaltedPasswd = userVM.loginVM.Password + logging_user.Salt;
                    var Hasher = new PasswordHasher<User>();

                    if (0 != Hasher.VerifyHashedPassword(logging_user, logging_user.Password, SaltedPasswd))
                    {
                        // the passwords match!
                        HttpContext.Session.SetInt32(LOGGED_IN_ID, logging_user.UserId);
                        HttpContext.Session.SetString(LOGGED_IN_USERNAME, userVM.loginVM.Email);
                        HttpContext.Session.SetString(LOGGED_IN_FIRSTNAME, logging_user.FirstName);
                        return RedirectToAction("ProfessionalProfile", "Network");
                    }
                    // else (password failed) -- place error in ModelState below
                    AddLoginError();
                }
                catch (Exception ex)
                {
                    // the username and password combination were not found
                    AddLoginError();
                }
            }
            // if login was not successful, return to index with errors exported in modelstate
            TempData["login_errors"] = true;
            return RedirectToAction("LandingPage");
        }

        // GET: /logout
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // POST: /register
        [HttpPost]
        [Route("register")]
        [ExportModelState]
        public IActionResult Register(LoginRegFormModel userVM)
        {
            if (TryValidateModel(userVM.registerVM))
            {
                // model validated correctly --> success
                // confirm that a user does not exist with the selected username
                try
                {
                    // Dapper connection commands
                    // User testUser = userFactory.FindByUsername(userVM.registerVM.Username);

                    // Entity PostGres Code First command
                    User testUser = _context.Users.SingleOrDefault(user => user.Email == userVM.registerVM.Email);
                    if (testUser != null)
                    {
                        // the username currently exists in the database
                        string key = "Username";
                        string errorMessage = "This username already exists. Please select another or login.";
                        ModelState.AddModelError(key, errorMessage);
                        TempData["errors"] = true;
                        return RedirectToAction("LandingPage");
                    }
                }
                catch
                {
                    // if username was not found - do nothing and proceed
                }
                // confirm that a user does not exist with the selected email
                try
                {
                    // Dapper connection commands
                    // User testUser = userFactory.FindByEmail(userVM.registerVM.Email);

                    // Entity PostGres Code First command
                    User testUser = _context.Users.SingleOrDefault(user => user.Email == userVM.registerVM.Email);
                    if (testUser != null)
                    {
                        // the email currently exists in the database
                        string key = "Email";
                        string errorMessage = "This email address already exists. Please select another or login.";
                        ModelState.AddModelError(key, errorMessage);
                        TempData["errors"] = true;
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    // if email was not found - do nothing and proceed
                }
                // Dapper factory command
                // userFactory.Add(userVM.registerVM);

                // Entity PostGres Code First command
                User NewUser = new User(userVM.registerVM);

                // generate a 128-bit salt using a secure PRNG
                byte[] newSalt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(newSalt);
                }
                string newSaltString = Convert.ToBase64String(newSalt);
                NewUser.Salt = newSaltString;
                // hash password
                string SaltedPasswd = NewUser.Password + newSaltString;
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, SaltedPasswd);

                _context.Users.Add(NewUser);
                _context.SaveChanges();
                string userSerialized = JsonConvert.SerializeObject(userVM.registerVM);
                TempData["user"] = (string)userSerialized;

                // store user id, first name, and username in session
                // run query to gather id number generated by the database
                // Dapper connection command
                // User NewUser = userFactory.FindByUsername(userVM.registerVM.Username);

                // Entity PostGres Code First command
                User UserFromDb = _context.Users.SingleOrDefault(user => user.Email == userVM.registerVM.Email);

                // login to the application
                HttpContext.Session.SetInt32(LOGGED_IN_ID, UserFromDb.UserId);
                HttpContext.Session.SetString(LOGGED_IN_USERNAME, UserFromDb.Email);
                HttpContext.Session.SetString(LOGGED_IN_FIRSTNAME, UserFromDb.FirstName);
                return RedirectToAction("Index");
            }
            // model did not validate correctly --> show errors to user
            TempData["errors"] = true;
            return RedirectToAction("Index");
        }





    }
}