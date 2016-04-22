using AutoMapper;
using PhoneBook.Models;
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
    public class ModelToViewModelMappingProfile:Profile
    {
        public ModelToViewModelMappingProfile():base(nameof(ModelToViewModelMappingProfile))
        {

        }
        protected override void Configure()
        {
            Mapper.CreateMap<Contact, ContactEditVM>()
                .ForMember(m => m.Countries, opt => opt.Ignore())
                .ForMember(m => m.Cities, opt => opt.Ignore())
                .ForMember(m => m.Groups, opt => opt.Ignore())
                .ForMember(m => m.CountryID, opt => opt.Ignore());

            Mapper.CreateMap<Group, GroupEditVM>();

            Mapper.CreateMap<Phone, PhoneEditVM>();

            Mapper.CreateMap<User, UserEditVM>();
        }
    }
}