using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Web.Models.GroupModel;

namespace DataImporter.Web.Profiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<CreateGroupModel, Group>().ReverseMap();
            CreateMap<GroupEditModel, Group>().ReverseMap();
        }
    }
}