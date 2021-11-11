using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using Microsoft.AspNetCore.Http;

namespace DataImporter.Web.Models
{
    public class DashBoardModel
    {
        private  IGroupService _groupService;
        private  IMapper _mapper;
        private  IHttpContextAccessor _httpContextAccessor;
        private ILifetimeScope _scope;
        public int GroupNumber { get; set; }
        public int ImportNumber { get; set; }
        public int ExportNumber { get; set; }
        public DashBoardModel()
        {
            
        }

        public DashBoardModel(IGroupService groupService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _groupService = groupService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        internal void loadData(Guid applicationUserId)
        {
            GroupNumber = _groupService.GetAllGroup(applicationUserId).Count;
            ImportNumber = _groupService.GetAllImportData(applicationUserId);
            ExportNumber = _groupService.GetAllExportData(applicationUserId);
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _groupService= _scope.Resolve<IGroupService>();
            _mapper=_scope.Resolve<IMapper>();
            _httpContextAccessor=_scope.Resolve<IHttpContextAccessor>();
        }
    }
}
