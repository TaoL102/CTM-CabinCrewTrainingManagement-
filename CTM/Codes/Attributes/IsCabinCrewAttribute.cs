using System.ComponentModel.DataAnnotations;
using CTM.Areas.API.Controllers;

namespace CTM.Codes.Attributes
{
    public class IsCabinCrewAttribute:ValidationAttribute
    {



            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                    return new ValidationResult("Not a valid value");

                if (new ValidateController().IsValidCabinCrew(value.ToString()))
                {
                return ValidationResult.Success;
            }

            return new ValidationResult("Name is not valid");
            }


    }
    }
