using ForecastingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class DataSyncProcessModel
    {
        public int DataSyncProcessId { get; set; }

        public string DataSyncType { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public DateTime LastSyncDateTime { get; set; }

        public DataSyncProcessStatuses Status { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime? FinishDateTime { get; set; }
    }
}
