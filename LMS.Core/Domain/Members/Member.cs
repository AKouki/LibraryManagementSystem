using LMS.Core.Domain.Issues;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Domain.Members
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public MemberType MemberType { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}