using CQRS_MediatorPattern.Context;
using CQRS_MediatorPattern.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_MediatorPattern.Features.ProductFeatures.Queries
{
   public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
   {
        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
        {
            private readonly IApplicationDBContext _context;
            private readonly IDistributedCache _distributedCache;

            public GetAllProductsQueryHandler(IApplicationDBContext context, IDistributedCache distributedCache)
            {
                _context = context;
                _distributedCache = distributedCache;
            }
            public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
            {
                var cacheKey = "productList";
                string serializedProductList;
                var productList = new List<Product>();
                var redisProductList = await _distributedCache.GetAsync(cacheKey);
                if (redisProductList != null)
                {
                    serializedProductList = Encoding.UTF8.GetString(redisProductList);
                    productList = JsonConvert.DeserializeObject<List<Product>>(serializedProductList);
                }
                else
                {
                    productList = await _context.Product.AsNoTracking().ToListAsync();
                    serializedProductList = JsonConvert.SerializeObject(productList);
                    redisProductList = Encoding.UTF8.GetBytes(serializedProductList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await _distributedCache.SetAsync(cacheKey, redisProductList, options);
                }
                return productList;
            }



        }
    }
}
