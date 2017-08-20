using AutoMapper;
using LMS.Core.Domain.Books;
using LMS.Web.ViewModels;
using System.Linq;

namespace LMS.Web.Mappings
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectViewModel>()
                .ForMember(dst => dst.Books, opt => opt.MapFrom(x => x.BookSubjects.Select(y => y.Book).ToList()));
        }
    }
}
