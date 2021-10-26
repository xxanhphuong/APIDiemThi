using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Helpers;
using APIDiemThi.Models;

namespace APIDiemThi.Repository.IRepository
{
    public interface IScoreRepository
    {
        PagedList<Score> GetScores(PageParamers ownerParameters);
        PagedList<Score> GetScoresByStudent(int studentId, PageParamers ownerParameters);
        Score GetScore(int studentId, int subjectId);
        bool ScoreExists(int studentId, int subjectId);
        bool CreateScore(Score Score);
        bool UpdateScore(Score Score);
        bool DeleteScore(Score Score);
        bool Save();
    }
}
