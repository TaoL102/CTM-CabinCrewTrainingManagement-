using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CTM.Areas.API.Controllers;
using CTMLib.Resources;

namespace CTM.Codes.Attributes
{
    public class IsCabinCrewAttribute:ValidationAttribute,IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {

                if (new ValidateController().IsValidCabinCrew(value?.ToString()))
                {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
            }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ErrorMessage = ConstViews.ERROR_CabinCrewNotFound,
                ValidationType = "iscabincrew"
            };
            // This is the name the jQuery adapter will use

            yield return rule;
        }
    }
    }
