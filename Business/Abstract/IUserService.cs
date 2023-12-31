﻿using Core.Entities.Concrete;
using Core.Ultities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<User>> GetAllUsers();
        IResult AddUser(User user);
        IResult UpdateUser(User user);
        IResult DeleteUser(User user);
        IDataResult<User> GetUserById(int userId);
        
        List<OperationClaim> GetClaims(User user);
        User GetByEmail(string email);


    }
}
