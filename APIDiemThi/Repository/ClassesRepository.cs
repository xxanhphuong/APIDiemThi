using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIDiemThi.Data;
using APIDiemThi.Models;
using APIDiemThi.Repository.IRepository;
using APIDiemThi.Helpers;

namespace APIDiemThi.Repository
{
    public class ClassesRepository : IClassesRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public PagedList<Classes> GetClassess(string kw, PageParamers ownerParameters)
        {
            if (kw != null)
            {
                return PagedList<Classes>.ToPagedList(_context.Classes.Include(c => c.Major).Where(c => c.Name.Contains(kw)),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize); 
            }
            return PagedList<Classes>.ToPagedList(_context.Classes.Include(c => c.Major),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
        }

        public Classes GetClasses(int ClassesId)
        {
            return _context.Classes.Include(c => c.Major).FirstOrDefault(a => a.Id == ClassesId);
        }

        public bool ClassesExists(string name)
        {
            bool value =  _context.Classes.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ClassesExists(int id)
        {
            bool value = _context.Classes.Any(a => a.Id == id);
            return value;
        }

        public bool CreateClasses(Classes Classes)
        {
            _context.Classes.Add(Classes);
            return Save();
        }

        public bool UpdateClasses(Classes Classes)
        {
            _context.Classes.Update(Classes);
            return Save();
        }

        public bool DeleteClasses(Classes Classes)
        {
            _context.Classes.Remove(Classes);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
