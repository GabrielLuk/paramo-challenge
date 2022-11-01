using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.ViewModel.UserController.CreateUser.Input;
using Xunit;
using System.Net;
using Application.User;
using System;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersControllerTest
    {
        private readonly UsersController _testee;
        private readonly CreateUserViewModel _userModel;
        private readonly IMediator _mediator;
        public UsersControllerTest()
        {
            _mediator = A.Fake<IMediator>();
            _testee = new UsersController(A.Fake<IMapper>(), _mediator);
            A.CallTo(() => _mediator.Send(A<CreateUserCommand>._, default)).Returns(true);
        }
        [Fact]
        public async void User_SholdReturnOk()
        {
           
            //Arrange
          var  userModel = new CreateUserViewModel {
                Name = "Mike",
                Email = "mike@gmail.com",
                Phone = "+349 1122354215",
                Address = "Av juna G",
                UserType = "Normal",
                Money = 124,
            };

            // Act 
            var result = await _testee.CreateUser(userModel);
            // Assert
            Assert.Equal((result.Result as StatusCodeResult)?.StatusCode, (int)HttpStatusCode.OK);
            Assert.True(result.Value);
          
        }

        [Fact]
        public async void User_SholdBeBadRequest()
        {
            //Arrange
            
            var userModel = new CreateUserViewModel
            {
                Name = "Mike",
                Email = "Agustina@gmail.com",
                Phone = "+349 1122354215",
                Address = "Av juna G",
                UserType = "Normal",
                Money = 124,
            };

            // Act 
            var result = await _testee.CreateUser(userModel);

            // Assert
            Assert.Equal((result.Result as StatusCodeResult)?.StatusCode, (int)HttpStatusCode.BadRequest);
            //TODO Falta Validar la el Mensaje de devolucion
           // Assert.True(result.Value);
            
            
          
        }
    }
}
