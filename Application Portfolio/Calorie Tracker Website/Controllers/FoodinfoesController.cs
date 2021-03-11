using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodTrackingFinalProject.Models;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodTrackingFinalProject.Controllers
{
    public class FoodinfoesController : Controller
    {
        private readonly PremiereContext _context;

        public FoodinfoesController(PremiereContext context)
        {
            _context = context;
        }

        // GET: Foodinfoes
        public async Task<IActionResult> Index()
        {
            var premiereContext = _context.Foodinfo.Include(f => f.User);

            DateTime today = DateTime.Now;

            int month = today.Month;
            int day = today.Day;
            int year = today.Year;

            var foods = from f in _context.Foodinfo
                        select f;

            foods = foods.Where(f => f.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            foods = foods.Where(f => f.DateEntered.Month == month && f.DateEntered.Day == day && f.DateEntered.Year == year);

            if(foods.Count() == 0)
            {
                ViewBag.Empty = true;
            }
            else
            {
                ViewBag.Empty = false;
            }

            return View(await _context.Foodinfo.Where(f => f.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).Where(f => f.DateEntered.Month == month && f.DateEntered.Day == day && f.DateEntered.Year == year).ToListAsync());
        }

        // GET: Foodinfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodinfo = await _context.Foodinfo
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodinfo == null)
            {
                return NotFound();
            }

            return View(foodinfo);
        }

        // GET: Foodinfoes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Foodinfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Calories,DateEntered,UserId")] Foodinfo foodinfo)
        {
            //makes sure its not null
            if(foodinfo.Name != null)
            {
                //get calories...

                //change spaces to %20
                if (foodinfo.Name.Replace(" ", "%20").Length != 0)
                {
                    foodinfo.Name = foodinfo.Name.Replace(" ", "%20");
                    //call api
                    string url = "https://api.nal.usda.gov/fdc/v1/foods/search?api_key=4yzPQJ6HCWvpCWm6KGaZr1QdIl3UkLhzdXs7os8o&query=" + foodinfo.Name;
                    //change spaces back :P
                    foodinfo.Name = foodinfo.Name.Replace("%20", " ");
                    HttpClient client = new HttpClient();
                    string response = await client.GetStringAsync(url);
                    //use json parser
                    var myJObject = JObject.Parse(response);
                    //check if there was at least one hit
                    int hits = (int)myJObject["totalHits"];
                    //if theres at least one valid hit then proceed
                    if (hits > 0)
                    {
                        //stores all nutrient info for the first food
                        IList<JToken> results = myJObject["foods"].ElementAt<JToken>(0)["foodNutrients"].ToList();
                        IList<FoodNutrients> foodNutrients = new List<FoodNutrients>();
                        //loop through all Jtokens and map them to a class
                        foreach (JToken result in results)
                        {
                            FoodNutrients foodNutrient = result.ToObject<FoodNutrients>();
                            foodNutrients.Add(foodNutrient);
                        }
                        //loop through list of nutrients to get energy
                        foreach (FoodNutrients nutrient in foodNutrients)
                        {
                            if (nutrient.nutrientName.Equals("Energy"))
                            {
                                //finally set calories to the energy value... YAY!
                                foodinfo.Calories = (int)nutrient.value;
                            }
                        }
                        //set date
                        foodinfo.DateEntered = DateTime.Now;

                        //set user id
                        string userID = User.FindFirstValue(ClaimTypes.NameIdentifier); // I tottally just stole grey's way, cause I don't know a better way....
                        foodinfo.UserId = userID;

                        if (ModelState.IsValid)
                        {
                            _context.Add(foodinfo);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", foodinfo.UserId);
                    }
                }
            }
            return View(foodinfo);
        }

        // GET: Foodinfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodinfo = await _context.Foodinfo.FindAsync(id);
            if (foodinfo == null)
            {
                return NotFound();
            }

            ViewBag.NameError = "Nothing";
            ViewBag.CalorieError = "Nothing";
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", foodinfo.UserId);
            return View(foodinfo);
        }

        // POST: Foodinfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Calories,DateEntered,UserId")] Foodinfo foodinfo)
        {
            if (id != foodinfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool emptyInput = false;
                ViewBag.NameError = "Nothing";
                ViewBag.CalorieError = "Nothing";

                if(foodinfo.Name == null || foodinfo.Name.Trim().Length == 0)
                {
                    emptyInput = true;
                    ViewBag.NameError = "Please enter an food.";
                }
                if(foodinfo.Calories == null || foodinfo.Calories <= 0)
                {
                    emptyInput = true;
                    ViewBag.CalorieError = "Please add calories.";
                }

                if (emptyInput)
                {
                    ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", foodinfo.UserId);
                    return View(foodinfo);
                }

                try
                {
                    _context.Update(foodinfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodinfoExists(foodinfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", foodinfo.UserId);
            return View(foodinfo);
        }

        // GET: Foodinfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodinfo = await _context.Foodinfo
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodinfo == null)
            {
                return NotFound();
            }

            return View(foodinfo);
        }

        // POST: Foodinfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodinfo = await _context.Foodinfo.FindAsync(id);
            _context.Foodinfo.Remove(foodinfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodinfoExists(int id)
        {
            return _context.Foodinfo.Any(e => e.Id == id);
        }
    }
}
