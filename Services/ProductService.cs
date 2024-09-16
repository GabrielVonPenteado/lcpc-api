using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ServiceResult<IEnumerable<Product>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<Product>>
            {
                Success = true,
                Data = products
            };
        }

        public async Task<ServiceResult<Product>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<Product>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                result.Success = false;
                result.Message = $"Product with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = product;
            return result;
        }

        public async Task<ServiceResult> AddAsync(Product product)
        {
            var result = new ServiceResult();

            if (product == null)
            {
                result.Success = false;
                result.Message = "Product cannot be null.";
                return result;
            }

            if (product.Value <= 0)
            {
                result.Success = false;
                result.Message = "Product value must be greater than zero.";
                return result;
            }

            await _productRepository.AddAsync(product);
            result.Success = true;
            result.Message = "Product added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(ProductUpdateDto productDto)
        {
            var result = new ServiceResult();

            var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
            if (existingProduct == null)
            {
                result.Success = false;
                result.Message = "Product not found.";
                return result;
            }

            if (productDto.Value <= 0)
            {
                result.Success = false;
                result.Message = "Product value must be greater than zero.";
                return result;
            }

            existingProduct.Name = productDto.Name;
            existingProduct.ProductType = productDto.ProductType;
            existingProduct.Description = productDto.Description;
            existingProduct.Value = productDto.Value;
            existingProduct.Thickness = productDto.Thickness;
            existingProduct.Width = productDto.Width;
            existingProduct.Length = productDto.Length;

            await _productRepository.UpdateAsync(existingProduct);
            result.Success = true;
            result.Message = "Product updated successfully.";
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var result = new ServiceResult();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                result.Success = false;
                result.Message = $"Product with ID {id} not found.";
                return result;
            }

            await _productRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "Product deleted successfully.";
            return result;
        }
    }
}
