using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Helpers;
using APIDiemThi.Models;

namespace APIDiemThi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Users Authenticate(string username, string password);
        Users Register(string username, string password);

        PagedList<Users> GetUsers(string kw, PageParamers ownerParameters);
        Users GetUser(int UserId);
        bool UserExists(string username);
        bool UserExists(int userId);
        bool CreateUser(Users User);
        bool UpdateUser(Users User);
        bool DeleteUser(Users User);
        bool Save();
    }
}
