using AutoMapper;
using PhoneBook.Models;
using PhoneBook.ViewModels.AccountVM;
using PhoneBook.ViewModels.ContactVM;
using PhoneBook.ViewModels.GroupVM;
using PhoneBook.ViewModels.PhoneVM;
using PhoneBook.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Mappings
{
    public class ViewModelToModelMappingProfile:Profile
    {
        public ViewModelToModelMappingProfile():base(nameof(ViewModelToModelMappingProfile))
        {
                
        }
        protected override void Configure()
        {
            Mapper.CreateMap<ContactEditVM, Contact>()
                .ForMember(c => c.Phones, opt => opt.Ignore())
                .ForMember(c => c.UserID, opt => opt.Ignore());

            Mapper.CreateMap<GroupEditVM, Group>();

            Mapper.CreateMap<PhoneEditVM, Phone>();

            Mapper.CreateMap<UserEditVM, User>()
                .ForMember(u => u.RememberMeHash, opt => opt.Ignore())
                .ForMember(u => u.DateExpire, opt => opt.Ignore());

            Mapper.CreateMap<AccountEditVM, User>();
        }
    }
}