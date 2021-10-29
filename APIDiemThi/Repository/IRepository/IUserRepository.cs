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
        Task<int> GetIdUser(string username);
        bool UserExists(string username);
        bool UserExists(int userId);
        Task<bool> CreateUser(Users User);
        Task<bool> UpdateUser(Users User);
        Task<bool> DeleteUser(Users User);
        Task<bool> Save();
    }
}
