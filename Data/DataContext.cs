using Microsoft.EntityFrameworkCore;
using OnlineShopping.API.Models;

namespace OnlineShopping.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options){}

        //Values will be the table name 
        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Coutries> tbl_Country {get; set;}
    }
}