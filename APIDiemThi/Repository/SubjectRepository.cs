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
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;
        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateSubject(Subject Subject)
        {
            _context.Subject.Add(Subject);
            return Save();
        }

        public bool DeleteSubject(Subject Subject)
        {
            _context.Subject.Remove(Subject);
            return Save();
        }

        public Subject GetSubject(int SubjectId)
        {
            return _context.Subject.FirstOrDefault(a => a.Id == SubjectId);
        }

        public PagedList<Subject> GetSubjects(string kw, PageParamers ownerParameters)
        {
            if (kw != null)
            {
                return PagedList<Subject>.ToPagedList(_context.Subject.Where(p => p.Name.Contains(kw)),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
            }
            return PagedList<Subject>.ToPagedList(_context.Subject,
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool SubjectExists(string name)
        {
            bool value = _context.Subject.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool SubjectExists(int id)
        {
            bool value = _context.Subject.Any(a => a.Id == id);
            return value;
        }

        public bool UpdateSubject(Subject Subject)
        {
            _context.Subject.Update(Subject);
            return Save();
        }
    }
}
