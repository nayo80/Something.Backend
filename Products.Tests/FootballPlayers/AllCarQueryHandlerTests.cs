using FluentAssertions;
using Moq;
using Products.Application.Handlers.FootballPlayer;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Xunit;

namespace Products.Tests.FootballPlayers;

public class AllPlayerQueryHandlerTests
{
    private readonly Mock<IGenericRepository<FootballPlayerModel>> _repoMock;
    private readonly Mock<IElasticEngineService> _elasticMock;
    private readonly AllPlayerQueryHandler _handler;

    public AllPlayerQueryHandlerTests()
    {
        _repoMock = new Mock<IGenericRepository<FootballPlayerModel>>();
        _elasticMock = new Mock<IElasticEngineService>();
        
        _handler = new AllPlayerQueryHandler(
            _repoMock.Object, 
            _elasticMock.Object);
    }

    [Fact]
    public async Task Handle_ElasticHasData_ShouldReturnElasticData_AndNotCallRepository()
    {
        // ARRANGE
        var query = new AllPlayerQuery();
        var elasticPlayers = new List<FootballPlayerModel> { new() { Id = 1, FirstName = "Elastic Player" } };

        _elasticMock.Setup(e => e.GetAllProductsAsync<FootballPlayerModel>())
                    .ReturnsAsync(elasticPlayers);

        // ACT
        var response = await _handler.Handle(query, CancellationToken.None);

        // ASSERT
        response.Result.Should().BeEquivalentTo(elasticPlayers);
        
        _elasticMock.Verify(e => e.GetAllProductsAsync<FootballPlayerModel>(), Times.Once);
        
        _repoMock.Verify(r => r.ReadAllAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ElasticIsNull_ShouldFallbackToRepository()
    {
        // ARRANGE
        var query = new AllPlayerQuery();
        var dbPlayers = new List<FootballPlayerModel> { new() { Id = 2, FirstName = "Lionel" } };

        // Elastic returns null
        _elasticMock.Setup(e => e.GetAllProductsAsync<FootballPlayerModel>())
                    .ReturnsAsync((IEnumerable<FootballPlayerModel>?)null);

        _repoMock.Setup(r => r.ReadAllAsync())
                 .ReturnsAsync(dbPlayers);

        // ACT
        var response = await _handler.Handle(query, CancellationToken.None);

        // ASSERT
        response.Result.Should().BeEquivalentTo(dbPlayers);
        
        _elasticMock.Verify(e => e.GetAllProductsAsync<FootballPlayerModel>(), Times.Once);
        _repoMock.Verify(r => r.ReadAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_BothElasticAndDbAreNull_ShouldThrowException()
    {
        // ARRANGE
        var query = new AllPlayerQuery();
        
        _elasticMock.Setup(e => e.GetAllProductsAsync<FootballPlayerModel>())
                    .ReturnsAsync((IEnumerable<FootballPlayerModel>?)null);

        _repoMock.Setup(r => r.ReadAllAsync())
                 .ReturnsAsync((IEnumerable<FootballPlayerModel>?)null);

        // ACT
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("*Players not found*");
    }
}