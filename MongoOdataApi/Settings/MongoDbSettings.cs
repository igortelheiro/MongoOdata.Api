namespace MongoOdataApi.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string PessoasCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
