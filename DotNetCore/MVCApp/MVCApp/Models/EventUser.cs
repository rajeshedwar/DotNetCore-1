using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCApp.Models
{
    public class EventUser : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public override int Id { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [Display(Name = "Fisrt Name")]
        [Required(ErrorMessage = "FisrtName is required!.")]
        public string FisrtName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "LastName isrequired!.")]
        public string LastName { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company is required!.")]
        public string Company { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number required!.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string ContactNo { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is required!.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

    }
}
