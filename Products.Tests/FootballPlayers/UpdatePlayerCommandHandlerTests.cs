using FluentAssertions;
using MapsterMapper;
using Moq;
using Products.Application.Commands.FootballPlayer;
using Products.Application.Dtos.FootballPlayers;
using Products.Application.Handlers.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Services.Redis.Service;
using Xunit;

namespace Products.Tests.FootballPlayers;

public class UpdatePlayerCommandHandlerTests
{
    private readonly Mock<IGenericRepository<FootballPlayerModel>> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IElasticEngineService> _elasticMock;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly UpdatePlayerCommandHandler _handler;

    public UpdatePlayerCommandHandlerTests()
    {
        _repoMock = new Mock<IGenericRepository<FootballPlayerModel>>();
        _mapperMock = new Mock<IMapper>();
        _elasticMock = new Mock<IElasticEngineService>();
        _cacheMock = new Mock<ICacheService>();

        _handler = new UpdatePlayerCommandHandler(
            _repoMock.Object,
            _mapperMock.Object,
            _elasticMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateAndInvalidateCache()
    {
        // ARRANGE
        var playerId = 1;
        var requestDto = new RequestFootballPlayer 
        { 
            FirstName = "Luka", 
            LastName = "Modric", 
            FootballClub = "Real Madrid" 
        };
        
        var command = new UpdatePlayerCommand(playerId, requestDto);
        
        var playerModel = new FootballPlayerModel 
        { 
            Id = playerId, 
            FirstName = requestDto.FirstName, 
            LastName = requestDto.LastName 
        };

        _mapperMock.Setup(m => m.Map<FootballPlayerModel>(command.Player))
                   .Returns(playerModel);

        _repoMock.Setup(r => r.UpdateAsync(playerId, playerModel))
                 .ReturnsAsync(true);

        // ACT
        var response = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        response.Result.Should().BeTrue();

        _mapperMock.Verify(m => m.Map<FootballPlayerModel>(It.IsAny<RequestFootballPlayer>()), Times.Once);
        
        _repoMock.Verify(r => r.UpdateAsync(playerId, playerModel), Times.Once);

        _elasticMock.Verify(e => e.UpdateProductAsync(playerId, playerModel), Times.Once);

        _cacheMock.Verify(c => c.RemoveAsync($"player:{playerId}", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPlayerUpdateFails_ShouldThrowException()
    {
        // ARRANGE
        var playerId = 99;
        var requestDto = new RequestFootballPlayer { FirstName = "Test" };
        var command = new UpdatePlayerCommand(playerId, requestDto);
        var playerModel = new FootballPlayerModel { Id = playerId };

        _mapperMock.Setup(m => m.Map<FootballPlayerModel>(command.Player))
                   .Returns(playerModel);

        _repoMock.Setup(r => r.UpdateAsync(playerId, playerModel))
                 .ReturnsAsync(false);

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<Exception>(); 

        _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}