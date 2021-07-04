using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalModels;
using PortalModels.Catalog.Products;
using PortalModels.Catalog.Repositories.Products;

namespace PortalAsp.EfCore.Catalog.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        public CatalogContext CatalogContext { get; set; }
        public EfProductRepository(CatalogContext catalogContext) => CatalogContext = catalogContext;

        public IQueryable<Product> GetAllProducts()
        {
            return CatalogContext.Products.AsNoTracking();
        }

        public IQueryable<Product> GetProductsBy(long productCategoryId)
        {
            return CatalogContext.Products.AsNoTracking()
                .Where(p => p.Category != null && p.Category.ProductCategoryId == productCategoryId);
        }

        public async Task<GeneralResult> AddProductAsync(Product product, long productCategoryId)
        {
            ProductCategory productCategory = await CatalogContext.ProductCategories
                .Include(pc => pc.Products)
                .FirstOrDefaultAsync(pc => pc.ProductCategoryId == productCategoryId);
            if (productCategory is null)
            {
                return new GeneralResult
                {
                    Success = false,
                    Message = "No such product category or no product category id is provided"
                };
            }

            productCategory.Products ??= new List<Product>();
            productCategory.Products.Add(product);
            await CatalogContext.SaveChangesAsync();
            return new GeneralResult {Success = true};
        }

        public async Task UpdateProductAsync(Product product)
        {
            Product existingProduct = CatalogContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct == null) throw new Exception("No such product");

            existingProduct.Name = product.Name;
            existingProduct.ShortDescription = product.ShortDescription;
            existingProduct.Price = product.Price;
            existingProduct.Images = product.Images;
            existingProduct.Manufacturer =
                CatalogContext.Manufacturers.FirstOrDefault(
                    m => m.ManufacturerId == product.Manufacturer.ManufacturerId);
            existingProduct.Category =
                CatalogContext.ProductCategories.FirstOrDefault(pc =>
                    pc.ProductCategoryId == product.Category.ProductCategoryId);

            await CatalogContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            CatalogContext.Products.Remove(product);
            await CatalogContext.SaveChangesAsync();
        }
    }
}
