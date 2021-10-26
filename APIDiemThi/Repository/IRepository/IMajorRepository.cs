using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Helpers;
using APIDiemThi.Models;

namespace APIDiemThi.Repository.IRepository
{
    public interface IMajorRepository
    {
        PagedList<Major> GetMajors(string kw, PageParamers ownerParameters);
        Major GetMajor(int MajorId);
        bool MajorExists(string name);
        bool MajorExists(int id);
        bool CreateMajor(Major Major);
        bool UpdateMajor(Major Major);
        bool DeleteMajor(Major Major);
        bool Save();
    }
}
