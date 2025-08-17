using E_voting.Model;
using Microsoft.EntityFrameworkCore;

namespace E_voting.DB_CONTEXT
{
    public class EVotingDbContext:DbContext
    {
        public EVotingDbContext(DbContextOptions<EVotingDbContext> options) : base(options)
        {
        }

        public DbSet<Voter> Voters { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

    }
}
