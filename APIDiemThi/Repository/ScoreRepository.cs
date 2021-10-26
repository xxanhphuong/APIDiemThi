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
    public class ScoreRepository : IScoreRepository
    {
        private readonly ApplicationDbContext _context;

        public ScoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateScore(Score Score)
        {
            _context.Score.Add(Score);
            return Save();
        }

        public bool DeleteScore(Score Score)
        {
            _context.Score.Remove(Score);
            return Save();
        }

        public Score GetScore(int studentId, int subjectId)
        {
            return _context.Score.Include(p => p.Subject).Include(p => p.Student).FirstOrDefault(a => a.StudentId == studentId && a.SubjectId == subjectId);
        }

        public PagedList<Score> GetScores(PageParamers ownerParameters)
        {
            return  PagedList<Score>.ToPagedList(_context.Score.Include(p=> p.Subject).Include(p => p.Student),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);
        }

        public PagedList<Score> GetScoresByStudent(int studentId, PageParamers ownerParameters)
        {
            return PagedList<Score>.ToPagedList(_context.Score.Where(p => p.StudentId == studentId),
                    ownerParameters.PageNumber,
                    ownerParameters.PageSize);



            //var kq = from p in _context.Score
            //         join e in _context.Subject
            //         on p.SubjectId equals e.Id
            //         where p.StudentId == studentId
            //         select new
            //         {
            //             p.StudentId,
            //             p.MidScore,
            //             p.FinalScore,
            //             e.Name
            //         };
            //return kq.ToList();

        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
        public bool ScoreExists(int studentId, int subjectId)
        {
            bool value = _context.Score.Any(a => a.StudentId == studentId && a.SubjectId == subjectId);
            return value;
        }

        public bool UpdateScore(Score Score)
        {
            _context.Score.Update(Score);
            return Save();
        }
    }
}
