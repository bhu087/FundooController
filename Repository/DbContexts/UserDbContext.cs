using Microsoft.EntityFrameworkCore;
using ModelClass.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.DbContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<Register> AccountRegisters { get; set; }
    }
}
