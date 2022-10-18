using Catalog.API.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Entities.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ICatalogContext _catelogContext;
        public ProductRepository(ICatalogContext catelogContext)
        {
            _catelogContext = catelogContext ?? throw new ArgumentNullException(nameof(catelogContext));
        }

        public async Task CreateProduct(Product product)
        {
            await _catelogContext.Products.InsertOneAsync(product);
        } 

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _catelogContext
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catelogContext.
                          Products.
                          Find(p => p.Id == id).
                          FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _catelogContext.
                Products.
                Find(filter).
                ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _catelogContext.
                Products.
                Find(filter).
                ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catelogContext.
                Products.
                Find(prop => true).
                ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _catelogContext
                .Products
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
