using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Data;
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Repository.IRepository;

namespace APIDiemThi.Repository
{
    public class MajorRepository : IMajorRepository
    {
        private readonly ApplicationDbContext _context;

        public MajorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public PagedList<Major> GetMajors(string kw, PageParamers ownerParameters)
        {
            if (kw != null)
            {
                return PagedList<Major>.ToPagedList(_context.Major.Where(p=>p.Name.Contains(kw)),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
            }
            return PagedList<Major>.ToPagedList(_context.Major,
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
        }

        public Major GetMajor(int MajorId)
        {
            return _context.Major.FirstOrDefault(a => a.Id == MajorId);
        }

        public bool MajorExists(string name)
        {
            bool value = _context.Major.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool MajorExists(int id)
        {
            bool value = _context.Major.Any(a => a.Id == id);
            return value;
        }

        public bool CreateMajor(Major Major)
        {
            _context.Major.Add(Major);
            return Save();
        }

        public bool UpdateMajor(Major Major)
        {
             _context.Major.Update(Major);
            return Save();
        }

        public bool DeleteMajor(Major Major)
        {
            _context.Major.Remove(Major);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
