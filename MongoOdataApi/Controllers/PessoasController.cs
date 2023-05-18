using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoOdataApi.Models;
using MongoOdataApi.Settings;
using System.Threading.Tasks;

namespace MongoOdataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IMongoCollection<Pessoa> _pessoas;
        private readonly ILogger<PessoasController> _logger;

        public PessoasController(IMongoDbSettings settings, ILogger<PessoasController> logger)
        {
            _logger = logger;
            var mongoConnectionUrl = new MongoUrl(settings.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            mongoClientSettings.ClusterConfigurator = cb => 
            {
                cb.Subscribe<CommandStartedEvent>(e => 
                {
                    logger.LogInformation($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };
            var client = new MongoClient(mongoClientSettings);
            var database = client.GetDatabase(settings.DatabaseName);
            _pessoas = database.GetCollection<Pessoa>(settings.PessoasCollectionName);
        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public IActionResult Get() => Ok(_pessoas.AsQueryable());

        [HttpPost]
        public async Task<IActionResult> Post(Pessoa pessoa)
        {
            await _pessoas.InsertOneAsync(pessoa);
            return Ok();
        }
    }
}
