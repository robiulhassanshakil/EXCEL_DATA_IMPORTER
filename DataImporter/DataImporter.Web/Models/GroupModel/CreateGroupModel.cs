using System;
using System.ComponentModel.DataAnnotations;
using Autofac;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.Services;

namespace DataImporter.Web.Models.GroupModel
{
    public class CreateGroupModel
    {
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 Characters")]
        public string Name { get; set; }

        private ILifetimeScope _scope;

        private  IGroupService _groupService;
        private  IMapper _mapper;
        public CreateGroupModel()
        {

        }
        public CreateGroupModel(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        public void CreateGroup(Guid applicationUserId)
        {
           
            var group = new Group()
            {
                Name = Name,
                ApplicationUserId = applicationUserId
            };
            _groupService.CreateGroup(group);
        }


        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _groupService = _scope.Resolve<IGroupService>();
            _mapper = _scope.Resolve<IMapper>();
        }
    }
}