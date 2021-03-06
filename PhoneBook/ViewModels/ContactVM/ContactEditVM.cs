﻿using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.ViewModels.ContactVM
{
    public class ContactEditVM
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(20)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Use letters only please")]
        public string Adress { get; set; }
        public List<Phone> Phones { get; set; }

        [Display(Name = "City")]
        public int CityID { get; set; }
        [Display(Name = "Country")]
        public int CountryID { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }

        public string ImagePath { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }
        public string[] SelectedGroups { get; set; }
    }
}