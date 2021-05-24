using Microsoft.EntityFrameworkCore;
using FundooModel.Account;
using System;
using System.Collections.Generic;
using System.Text;
using FundooModel.Notes;

namespace FundooRepository.DbContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Collaborater> Collaborater { get; set; }
    }
}
