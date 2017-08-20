using AutoMapper;
using LMS.Core.Domain.Books;
using LMS.Web.Admin.ViewModels;
using System.Linq;

namespace LMS.Web.Admin.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(dst => dst.Authors, opt => opt.MapFrom(x => x.BookAuthors.Select(y => y.Author).ToList()))
                .ForMember(dst => dst.Subjects, opt => opt.MapFrom(x => x.BookSubjects.Select(y => y.Subject).ToList()));

            CreateMap<BookViewModel, Book>();
        }
    }
}
