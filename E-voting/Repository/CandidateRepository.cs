using E_voting.DB_CONTEXT;
using E_voting.Model;

namespace E_voting.Repository;

public class CandidateRepository:CommonRepository<Candidate>,ICandidateRepository
{
    private readonly EVotingDbContext _context;

    public CandidateRepository(EVotingDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }
}
