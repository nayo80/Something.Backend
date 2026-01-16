using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services.ElasticSearch;

public class ElasticEngineService : IElasticEngineService
{
    private readonly ElasticsearchClient _client;
    private readonly string _defaultIndex;
    private readonly ILogger<ElasticEngineService> _logger;

    public ElasticEngineService(IConfiguration configuration, ILogger<ElasticEngineService> logger)
    {
        _logger = logger;
        var settingsSection = configuration.GetSection("ElasticSettings");

        var uri = new Uri(settingsSection["Uri"] ?? string.Empty);
        var userName = settingsSection["UserName"] ?? string.Empty;
        var password = settingsSection["Password"] ?? string.Empty;
        _defaultIndex = settingsSection["DefaultIndex"] ?? string.Empty;

        var settings = new ElasticsearchClientSettings(uri)
            .Authentication(new BasicAuthentication(userName, password))
            .DefaultIndex(_defaultIndex);

        _client = new ElasticsearchClient(settings);
    }

    public async Task IndexProductAsync<T>(T product)
    {
        var response = await _client.IndexAsync(product, idx => idx.Index(_defaultIndex));

        if (!response.IsValidResponse)
        {
            _logger.LogError("Failed to index product: {Message}", response.DebugInformation);
        }
        else
        {
            _logger.LogInformation($"Product {typeof(T)} successfully indexed");
        }
    }

    public async Task<T?> GetProductAsync<T>(int id) where T : class
    {
        var response = await _client.GetAsync<T>(id, idx => idx.Index(_defaultIndex));

        if (response.Found)
        {
            _logger.LogWarning("Product with ID {Id} successfully found in Elasticsearch", id);
            return response.Source;
        }

        _logger.LogWarning("Product with ID {Id} not found in Elasticsearch", id);
        return null;
    }

    public async Task UpdateProductAsync<T>(int id, T updatedProduct) where T : class
    {
        var response = await _client.UpdateAsync(new UpdateRequest<T, T>(_defaultIndex, id)
        {
            Doc = updatedProduct
        });

        if (!response.IsValidResponse)
        {
            _logger.LogError("Failed to update product with ID {Id}: {Message}", id, response.DebugInformation);
        }
        else
        {
            _logger.LogInformation("Product with ID {Id} successfully updated", id);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _client.DeleteAsync<object>(id, d => d.Index(_defaultIndex));

        if (!response.IsValidResponse)
        {
            _logger.LogError("Failed to delete product with ID {Id}: {Message}", id, response.DebugInformation);
        }
        else
        {
            _logger.LogInformation("Product with ID {Id} successfully deleted", id);
        }
    }
    
    public async Task<IEnumerable<T>?> GetAllProductsAsync<T>() where T : class
    {
        var response = await _client.SearchAsync<T>(s => s
                .Index(_defaultIndex)
                .From(1)  
                .Size(100)    
                .Query(q => q.MatchAll()) 
        );

        if (!response.IsValidResponse)
        {
            _logger.LogError("Failed to retrieve all products: {Message}", response.DebugInformation);
            return Enumerable.Empty<T>();
        }

        _logger.LogInformation("Successfully retrieved {Count} documents", response.Documents.Count);
        return response.Documents;
    }
}