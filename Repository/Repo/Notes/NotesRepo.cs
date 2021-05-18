using FundooRepository.DbContexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooRepository.Repo.Notes
{
    public class NotesRepo : INotesRepo
    {
        public readonly UserDbContext context;
        public readonly IConfiguration config;
        public NotesRepo(UserDbContext userDbContext, IConfiguration configuration)
        {
            this.context = userDbContext;
            this.config = configuration;

        }
    }
}
