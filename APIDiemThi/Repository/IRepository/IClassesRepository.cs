using APIDiemThi.Helpers;
using APIDiemThi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Repository.IRepository
{
    public interface IClassesRepository
    {
        PagedList<Classes> GetClassess(string kw, PageParamers ownerParameters);
        Classes GetClasses(int ClassesId);
        bool ClassesExists(string name);
        bool ClassesExists(int id);
        bool CreateClasses(Classes Classes);
        bool UpdateClasses(Classes Classes);
        bool DeleteClasses(Classes Classes);
        bool Save();
    }
}
