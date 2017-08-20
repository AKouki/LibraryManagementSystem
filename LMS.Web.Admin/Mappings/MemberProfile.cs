using AutoMapper;
using LMS.Core.Domain.Members;
using LMS.Web.Admin.ViewModels;

namespace LMS.Web.Admin.Mappings
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberViewModel>();
            CreateMap<MemberViewModel, Member>();
        }
    }
}
