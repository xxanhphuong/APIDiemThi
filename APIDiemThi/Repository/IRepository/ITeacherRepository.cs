using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Helpers;
using APIDiemThi.Models;

namespace APIDiemThi.Repository.IRepository
{
    public interface ITeacherRepository
    {
        PagedList<Teacher> GetTeachers(string kw, PageParamers ownerParameters);
        Teacher GetTeacher(int TeacherId);
        bool TeacherExists(int id);
        Task<bool> CreateTeacher(Teacher Teacher);
        Task<bool> UpdateTeacher(Teacher Teacher);
        Task<bool> DeleteTeacher(Teacher Teacher);
        Task<bool> Save();
    }
}
