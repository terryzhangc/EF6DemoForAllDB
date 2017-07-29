using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestLiteDB
{
    // Create your POCO class
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Open database (or create if doesn't exist)

            var dbPath = Directory.GetCurrentDirectory() + "\\..\\..\\MyData.db";

            if (File.Exists(dbPath))
                ;
            using (var db = new LiteDatabase(dbPath))
            {
                // Get customer collection
                var col = db.GetCollection<Customer>("customers");

                // Create your new customer instance
                var customer = new Customer
                {
                    Name = "John Doe",
                    Phones = new string[] { "8000-0000", "9000-0000" },
                    Age = 39,
                    IsActive = true
                };

                // Create unique index in Name field
                col.EnsureIndex(x => x.Name, true);

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(customer);

                // Update a document inside a collection
                customer.Name = "Joana Doe";

                col.Update(customer);

                // Use LINQ to query documents (will create index in Age field)
                var results = col.Find(x => x.Age > 20);
            }
        }
    }
}
