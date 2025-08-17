using E_voting.DB_CONTEXT;
using E_voting.Model;

namespace E_voting.Repository;

public class VoterRepository:CommonRepository<Voter>,IVoterRepository
{
    private readonly EVotingDbContext _context;

    public VoterRepository(EVotingDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }
}
