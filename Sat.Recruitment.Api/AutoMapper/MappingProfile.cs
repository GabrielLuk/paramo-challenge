using AutoMapper;
using Domain;
using Sat.Recruitment.Api.ViewModel.UserController.CreateUser.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserViewModel, User>();
        }
    }
}
