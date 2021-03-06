﻿using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using remotevotersapi.Utils.Results;

namespace remotevotersapi.Utils
{
    /// <summary>
    /// Attribute responsible for the view model validation
    /// 
    /// Author: FStrony
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (context.ModelState.IsValid) return;

            var errors = modelState.Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
                .ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).First())
                .Select(x => new ErrorValidate { Property = x.Key, Message = x.Value })
                .ToList();
            var response = context.HttpContext.Response;
            response.StatusCode = 400;
            context.Result = new ErrorJsonResult(errors);
        }
    }
}
