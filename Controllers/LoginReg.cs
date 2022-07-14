using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
// Change based on project name
using SpotifyPlus.Models;


// Change based on project name
namespace LoginRegistration.Controllers;

public class LoginController : Controller
{
    private MyContext _context;

    public LoginController(MyContext context)
    {
        _context = context;
    }
    [HttpGet("/")]
    public IActionResult Index()
    {
        HttpContext.Session.Clear();
        return View();
    }
    // [HttpGet("/profile")]
    // public ViewResult profilePage()
    // {
    //     return View("profile");
    // }
    
    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        // Check initial ModelState
        if(ModelState.IsValid)
        {
            if(_context.User.Any(u => u.Email == user.Email))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("Email", "Email already in use!");
                Console.WriteLine("Email was invalid");
                // You may consider returning to the View at this point
                return View("Index");
            }
            // If a User exists with provided email
            if(_context.User.Any(u => u.Username == user.Username))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("Username", "Username already in use!");
                Console.WriteLine("Username was invalid");
                // You may consider returning to the View at this point
                return View("Index");
            }
            
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            _context.Add(user);
            _context.SaveChanges();
            User? loggedin = _context.User.FirstOrDefault(l => l.Email == user.Email);
            HttpContext.Session.SetInt32("ID", loggedin.UserId);
            int? LocalId = HttpContext.Session.GetInt32("ID");
            return Redirect("dashboard");
        }
        else
        {
            ModelState.AddModelError("Password", "Passwords must match");
            Console.WriteLine("ModelState was invalid for whatever reason");
            return View("Index");
        }
        
    } 
    [HttpPost("login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if(ModelState.IsValid)
        {
            // If initial ModelState is valid, query for a user with provided email
            var userInDb = _context.User.FirstOrDefault(u => u.Username == userSubmission.LoginUsername);
            // If no user exists with provided email
            if(userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("LoginUsername", "Invalid Username/Password");
                return View("Index");
            }
                
            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();
                
            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                
            // result can be compared to 0 for failure
            if(result == 0)
            {
                ModelState.AddModelError("LoginPassword", "Invalid Username/Password");
                // You may consider returning to the View at this point
                return View("Index");
            }
            else if(result != 0)
            {
                HttpContext.Session.SetInt32("ID", userInDb.UserId);
                return Redirect("dashboard");
            }
        }
        return View("index");
    }
}