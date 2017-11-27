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

        private string findStateName(string zipCode)
        {
            int numZip = Int32.Parse(zipCode);
            string fullName = "";
            if (numZip >= 35004 && numZip <= 36925)
            {
                fullName = "Alabama";
            }
            else if (numZip >= 99501 && numZip <= 99950)
            {
                fullName = "Alaska";
            }
            else if ((numZip >= 71601 && numZip <= 75929) || numZip == 75502)
            {
                fullName = "Arkansas";
            }
            else if (numZip >= 85001 && numZip <= 86556)
            {
                fullName = "Arizona";
            }
            else if (numZip >= 90001 && numZip <= 96162)
            {
                fullName = "California";
            }
            else if (numZip >= 80001 && numZip <= 81658)
            {
                fullName = "Colorado";
            }
            else if (numZip >= 6001 && numZip <= 6389)
            {
                fullName = "Connecticut";
            }
            else if (numZip >= 20001 && numZip <= 20799)
            {
                fullName = "Washington, D.C.";
            }
            else if (numZip >= 19701 && numZip <= 19980)
            {
                fullName = "Delaware";
            }
            else if (numZip >= 32004 && numZip <= 34997)
            {
                fullName = "Florida";
            }
            else if ((numZip >= 30001 && numZip <= 31999) || numZip == 39901)
            {
                fullName = "Georgia";
            }
            else if (numZip >= 96701 && numZip <= 96898)
            {
                fullName = "Hawaii";
            }
            else if ((numZip >= 50001 && numZip <= 52809) || (numZip >= 68119 && numZip <= 68120))
            {
                fullName = "Iowa";
            }
            else if (numZip >= 83201 && numZip <= 83876)
            {
                fullName = "Idaho";
            }
            else if (numZip >= 60001 && numZip <= 62999)
            {
                fullName = "Illinois";
            }
            else if (numZip >= 46001 && numZip <= 47997)
            {
                fullName = "Indiana";
            }
            else if (numZip >= 66002 && numZip <= 67954)
            {
                fullName = "Kansas";
            }
            else if (numZip >= 40003 && numZip <= 42788)
            {
                fullName = "Kentucky";
            }
            else if ((numZip >= 70001 && numZip <= 71232) || (numZip >= 71234 && numZip <= 71497))
            {
                fullName = "Louisiana";
            }
            else if ((numZip >= 1001 && numZip <= 2791) || (numZip >= 5501 && numZip <= 5544))
            {
                fullName = "Massachusetts";
            }
            else if (numZip == 20331 || (numZip >= 20335 && numZip <= 20797) || (numZip >= 20812 && numZip <= 21930))
            {
                fullName = "Maryland";
            }
            else if (numZip >= 3901 && numZip <= 4992)
            {
                fullName = "Maine";
            }
            else if (numZip >= 48001 && numZip <= 49971)
            {
                fullName = "Michigan";
            }
            else if (numZip >= 55001 && numZip <= 56763)
            {
                fullName = "Minnesota";
            }
            else if (numZip >= 63001 && numZip <= 65899)
            {
                fullName = "Missouri";
            }
            else if ((numZip >= 38601 && numZip <= 39776) || numZip == 71233)
            {
                fullName = "Mississippi";
            }
            else if (numZip >= 59001 && numZip <= 59937)
            {
                fullName = "Montana";
            }
            else if (numZip >= 27006 && numZip <= 28909)
            {
                fullName = "North Carolina";
            }
            else if (numZip >= 58001 && numZip <= 58856)
            {
                fullName = "North Dakota";
            }
            else if ((numZip >= 68001 && numZip <= 68118) || (numZip >= 68122 && numZip <= 69367))
            {
                fullName = "Nebraska";
            }
            else if (numZip >= 3031 && numZip <= 3897)
            {
                fullName = "New Hampshire";
            }
            else if (numZip >= 7001 && numZip <= 8989)
            {
                fullName = "New Jersey";
            }
            else if (numZip >= 87001 && numZip <= 88441)
            {
                fullName = "New Mexico";
            }
            else if (numZip >= 88901 && numZip <= 89883)
            {
                fullName = "Nevada";
            }
            else if (numZip == 6390 || (numZip >= 10001 && numZip <= 14975))
            {
                fullName = "New York";
            }
            else if (numZip >= 43001 && numZip <= 45999)
            {
                fullName = "Ohio";
            }
            else if ((numZip >= 73001 && numZip <= 73199) || (numZip >= 73401 && numZip <= 74966))
            {
                fullName = "Oklahoma";
            }
            else if (numZip >= 97001 && numZip <= 97920)
            {
                fullName = "Oregon";
            }
            else if (numZip >= 15001 && numZip <= 19640)
            {
                fullName = "Pennsylvania";
            }
            else if (numZip >= 2801 && numZip <= 2940)
            {
                fullName = "Rhode Island";
            }
            else if (numZip >= 29001 && numZip <= 29948)
            {
                fullName = "South Carolina";
            }
            else if (numZip >= 57001 && numZip <= 57799)
            {
                fullName = "South Dakota";
            }
            else if (numZip >= 37010 && numZip <= 38589)
            {
                fullName = "Tennessee";
            }
            else if (numZip == 73301 || (numZip >= 75001 && numZip <= 75501) || (numZip >= 75503 && numZip <= 79999) || (numZip >= 88510 && numZip <= 88589))
            {
                fullName = "Texas";
            }
            else if (numZip >= 84001 && numZip <= 84784)
            {
                fullName = "Utah";
            }
            else if (numZip >= 20040 && numZip <= 24658)
            {
                fullName = "Virginia";
            }
            else if ((numZip >= 5001 && numZip <= 5495) || (numZip >= 5601 && numZip <= 5907))
            {
                fullName = "Vermont";
            }
            else if (numZip >= 98001 && numZip <= 99403)
            {
                fullName = "Washington";
            }
            else if (numZip >= 53001 && numZip <= 54990)
            {
                fullName = "Wisconsin";
            }
            else if (numZip >= 24701 && numZip <= 26886)
            {
                fullName = "West Virginia";
            }
            else if (numZip >= 82001 && numZip <= 83128)
            {
                fullName = "Wyoming";
            }
            return fullName;
        }
    //     }
    // }

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
        int? currentUserId = HttpContext.Session.GetInt32(LOGGED_IN_ID);

        User currentUser = _context.Users.SingleOrDefault(u => u.UserId == (int)currentUserId);

        if (currentUser != null)
        {
            ViewBag.User = currentUser;
        }
        List<User> allUsers = _context.Users.Take(4).ToList();
        List<Business> allBusinesses = _context.Businesses.Include(b => b.Category).OrderByDescending(b => b.UpdatedAt).Take(3).ToList();
        List<Business> businessLocations = _context.Businesses.OrderByDescending(b => b.UpdatedAt).Distinct().Take(4).ToList();

        ViewBag.Businesses = allBusinesses;
        ViewBag.Locations = businessLocations;
        ViewBag.RecentReviews = allUsers;
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
                    return RedirectToAction("Index");
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
        return RedirectToAction("Index");
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

    [HttpGet]
    [Route("/users/{userId}/settings")]
    public IActionResult UserProfile(int UserId)
    {
        User thisUser = _context.Users.Include(u => u.Reviews).SingleOrDefault(u => u.UserId == UserId);
        List<Review> userReviews = _context.Reviews.Include(r => r.Business).Where(r => r.UserId == thisUser.UserId).ToList();

        ViewBag.UserReviews = userReviews;
        ViewBag.User = thisUser;
        return View();
    }

    [HttpGet]
    [Route("/users/{userId}/profile")]
    public IActionResult PublicProfile(int UserId)
    {
        User thisUser = _context.Users.Include(u => u.Reviews).ThenInclude(r => r.Business).SingleOrDefault(u => u.UserId == UserId);

        // hopefully query above can remove necessity for below
        List<Review> userReviews = _context.Reviews.Where(r => r.UserId == thisUser.UserId).ToList();
        string userState = findStateName(thisUser.ZipCode);

        ViewBag.StateName = userState;
        ViewBag.User = thisUser;
        ViewBag.UserReviews = userReviews;
        return View();
    }

    [HttpPost]
    [Route("/users/{userId}/settings/update")]
    public IActionResult UpdateSettings(UserRegViewModel model, int userId)
    {
        User upUser = _context.Users.Include(u => u.Reviews).SingleOrDefault(u => u.UserId == userId);
        if (ModelState.IsValid)
        {
            upUser.FirstName = model.FirstName;
            upUser.LastName = model.LastName;
            upUser.Email = model.Email;
            upUser.Password = model.Password;
            upUser.ZipCode = model.ZipCode;
            upUser.UpdatedAt = DateTime.Now;
            _context.Update(upUser);
            _context.SaveChanges();
            return RedirectToAction("UserProfile");
        }
        return RedirectToAction("UserProfile");
    }

    [HttpGet]
    [Route("/users/{userId}/reviews/{reviewId}/delete")]
    public IActionResult DeleteReview(int userID, int reviewID)
    {
        User thisUser = _context.Users.Include(u => u.Reviews).SingleOrDefault(u => u.UserId == userID);
        Review thisReview = _context.Reviews.SingleOrDefault(r => r.UserId == userID);
        _context.Remove(thisReview);
        _context.SaveChanges();
        return RedirectToAction("UserProfile");
    }
}
}