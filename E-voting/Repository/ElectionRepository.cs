using E_voting.DB_CONTEXT;
using E_voting.Model;

namespace E_voting.Repository
{
    public class ElectionRepository : CommonRepository<Election>, IElectionRepository
    {
        private readonly EVotingDbContext _dbContext;
        public ElectionRepository(EVotingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
