using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPS.Web.Host.HangFire.ConfigModels
{
    public class HangFireJobsConfig
    {
        public string InsertDataFromPowerBiRestApiIntoDatabaseWorkerCronExpression { get; set; }
    }
}
