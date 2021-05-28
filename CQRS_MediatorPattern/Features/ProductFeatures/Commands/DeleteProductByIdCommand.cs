using CQRS_MediatorPattern.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_MediatorPattern.Features.ProductFeatures.Commands
{
        public class DeleteProductByIdCommand : IRequest<int>
        {
            public int Id { get; set; }
            public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>
            {
                private readonly IApplicationDBContext _context;
                public DeleteProductByIdCommandHandler(IApplicationDBContext context)
                {
                    _context = context;
                }
                public async Task<int> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
                {
                    var product = await _context.Product.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                    if (product == null) 
                    return default;
                    _context.Product.Remove(product);
                    await _context.SaveChangesAsync();
                    return product.Id;
                }
            }
        }
}
