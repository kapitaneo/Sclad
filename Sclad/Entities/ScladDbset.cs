using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Sclad.Entities
{
    class ScladDbset: DbContext
    {
       public ScladDbset(string connection)
        {
            Database.Connection.ConnectionString = connection;
        }

        public DbSet<Good> Goods { get; set; }
        public DbSet<Selling> Sellings { get; set; }
    }

    class Good
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Instock { get; set; }
        public ICollection<Selling> Sellings { get; set; }
        public Good()
        {
            Sellings = new List<Selling>();
        }
    }

    class Selling
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public DateTime Date { get; set; }
        public double Cost { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double Sumsell { get; set; }
        public double Profit { get; set; }
        public int? GoodId { get; set; }
        public Good Good { get; set; }
    }

}
