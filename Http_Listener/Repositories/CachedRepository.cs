using Microsoft.Extensions.Caching.Memory;
using SharedParts.Models;

namespace Http_Listener.Repositories
{
    public class CachedRepository : IRepository
    {
        private readonly IRepository _inner;
        private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(10); 
        private const string AllKey = "products:all";
        private static string Key(int id) => $"product:{id}";

        public CachedRepository(IRepository inner) => _inner = inner;

        public Product? GetById(int id)
        {
            if (_cache.TryGetValue(Key(id), out Product? cached))
            {
                Console.WriteLine($"[CACHE] {Key(id)}");
                return cached;
            }

            var product = _inner.GetById(id);
            Console.WriteLine($"[DB] {Key(id)}");
            if (product != null)
                _cache.Set(Key(id), product, Ttl);
            return product;
        }

        public List<Product>? GetAll()
        {
            if (_cache.TryGetValue(AllKey, out List<Product>? cached))
            {
                Console.WriteLine($"[CACHE] {AllKey}");
                return cached;
            }

            var list = _inner.GetAll();
            Console.WriteLine($"[DB] {AllKey}");
            if (list != null)
                _cache.Set(AllKey, list, Ttl);
            return list;
        }

        public Task<bool> Add(Product product)
        {
            var ok = _inner.Add(product);
            if (Convert.ToBoolean(ok)) _cache.Remove(AllKey);
            return ok;
        }

        public Task<bool> Update(Product product)
        {
            var ok = _inner.Update(product);
            if (Convert.ToBoolean(ok))
            {
                _cache.Remove(Key(product.Id));
                _cache.Remove(AllKey);
            }
            return ok;
        }

        public Task<bool> Delete(int id)
        {
            var ok = _inner.Delete(id);
            if (Convert.ToBoolean(ok))
            {
                _cache.Remove(Key(id));
                _cache.Remove(AllKey);
            }
            return ok;
        }
    }
}