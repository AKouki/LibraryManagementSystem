using AutoMapper;
using LMS.Core.Domain.Books;
using LMS.Web.Admin.ViewModels;
using System.Linq;

namespace LMS.Web.Admin.Mappings
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorViewModel>()
                .ForMember(dst => dst.Books, opt => opt.MapFrom(x => x.BookAuthors.Select(y => y.Book).ToList()));

            CreateMap<AuthorViewModel, Author>();
        }
    }
}
