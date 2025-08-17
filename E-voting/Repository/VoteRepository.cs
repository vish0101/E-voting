using E_voting.DB_CONTEXT;
using E_voting.Model;

namespace E_voting.Repository
{
    public class VoteRepository:CommonRepository<Vote> , IVoteRepository
    {
        private readonly EVotingDbContext _dbContext;

        public VoteRepository(EVotingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
