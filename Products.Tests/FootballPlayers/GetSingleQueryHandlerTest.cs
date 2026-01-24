using FluentAssertions;
using Moq;
using Products.Application.Handlers.FootballPlayer;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.Redis.Service;
using Shared.Responses;
using Xunit;

namespace Products.Tests.FootballPlayers
{
    public class GetSingleQueryHandlerTest
    {
        private readonly Mock<IGenericRepository<FootballPlayerModel>> _repoMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly SinglePlayerQueryHandler _handler;

        public GetSingleQueryHandlerTest()
        {
            _repoMock = new Mock<IGenericRepository<FootballPlayerModel>>();
            _cacheMock = new Mock<ICacheService>();

            _handler = new SinglePlayerQueryHandler(
                _repoMock.Object,
                _cacheMock.Object);
        }

        [Fact]
        public async Task Handle_PlayerInCache_ShouldReturnCachedPlayer()
        {
            // ARRANGE
            var playerId = 1;
            var query = new SinglePlayerQuery(playerId);

            var cachedPlayer = new FootballPlayerModel
            {
                Id = playerId,
                FirstName = "Cristiano",
                LastName = "Ronaldo",
                FootballClub = "Al Nassr"
            };

            _cacheMock.Setup(c => c.GetAsync<FootballPlayerModel>($"player:{playerId}", It.IsAny<CancellationToken>()))
                      .ReturnsAsync(cachedPlayer);

            // ACT
            BaseResponse<FootballPlayerModel> response = await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            response.Result.Should().BeEquivalentTo(cachedPlayer);
            _cacheMock.Verify(c => c.GetAsync<FootballPlayerModel>($"player:{playerId}", It.IsAny<CancellationToken>()), Times.Once);
            _repoMock.Verify(r => r.ReadAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_PlayerNotInCache_ShouldReadFromDb_AndCacheIt()
        {
            // ARRANGE
            var playerId = 2;
            var query = new SinglePlayerQuery(playerId);
            var dbPlayer = new FootballPlayerModel { Id = playerId, FirstName = "Kylian", LastName = "mbappe",FootballClub = "realMadrid"};

            _cacheMock.Setup(c => c.GetAsync<FootballPlayerModel>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((FootballPlayerModel)null);

            _repoMock.Setup(r => r.ReadAsync(playerId))
                .ReturnsAsync(dbPlayer);

            // ACT
            var response = await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            response.Result.Should().BeEquivalentTo(dbPlayer);
    
            _repoMock.Verify(r => r.ReadAsync(playerId), Times.Once);
    
            _cacheMock.Verify(c => c.SetAsync(
                $"player:{playerId}", 
                dbPlayer, 
                It.IsAny<TimeSpan?>(), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidId_ShouldThrowArgumentException()
        {
            // ARRANGE
            var query = new SinglePlayerQuery(0); // Invalid ID

            // ACT
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("*Invalid Player ID*");
        }

        [Fact]
        public async Task Handle_PlayerNotInCacheAndDb_ShouldReturnNull()
        {
            // ARRANGE
            var playerId = 3;
            var query = new SinglePlayerQuery(playerId);

            _cacheMock.Setup(c => c.GetAsync<FootballPlayerModel>($"player:{playerId}", It.IsAny<CancellationToken>()))
                      .ReturnsAsync((FootballPlayerModel)null);

            _repoMock.Setup(r => r.ReadAsync(playerId))
                     .ReturnsAsync((FootballPlayerModel)null);

            // ACT
            BaseResponse<FootballPlayerModel> response = await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            response.Result.Should().BeNull();
            _repoMock.Verify(r => r.ReadAsync(playerId), Times.Once);
            _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<FootballPlayerModel>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
