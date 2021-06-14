using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trend_Service.Context;
using Trend_Service.Interfaces;

namespace Trend_Service.Service
{
    public class TrendService : ITrendService
    {
        private readonly TrendServiceContext _context;

        public TrendService(TrendServiceContext context)
        {
            _context = context;
        }


    }
}
