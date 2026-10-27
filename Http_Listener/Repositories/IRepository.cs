using SharedParts.Models;

namespace Http_Listener.Repositories
{
    public interface IRepository
    {
        Product? GetById(int id);
        List<Product>? GetAll();
        Task<bool> Add(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(int id);
    }
}
