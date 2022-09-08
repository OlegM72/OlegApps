using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DictionaryApi.Models
{
    public class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options) { }

        public DbSet<DictionaryItem> DictionaryItems { get; set; } = null!;
    }
}