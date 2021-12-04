using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Services.Implementation
{
    public class AppsToSellService : IAppsToSellService
    {
        private readonly AppsToSellRepository _appsToSellRepository;

        public AppsToSellService(AppsToSellRepository appsToSellRepository)
        {
            _appsToSellRepository = appsToSellRepository;
        }

        public async Task<List<AppsToSell>> FindAll()
        {
            var apps = await _appsToSellRepository.FindAll();
            
            if (apps.Count is not 0)
                return apps;

            apps = new AppsToSell().CreateSamples();

            await _appsToSellRepository.InsertMany(apps);

            return apps;
        }
    }
}
