using Duende.IdentityServer.EntityFramework.Interfaces;

namespace invensys.iserve.Application.Clients.Queries.GetClients;

public record GetClientsQuery : IRequest<List<ClientDto>>
{
}

public class GetClientsQueryValidator : AbstractValidator<GetClientsQuery>
{
   public GetClientsQueryValidator()
   {
   }
}

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<ClientDto>>
{
   private readonly IConfigurationDbContext _context;
   private readonly IMapper _mapper;

   public GetClientsQueryHandler(IConfigurationDbContext context, IMapper mapper)
   {
      _context = context;
      _mapper = mapper;
   }

   public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
   {
      return await _context.Clients
        .Where(s => s.Enabled)
         .AsNoTracking()
         .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
         .OrderBy(t => t.ClientName)
         .ToListAsync(cancellationToken);
   }
}
