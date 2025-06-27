using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Product;
using ECommerceAIMockUp.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductBasicDto> GetProductBasicAsync(int productId)
    {
        var product = await _productRepository.GetByIdWithVariantsAsync(productId);

        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        var sizes = product.ProductDetails
            .Select(v => v.Size)
            .Distinct()
            .OrderBy(s => s)
            .ToList();

        return new ProductBasicDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            AvailableSizes = sizes
        };
    }

    public async Task<ColorOptionsDto> GetAvailableColorsAsync(int productId, string size)
    {
        var product = await _productRepository.GetByIdWithVariantsAsync(productId);

        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        var colors = product.ProductDetails
            .Where(v => v.Size.Equals(size, StringComparison.OrdinalIgnoreCase))
            .Select(v => v.Color)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        if (!colors.Any())
            throw new InvalidOperationException("No colors found for the specified size.");

        return new ColorOptionsDto
        {
            Size = size,
            AvailableColors = colors
        };
    }

    public async Task<ProductVariantDto> GetVariantAsync(int productId, string size, string color)
    {
        var product = await _productRepository.GetByIdWithVariantsAsync(productId);

        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        var variant = product.Variants
            .FirstOrDefault(v =>
                v.Size.Equals(size, StringComparison.OrdinalIgnoreCase) &&
                v.Color.Equals(color, StringComparison.OrdinalIgnoreCase));

        if (variant == null)
            throw new InvalidOperationException("Variant not found for the given size and color.");

        return new ProductVariantDto
        {
            ProductId = productId,
            Size = variant.Size,
            Color = variant.Color,
            Price = variant.Price,
            Stock = variant.Stock
        };
    }
}

