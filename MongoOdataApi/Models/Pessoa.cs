using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoOdataApi.Models
{
    public class Pessoa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id{ get; set; }
        [BsonElement("nome")]
        public string Nome { get; set; }
        [BsonElement("sobrenome")]
        public string Sobrenome { get; set; }
    }
}
