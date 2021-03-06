﻿using AutoMapper;
using CustomerFollow.Models;
using Followups.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Followups.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<Customer, CustomerFollowUp>();
            CreateMap<CustomerFollowUp, Customer>();
        }
    }
}
