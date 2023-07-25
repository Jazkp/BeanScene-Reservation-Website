using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using BeanSceneReservationApplication.Models.APIModel;


namespace BeanSceneReservationApplication.Services
{
    public class ProductDBService
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _productsCollection = database.GetCollection<Product>(mongoDBSettings.Value.Products);
        }

        public async Task CreateAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
            return;
        }
        public async Task<List<Product>> GetAsync()
        {
            ; return await _productsCollection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(string id, string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
            UpdateDefinition<Product> update = Builders<Product>.Update.AddToSet<string>("Name", name);
            await _productsCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task DeleteAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
            await _productsCollection.DeleteOneAsync(filter);
            return;
        }

        // GET to use find based on Name
        public async Task<Product> GetAsync(string name)
        {
            return await _productsCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

        }

        // GET to use find based on Sittings

        public async Task<ActionResult<IEnumerable<Product>>> GetAsyncSitting(string sitting)
        {
            var filter = Builders<Product>.Filter.AnyEq(p => p.Sittings, sitting);
            var products = await _productsCollection.Find(filter).ToListAsync();
            return products;
        }
    }

}