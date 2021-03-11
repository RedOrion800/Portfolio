using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodTrackingFinalProject.Models
{
    public partial class Userinfo
    {
        public string Id { get; set; }

        [Required]
        public string Fname { get; set; }

        [Required]
        public string Lname { get; set; }

        [Required]
        public decimal Startweight { get; set; }

        public decimal? Currentweight { get; set; }

        [Required]
        public decimal Desiredweight { get; set; }

        [Required]
        public byte Height { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public string Activitylevel { get; set; }
        public DateTime Lastloggedin { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }

        public Userinfo()
        {
            Id = "";
            Fname = "";
            Lname = "";
            Startweight = 0;
            Currentweight = 0;
            Desiredweight = 0;
            Height = 0;
            Sex = "";
            Activitylevel = "";
            Lastloggedin = DateTime.Now;
            Birthday = DateTime.Now;
        }

        public Userinfo(string id, string fname, string lname, decimal start, decimal? current, decimal desired, byte height, string sex, string activity, DateTime birthday)
        {
            Id = id;
            Fname = fname;
            Lname = lname;
            Startweight = start;
            Currentweight = current;
            Desiredweight = desired;
            Height = height;
            Sex = sex;
            Activitylevel = activity;
            Birthday = birthday;
        }

        public Userinfo(string id, string fname, string lname, decimal start, decimal? current, decimal desired, byte height, string sex, string activity, DateTime lastLoggedIn, DateTime birthday)
        {
            Id = id;
            Fname = fname;
            Lname = lname;
            Startweight = start;
            Currentweight = current;
            Desiredweight = desired;
            Height = height;
            Sex = sex;
            Activitylevel = activity;
            Lastloggedin = lastLoggedIn;
            Birthday = birthday;
        }
    }
}
