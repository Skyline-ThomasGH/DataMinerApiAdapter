using System;
using System.Collections.Generic;
using System.Linq;
using DataMinerApiAdapter.Models;
using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
using Skyline.DataMiner.Net.Messages.SLDataGateway;
using Skyline.DataMiner.Net.Sections;

namespace DataMinerApiAdapter.Services
{
    public class JobsService
    {
        private readonly DomHelper _domHelper;

        public JobsService(DataMinerAdapterService adapterService)
        {
            _domHelper = new DomHelper(adapterService.HandleMessages, "my_dom_module");
        }

        public List<Job> GetAllJobs()
        {
            var filter = DomInstanceExposers.DomDefinitionId.Equal(JobDomIds.DomDefinitionId.Id);
            var result = _domHelper.DomInstances.Read(filter);

            return result.Select(one => new Job()
            {
                Name = one.GetFieldValue<string>(JobDomIds.SectionDefinitionId, JobDomIds.NameFieldDescriptorId).Value,
                Start = one.GetFieldValue<DateTime>(JobDomIds.SectionDefinitionId, JobDomIds.StartFieldDescriptorId).Value,
                End = one.GetFieldValue<DateTime>(JobDomIds.SectionDefinitionId, JobDomIds.EndFieldDescriptorId).Value,
            }).ToList();
        }
    }

    public class JobDomIds
    {
        public static readonly DomDefinitionId DomDefinitionId =
            new DomDefinitionId(Guid.Parse("e37fa9ba-1d6a-4789-b85d-8190213f5be2"));

        public static readonly SectionDefinitionID SectionDefinitionId =
            new SectionDefinitionID(Guid.Parse("0db4c52a-cbe1-46e4-8e0f-f76adde86819"));

        public static readonly FieldDescriptorID NameFieldDescriptorId =
            new FieldDescriptorID(Guid.Parse("01917961-b626-47df-887e-b5527da6c20a"));

        public static readonly FieldDescriptorID StartFieldDescriptorId =
            new FieldDescriptorID(Guid.Parse("a2cf5218-e595-464d-8700-141c3ef2ddfe"));

        public static readonly FieldDescriptorID EndFieldDescriptorId =
            new FieldDescriptorID(Guid.Parse("5a05e858-64c6-4101-b6b3-8527e68234d5"));
    }
}