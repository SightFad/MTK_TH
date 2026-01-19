using TH1.DTOs;
using TH1.Models;

namespace TH1.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateProduct(ProductDto productDto);
        Task<ProductDto> UpdateProduct(int id, ProductDto productDto);
        Task DeleteProduct(int id);
    }
}
