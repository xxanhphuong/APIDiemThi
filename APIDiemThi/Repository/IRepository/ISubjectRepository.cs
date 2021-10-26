using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Helpers;
using APIDiemThi.Models;

namespace APIDiemThi.Repository.IRepository
{
    public interface ISubjectRepository
    {
        PagedList<Subject> GetSubjects(string kw, PageParamers ownerParameters);
        Subject GetSubject(int SubjectId);
        bool SubjectExists(string name);
        bool SubjectExists(int id);
        bool CreateSubject(Subject Subject);
        bool UpdateSubject(Subject Subject);
        bool DeleteSubject(Subject Subject);
        bool Save();
    }
}
