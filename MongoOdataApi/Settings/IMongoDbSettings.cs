namespace MongoOdataApi.Settings
{
    public interface IMongoDbSettings
    {
        string PessoasCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
