using CQRS_MediatorPattern.Context;
using CQRS_MediatorPattern.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_MediatorPattern.Features.ProductFeatures.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal Rate { get; set; }


        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
        {
            private readonly IApplicationDBContext _context;
            public CreateProductCommandHandler(IApplicationDBContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
            {
                var product = new Product
                {
                    Barcode = command.Barcode,
                    Name = command.Name,
                    BuyingPrice = command.BuyingPrice,
                    Rate = command.Rate,
                    Description = command.Description
                };
                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }
        }
    }
}
