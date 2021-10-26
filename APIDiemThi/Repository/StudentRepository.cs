using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Data;
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIDiemThi.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateStudent(Student Student)
        {
            _context.Student.Add(Student);
            return Save();
        }

        public bool DeleteStudent(Student Student)
        {
            _context.Student.Remove(Student);
            return Save();
        }

        public Student GetStudent(int StudentId)
        {
            return _context.Student.Include(t => t.User).FirstOrDefault(a => a.StudentId == StudentId);
        }

        public PagedList<Student> GetStudents(string kw, PageParamers ownerParameters)
        {
            if (kw != null)
            {
                return PagedList<Student>.ToPagedList(_context.Student.Include(s => s.User).Where(p => p.User.FullName.Contains(kw)),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
            }
            return PagedList<Student>.ToPagedList(_context.Student.Include(s => s.User),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool StudentExists(int id)
        {
            bool value = _context.Student.Any(a => a.StudentId == id);
            return value;
        }

        public bool UpdateStudent(Student Student)
        {
            _context.Student.Update(Student);
            return Save();
        }
    }
}
