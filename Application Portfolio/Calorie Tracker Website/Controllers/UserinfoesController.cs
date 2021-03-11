using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodTrackingFinalProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodTrackingFinalProject.Controllers
{
    public class UserinfoesController : Controller
    {
        private readonly PremiereContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserinfoesController(PremiereContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Userinfoes
        //public async Task<IActionResult> Index()
        //{
        //    var premiereContext = _context.Userinfo.Include(u => u.IdNavigation);
        //    return View(await premiereContext.ToListAsync());
        //}

        // GET: Userinfoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userinfo = await _context.Userinfo
                .Include(u => u.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userinfo == null)
            {
                return NotFound();
            }

            return View(userinfo);
        }

        public async Task<IActionResult> Profile()
        {
            Userinfo theUser = new Userinfo();

            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);//gets userID, don't know a better way.  If you want to find a better way, you can replace this.
                var users = from u in _context.Userinfo
                            select u;
                users = users.Where(u => u.Id.Contains(userID));

                if (users.Count() == 0)
                {
                    return RedirectToAction(nameof(Create));
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

            theUser.Lastloggedin = DateTime.Now;

            try
            {
                _context.Update(theUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserinfoExists(theUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            int age = 0;
            age = DateTime.Now.Year - theUser.Birthday.Year;
            if (DateTime.Now.DayOfYear < theUser.Birthday.DayOfYear)
            {
                age = age - 1;
            }

            ViewBag.Age = age;
            ViewBag.Birthday = theUser.Birthday.ToString("MM/dd/yyyy");

            return View(theUser);


        }

        // GET: Userinfoes/Create
        public IActionResult Create()
        {
            AspNetUsers theUser = new AspNetUsers();
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);//gets userID, don't know a better way.  If you want to find a better way, you can replace this.
                var users = from u in _context.AspNetUsers
                            select u;
                users = users.Where(u => u.Id.Contains(userID));
                foreach (var user in users)
                {
                    theUser = user;
                }

                ViewBag.userID = userID;
                ViewBag.email = theUser.Email;
            }
            catch(Exception ex)
            {
                ViewBag.userID = "0";
                ViewBag.email = "Test@test.com";
            }

            //ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id"); //replacing a list of IDs with one userID
            return View();
        }

        // POST: Userinfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fname,Lname,Startweight,Currentweight,Desiredweight,Feet,Inches,Sex,Activitylevel,Lastloggedin,Birthday")] UserInfoInput userinfoinput)
        {
            byte height = (byte)((userinfoinput.Feet * 12) + userinfoinput.Inches);
            Userinfo userinfo = new Userinfo(userinfoinput.Id, userinfoinput.Fname, userinfoinput.Lname, userinfoinput.Startweight, userinfoinput.Startweight, userinfoinput.Desiredweight, height, userinfoinput.Sex, userinfoinput.Activitylevel, userinfoinput.Birthday);
            userinfo.Lastloggedin = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(userinfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile));
            }
            ViewBag.userID = userinfoinput.Id;
            return View(userinfoinput);
            /*Replacing below code*/
            /*
            if (ModelState.IsValid)
            {
                _context.Add(userinfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", userinfo.Id);
            return View(userinfo);
            */
        }

        // GET: Userinfoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            UserInfoInput theUser;
            if (id == null)
            {
                return NotFound();
            }

            var userinfo = await _context.Userinfo.FindAsync(id);
            if (userinfo == null)
            {
                return NotFound();
            }

            byte feet = (byte)(userinfo.Height / 12);
            byte inches = (byte)(userinfo.Height % 12);

            theUser = new UserInfoInput(userinfo.Id, userinfo.Fname, userinfo.Lname, userinfo.Startweight, userinfo.Currentweight, userinfo.Desiredweight, feet, inches, userinfo.Sex, userinfo.Activitylevel, userinfo.Birthday);

            //ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", userinfo.Id);
            return View(theUser);
        }

        // POST: Userinfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Fname,Lname,Startweight,Currentweight,Desiredweight,Feet,Inches,Sex,Activitylevel,Lastloggedin,Birthday")] UserInfoInput userinfo)
        {
            if (id != userinfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(!userinfo.Currentweight.HasValue)
                {
                    userinfo.Currentweight = userinfo.Startweight;
                }
                byte height = (byte)((userinfo.Feet * 12) + userinfo.Inches);
                Userinfo theUser = new Userinfo(userinfo.Id, userinfo.Fname, userinfo.Lname, userinfo.Startweight, userinfo.Currentweight, userinfo.Desiredweight, height, userinfo.Sex, userinfo.Activitylevel, DateTime.Now, userinfo.Birthday);
                try
                {
                    _context.Update(theUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserinfoExists(userinfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Profile));
            }
            //ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", userinfo.Id);
            return View(userinfo);
        }

        // GET: Userinfoes/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userinfo = await _context.Userinfo
        //        .Include(u => u.IdNavigation)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userinfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userinfo);
        //}

        // POST: Userinfoes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var userinfo = await _context.Userinfo.FindAsync(id);
        //    _context.Userinfo.Remove(userinfo);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool UserinfoExists(string id)
        {
            return _context.Userinfo.Any(e => e.Id == id);
        }
    }
}
