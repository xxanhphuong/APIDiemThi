﻿using System;
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
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;
        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateTeacher(Teacher Teacher)
        {
            _context.Teacher.Add(Teacher);
            return Save();
        }

        public bool DeleteTeacher(Teacher Teacher)
        {
            _context.Teacher.Remove(Teacher);
            return Save();
        }

        public Teacher GetTeacher(int TeacherId)
        {
            return _context.Teacher.Include(t => t.User).FirstOrDefault(a => a.TeacherId == TeacherId);
        }

        public PagedList<Teacher> GetTeachers(string kw, PageParamers ownerParameters)
        {
            if (kw != null)
            {
                return PagedList<Teacher>.ToPagedList(_context.Teacher.Include(s => s.User).Where(p => p.User.FullName.Contains(kw)),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
            }
            return PagedList<Teacher>.ToPagedList(_context.Teacher.Include(s => s.User),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool TeacherExists(int id)
        {
            bool value = _context.Teacher.Any(a => a.TeacherId == id);
            return value;
        }

        public bool UpdateTeacher(Teacher Teacher)
        {
            _context.Teacher.Update(Teacher);
            return Save();
        }
    }
}
