using FluentAssertions;
using MapsterMapper;
using Moq;
using Products.Application.Commands.FootballPlayer;
using Products.Application.Dtos.FootballPlayers;
using Products.Application.Handlers.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Responses;
using Xunit;

namespace Products.Tests.FootballPlayers
{
    public class CreatePlayerCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<FootballPlayerModel>> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IElasticEngineService> _elasticMock;
        private readonly CreatePlayerCommandHandler _handler;

        public CreatePlayerCommandHandlerTests()
        {
            _repoMock = new Mock<IGenericRepository<FootballPlayerModel>>();
            _mapperMock = new Mock<IMapper>();
            _elasticMock = new Mock<IElasticEngineService>();

            _handler = new CreatePlayerCommandHandler(
                _repoMock.Object,
                _mapperMock.Object,
                _elasticMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_Should_CreatePlayer_And_IndexElastic()
        {
            // ARRANGE
            var playerDto = new RequestFootballPlayer
            {
                FirstName = "Lionel",
                LastName = "Messi",
                FootballClub = "PSG"
            };

            var command = new CreatePlayerCommand(playerDto);

            var mappedPlayer = new FootballPlayerModel
            {
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                FootballClub = playerDto.FootballClub
            };

            const int createdId = 1;

            // Mock Map
            _mapperMock
                .Setup(m => m.Map<FootballPlayerModel>(playerDto))
                .Returns(mappedPlayer);

            // Mock Repository
            _repoMock
                .Setup(r => r.CreateAsync(mappedPlayer))
                .ReturnsAsync(createdId);

            // Mock Elastic
            _elasticMock
                .Setup(e => e.IndexProductAsync(mappedPlayer))
                .Returns(Task.CompletedTask);

            // ACT
            BaseResponse<int> response = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            response.Result.Should().Be(createdId);

            _mapperMock.Verify(m => m.Map<FootballPlayerModel>(playerDto), Times.Once);
            _repoMock.Verify(r => r.CreateAsync(mappedPlayer), Times.Once);
            _elasticMock.Verify(e => e.IndexProductAsync(It.Is<FootballPlayerModel>(p => p.Id == createdId)), Times.Once);
        }

        [Fact]
        public async Task Handle_NullPlayer_ShouldThrowArgumentNullException()
        {
            // ARRANGE
            var command = new CreatePlayerCommand(null);

            // ACT
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
    
}
