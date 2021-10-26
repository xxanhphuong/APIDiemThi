using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.ClassesDto;
using APIDiemThi.Models.Dtos.MajorDto;
using APIDiemThi.Models.Dtos.ScoreDto;
using APIDiemThi.Models.Dtos.StudentDto;
using APIDiemThi.Models.Dtos.SubjectDto;
using APIDiemThi.Models.Dtos.TeacherDto;
using APIDiemThi.Models.Dtos.UserDto;
using AutoMapper;

namespace APIDiemThi.ParkyMapper
{
    public class ParkyMapping : Profile
    {
        public ParkyMapping()
        {
            CreateMap<Classes, ClassesCreateDto>().ReverseMap();
            CreateMap<Classes, ClassesUpdateDto>().ReverseMap();
            CreateMap<Classes, ClassesViewDto>().ReverseMap();

            CreateMap<Major, MajorCreateDto>().ReverseMap();
            CreateMap<Major, MajorUpdateDto>().ReverseMap();
            CreateMap<Major, MajorViewDto>().ReverseMap();

            CreateMap<Score, ScoreCreateDto>().ReverseMap();
            CreateMap<Score, ScoreUpdateDto>().ReverseMap();
            CreateMap<Score, ScoreViewDto>().ReverseMap();

            CreateMap<Student, StudentCreateDto>().ReverseMap();
            CreateMap<Student, StudentUpdateDto>().ReverseMap();
            CreateMap<Student, StudentViewDto>().ReverseMap();

            CreateMap<Subject, SubjectCreateDto>().ReverseMap();
            CreateMap<Subject, SubjectUpdateDto>().ReverseMap();
            CreateMap<Subject, SubjectViewDto>().ReverseMap();

            CreateMap<Teacher, TeacherCreateDto>().ReverseMap();
            CreateMap<Teacher, TeacherUpdateDto>().ReverseMap();
            CreateMap<Teacher, TeacherViewDto>().ReverseMap();

            CreateMap<Users, UserCreateDto>().ReverseMap();
            CreateMap<Users, UserUpdateDto>().ReverseMap();
            CreateMap<Users, UserViewDto>().ReverseMap();
            CreateMap<Users, UserLoginDto>().ReverseMap();

        }
    }
}
