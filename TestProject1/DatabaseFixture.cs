using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject1
{
    public class DatabaseFixture : IDisposable
    {
        //internal readonly MyShop_327882650Context? DbContext;

        public MyShop_327882650Context Context { get; private set; }
        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<MyShop_327882650Context>()
                .UseSqlServer("Server =localhost\\SQLEXPRESS ; Database = ShopTest_Integration ; Trusted_Connection = True; TrustServerCertificate = True")
                .Options;
            Context = new MyShop_327882650Context(options);
            Context.Database.EnsureCreated();

        }
        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
