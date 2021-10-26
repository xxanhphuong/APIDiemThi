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
        bool CreateStudent(Student Student);
        bool UpdateStudent(Student Student);
        bool DeleteStudent(Student Student);
        bool Save();
    }
}
