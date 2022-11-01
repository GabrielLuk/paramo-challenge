using Application.User;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.ViewModel.UserController.CreateUser.Input;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UsersController(IMapper mapper
            , IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        /// <summary>
        ///     Action to create a new user.
        /// </summary>
        /// <param name="CreateUserViewModel">Model to create a new order</param>
        /// <returns>Returns the created order</returns>
        /// <response code="200">Returned if the user was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or saved or validation failed</response>
        /// <response code="422">Returned when the validation failed</response>
        /// <response code="500">Returned for another reason</response>
        [HttpPost]
        [Route("/create-user")]
        public async Task<ActionResult<bool>> CreateUser([FromBody] CreateUserViewModel user)
        {
            var result = await _mediator.Send(new CreateUserCommand
            {
                User = _mapper.Map<User>(user)
            });
            return Ok(result);
        }
    }
   
}
