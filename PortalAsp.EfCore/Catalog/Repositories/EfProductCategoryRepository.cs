using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalModels;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;
using PortalModels.Catalog.Repositories.Products;

namespace PortalAsp.EfCore.Catalog.Repositories
{
    public class EfProductCategoryRepository : IProductCategoryRepository
    {
        public CatalogContext CatalogContext { get; set; }
        public EfProductCategoryRepository(CatalogContext catalogContext) => CatalogContext = catalogContext;

        public IQueryable<ProductCategory> GetAllProductCategories()
        {
            return CatalogContext.ProductCategories.AsNoTracking();
        }

        public async Task<GeneralResult> AddProductCategoryAsync(ProductCategory productCategory, long subCategoryId)
        {
            CatalogSubCategory subCategory = await CatalogContext.CatalogSubCategories
                .Include(sc => sc.ProductCategories)
                .FirstOrDefaultAsync(sc => sc.CatalogSubCategoryId == subCategoryId);
            if (subCategory is null)
            {
                return new GeneralResult
                {
                    Success = false,
                    Message = "No such subcategory or no subcategory id is provided"
                };
            }

            subCategory.ProductCategories ??= new List<ProductCategory>();
            subCategory.ProductCategories.Add(productCategory);
            await CatalogContext.SaveChangesAsync();

            return new GeneralResult {Success = true};
        }

        public async Task UpdateProductCategoryAsync(ProductCategory productCategory)
        {
            ProductCategory existingProductCategory = CatalogContext.ProductCategories.FirstOrDefault(pc =>
                pc.ProductCategoryId == productCategory.ProductCategoryId);
            if (existingProductCategory == null) throw new Exception("No such product category");

            existingProductCategory.Name = productCategory.Name;
            CatalogSubCategory parentSubCategory = CatalogContext.CatalogSubCategories.FirstOrDefault(sc =>
                sc.CatalogSubCategoryId == productCategory.ParentSubCategory.CatalogSubCategoryId);
            existingProductCategory.ParentSubCategory = parentSubCategory;
            await CatalogContext.SaveChangesAsync();
        }

        public async Task DeleteProductCategoryAsync(ProductCategory productCategory)
        {
            CatalogContext.ProductCategories.Remove(productCategory);
            await CatalogContext.SaveChangesAsync();
        }
    }
}
