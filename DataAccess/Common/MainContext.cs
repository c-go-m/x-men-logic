using DataAccess.Common.Interfaces;
using MongoDB.Driver;

namespace DataAccess.Common
{
    public class MainContext : IMainContext
    {
        private readonly IMongoDatabase mongoDatabase;

        public MainContext(MongoSettings configuration)
        {            
            IMongoClient mongoClient = new MongoClient(configuration.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(configuration.DatabaseName);
        }

        /// <summary>
        /// Obtiene la coleccion de base de datos
        /// </summary>
        /// <typeparam name="T">Entidad que representa la colección en mongo</typeparam>
        /// <param name="name">nombre opcional de la colleción</param>
        /// <returns>coleccion de mongo</returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return mongoDatabase.GetCollection<T>(name);
        }
    }
}
