using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BeanSceneReservationApplication.Models.APIModel;
using Microsoft.EntityFrameworkCore;
using BeanSceneReservationApplication.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneReservationApplication.Services
{
    public class OrderDBService
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _ordersCollection = database.GetCollection<Order>(mongoDBSettings.Value.Orders);
        }

        public async Task CreateAsync(Order order)
        {
            await _ordersCollection.InsertOneAsync(order);
            return;
        }

    }
}
