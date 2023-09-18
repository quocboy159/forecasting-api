using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Models;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class BambooHRMetadataSyncJob : BaseSyncJob
    {
        public override string DataSyncType => DataSyncTypes.BambooHRSyncSkillsJob;

        public override string Source => DataSyncSources.BambooHRSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.BambooHRSyncSkillJobOrder;

        private readonly IBambooHRConnectionFactory _bambooHRConnectionFactory;
        private readonly IMapper _mapper;
        private readonly ISkillsetPersistentService _skillsetPersistentService;
        public BambooHRMetadataSyncJob(IDataSyncServiceLogger dataSyncLogger 
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService
            , IBambooHRConnectionFactory bambooHRConnectionFactory
            , IMapper mapper
            , ISkillsetPersistentService skillsetPersistentService) 
            : base(dataSyncLogger , datetimeHelper , dataSyncProcessService)
        {
            _bambooHRConnectionFactory= bambooHRConnectionFactory;
            _mapper= mapper;
            _skillsetPersistentService= skillsetPersistentService;
        }

        protected override Task SyncData(CancellationToken cancellationToken , DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            
            IBambooHRApi bambooHRApi = _bambooHRConnectionFactory.GetBambooHrHttpClient();
            BHRMetaList[] metaLists = bambooHRApi.GetBambooHRMetaLists().Result;

            BHRMetaList? skillsList = metaLists.FirstOrDefault(x => x.Name == Constants.BambooHRMetaTypes.Skill);
            if (skillsList != null && skillsList.Options.Any())
            {
                int count = skillsList.Options.Count();
                Skillset[] skillsets = _mapper.Map<Skillset[]>(skillsList.Options);
                _skillsetPersistentService.Save(skillsets);
            }
            return Task.CompletedTask;
        }
        }
    }
