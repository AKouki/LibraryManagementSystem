using AutoMapper;
using LMS.Core.Domain.Issues;
using LMS.Web.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Web.Admin.Mappings
{
    public class IssueReturnProfile : Profile
    {
        public IssueReturnProfile()
        {
            CreateMap<Issue, IssueReturnViewModel>()
                .ForMember(dst => dst.MemberId, opt => opt.MapFrom(x => x.MemberId))
                .ForMember(dst => dst.BookId, opt => opt.MapFrom(x => x.BookId))
                .ForMember(dst => dst.BookTitle, opt => opt.MapFrom(x => x.Book.Title))
                .ForMember(dst => dst.MemberName,
                            opt => opt.MapFrom(x => string.Format("{0} {1}", x.Member.FirstName, x.Member.LastName)))
                .ForMember(dst => dst.MemberPhone, opt => opt.MapFrom(x => x.Member.PhoneNumber))
                .ForMember(dst => dst.IssueDate, opt => opt.MapFrom(x => x.IssueDate))
                .ForMember(dst => dst.ExpireDate, opt => opt.MapFrom(x => x.ExpireDate));
        }
    }
}
