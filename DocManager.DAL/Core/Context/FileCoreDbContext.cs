using DocManager.DAL.Entities;
using FileContextCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocManager.DAL.Core.Context
{
    public class FileCoreDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public FileCoreDbContext(DbContextOptions<FileCoreDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseFileContextDatabase();
        }
    }
}
