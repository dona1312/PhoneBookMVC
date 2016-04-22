using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Mappings
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(map =>
            {
                map.AddProfile<ModelToViewModelMappingProfile>();
                map.AddProfile<ViewModelToModelMappingProfile>();
            });
        }
    }
}