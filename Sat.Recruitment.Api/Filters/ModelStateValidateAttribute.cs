using Correlate;
using Demo.Luka.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Sat.Recruitment.Api.Filters
{
    /// <summary>
    /// ModelStateValidateAttribute
    /// </summary>
    public class ModelStateValidateAttribute : ActionFilterAttribute
    {
        private readonly ICorrelationContextAccessor _correlation;

        /// <summary>
        /// ModelStateValidateAttribute
        /// </summary>
        public ModelStateValidateAttribute(ICorrelationContextAccessor correlation)
        {
            _correlation = correlation;
        }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = BadRequestResponse(context);
            }
        }

        private IActionResult BadRequestResponse(ActionExecutingContext context)
        {
           var errors = new List<string>();
            var response = new ErrorDetailModel();
            response.Code = ((int)HttpStatusCode.UnprocessableEntity).ToString();
            foreach (var error in context.ModelState.SelectMany(item => item.Value.Errors))
            {
                response.Errors.Add( error.ErrorMessage );
            }
            return new BadRequestObjectResult(response);

        }
    }
}
