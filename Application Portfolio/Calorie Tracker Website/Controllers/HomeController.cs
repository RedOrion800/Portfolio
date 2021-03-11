using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodTrackingFinalProject.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FoodTrackingFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PremiereContext _context;

        public HomeController(PremiereContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                Userinfo theUser = new Userinfo();
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);//gets userID, don't know a better way.  If you want to find a better way, you can replace this.
                
                try
                {
                    var users = from u in _context.Userinfo
                                select u;
                    users = users.Where(u => u.Id.Contains(userID));

                    if (users.Count() == 0)
                    {
                        return RedirectToAction("Create", "Userinfoes");
                    }

                    foreach (var user in users)
                    {
                        theUser = user;
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }

                double BMR = 0;
                int calories = 0;
                int caloriesEaten = 0;

                int age = 0;
                age = DateTime.Now.Year - theUser.Birthday.Year;
                if (DateTime.Now.DayOfYear < theUser.Birthday.DayOfYear)
                {
                    age = age - 1;
                }

                if (theUser.Sex == "Male")
                {
                    BMR = 66 + (6.3 * (double)theUser.Currentweight) + (12.9 * theUser.Height) - (6.8 * age);
                }
                else if(theUser.Sex == "Female")
                {
                    BMR = 655 + (4.3 * (double)theUser.Currentweight) + (4.7 * theUser.Height) - (4.7 * age);
                }

                switch (theUser.Activitylevel)
                {
                    case "Sedentary":
                        calories = (int)(BMR * 1.2);
                        break;
                    case "Lightly Active":
                        calories = (int)(BMR * 1.375);
                        break;
                    case "Moderately Active":
                        calories = (int)(BMR * 1.55);
                        break;
                    case "Very Active":
                        calories = (int)(BMR * 1.725);
                        break;
                    case "Extra Active":
                        calories = (int)(BMR * 1.9);
                        break;
                    default:
                        calories = -10;//THIS SHOULD NEVER SET OFF.
                        break;
                }

                DateTime today = DateTime.Now;

                int month = today.Month;
                int day = today.Day;
                int year = today.Year;

                var todaysFood = from f in _context.Foodinfo
                                 select f;

                todaysFood = todaysFood.Where(f => f.UserId.Equals(userID));
                todaysFood = todaysFood.Where(f => f.DateEntered.Month == month && f.DateEntered.Day == day && f.DateEntered.Year == year);

                foreach (var food in todaysFood)
                {
                    caloriesEaten += (int)(food.Calories);
                    calories -= (int)(food.Calories);
                }

                ViewBag.CaloriesAvailable = calories;
                ViewBag.CaloriesEaten = caloriesEaten;

                double weightLost = (double)(theUser.Startweight - theUser.Currentweight);
                if(weightLost < 0)
                {
                    weightLost = 0;
                }

                double weightLeftToGo = (double)(theUser.Currentweight - theUser.Desiredweight);
                if(weightLeftToGo < 0)
                {
                    weightLeftToGo = 0;
                }

                ViewBag.WeightLost = weightLost;
                ViewBag.WeightLeftToGo = weightLeftToGo;

                return View();
            }

            ViewBag.CaloriesAvailable = -1;
            ViewBag.WeightLost = -1;
            ViewBag.WeightLeftToGo = -1;

            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
