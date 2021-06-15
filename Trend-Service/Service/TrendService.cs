using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trend_Service.Context;
using Trend_Service.Interfaces;
using Trend_Service.Models;

namespace Trend_Service.Service
{
    public class TrendService : ITrendService
    {
        private readonly TrendServiceContext _context;

        public TrendService(TrendServiceContext context)
        {
            _context = context;
        }

        public async Task<Trend> CreateTrend(Trend trend)
        {
            trend.Id = Guid.NewGuid();
            await _context.Trend.AddAsync(trend);
            await _context.SaveChangesAsync();

            return trend;
        }

        public IEnumerable<TrendDTO> GetTopTrends()
        {
            var desiredTable = _context.Trend.GroupBy(o => o.Name,
                                        o => 1, // don't need the whole object
                                        (key, g) => new { key, count = g.Sum() });
            var desiredDictionary = desiredTable.ToDictionary(x => x.key, x => x.count);

            List<TrendDTO> trends = new();
            foreach (var item in desiredDictionary)
            {
                TrendDTO trendDTO = new()
                {
                    Trend = item.Key,
                    Count = item.Value
                };
                trends.Add(trendDTO);
            }
            // TODO: propely need an 
            return trends.Take(10);
        }
        
    }
}
