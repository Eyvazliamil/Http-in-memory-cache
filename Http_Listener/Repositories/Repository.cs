using SharedParts.Contexts;
using SharedParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Http_Listener.Repositories
{
    public class Repository : IRepository
    { 
        public async Task<bool> Add(Product product)
        {
            using var DB = new Context();
            await DB.Products.AddAsync(product);
            return await DB.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var DB = new Context();
            var p = DB.Products.FirstOrDefault(x => x.Id == id);
            if (p == null) return false;
            DB.Products.Remove(p);
            return await DB.SaveChangesAsync() > 0;
        }

        public List<Product>? GetAll()
        {
            using var DB = new Context();
            return DB.Products.ToList();
        }

        public Product? GetById(int id)
        {
            using var DB = new Context();
            return DB.Products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> Update(Product product)
        {
            using var DB = new Context();
            var old = DB.Products.FirstOrDefault(p => p.Id == product.Id);
            if (old == null) return false;
            old.Name = product.Name;
            old.Price = product.Price;
            old.Category = product.Category;
            return await DB.SaveChangesAsync() > 0;
        }
    }
}
