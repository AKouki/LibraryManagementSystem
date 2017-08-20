using AutoMapper;
using LMS.Core.Domain.Books;
using LMS.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Web.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(dst => dst.Authors, opt => opt.MapFrom(x => x.BookAuthors.Select(y => y.Author).ToList()))
                .ForMember(dst => dst.Subjects, opt => opt.MapFrom(x => x.BookSubjects.Select(y => y.Subject).ToList()));
        }
    }
}
