using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTrackingFinalProject.Models
{
    public class UserInfoInput
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
        public byte Feet { get; set; }

        [Required]
        public byte Inches { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public string Activitylevel { get; set; }
        public DateTime Lastloggedin { get; set; }

        [Required]
        public DateTime Birthday { get; set; }


        public UserInfoInput()
        {
            Id = "";
            Fname = "";
            Lname = "";
            Startweight = 0;
            Currentweight = 0;
            Desiredweight = 0;
            Feet = 0;
            Inches = 0;
            Sex = "";
            Activitylevel = "";
            Lastloggedin = DateTime.Now;
            Birthday = DateTime.Now;
        }

        public UserInfoInput(string id, string fname, string lname, decimal start, decimal? current, decimal desired, byte feet, byte inches, string sex, string activity, DateTime birthday)
        {
            Id = id;
            Fname = fname;
            Lname = lname;
            Startweight = start;
            Currentweight = current;
            Desiredweight = desired;
            Feet = feet;
            Inches = inches;
            Sex = sex;
            Activitylevel = activity;
            Birthday = birthday;
        }
    }
}
