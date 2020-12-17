﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ApiModels
{
    public class LoginApiModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public User Convert() => new User
        {
            Username = UserName,
            Password = Password
        };
    }
}
