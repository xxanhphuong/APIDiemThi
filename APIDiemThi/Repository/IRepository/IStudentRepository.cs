using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Helpers;
using APIDiemThi.Models;

namespace APIDiemThi.Repository.IRepository
{
    public interface IStudentRepository
    {
        PagedList<Student> GetStudents(string kw, PageParamers ownerParameters);
        Student GetStudent(int StudentId);
        bool StudentExists(int id);
        Task<bool> CreateStudent(Student Student);
        Task<bool> UpdateStudent(Student Student);
        Task<bool> DeleteStudent(Student Student);
        Task<bool> Save();
    }
}
