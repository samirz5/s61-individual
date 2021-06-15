using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trend_Service.Models;

namespace Trend_Service.Interfaces
{
    public interface ITrendService
    {
        Task<Trend> CreateTrend(Trend trend);
        IEnumerable<TrendDTO> GetTopTrends();
    }
}
