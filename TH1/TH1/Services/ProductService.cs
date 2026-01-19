using TH1.DTOs;
using TH1.Models;
using TH1.Repositories;

namespace TH1.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock
            };
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            productDto.ProductId = product.ProductId;
            return productDto;
        }

        public async Task<ProductDto> UpdateProduct(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return productDto;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return;
            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}
