using E_voting.DB_CONTEXT;
using E_voting.Model;
using Microsoft.EntityFrameworkCore;


namespace E_voting.Repository
{
    public class AdminRepository:CommonRepository<Admin>,IAdminRepository
    {
        private readonly EVotingDbContext _dbContext;

        public AdminRepository(EVotingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
