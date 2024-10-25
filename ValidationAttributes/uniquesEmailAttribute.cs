using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using crickinfo_mvc_ef_core.Models;
using Microsoft.EntityFrameworkCore;

namespace crickinfo_mvc_ef_core.ValidationAttributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var context = (CrickInfoContext)validationContext.GetService(typeof(CrickInfoContext));

            if (context == null)
            {
                throw new InvalidOperationException("ApplicationDbContext is not registered.");
            }

            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success;
            }

            var emailExists = context.Users.Any(u => u.Email.Equals(email));

            if (emailExists)
            {
                return new ValidationResult("Email address is already used");
            }

            return ValidationResult.Success;
        }
    }
}
