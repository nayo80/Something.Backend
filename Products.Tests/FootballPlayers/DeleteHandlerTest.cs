using FluentAssertions;
using Moq;
using Products.Application.Commands.FootballPlayer;
using Products.Application.Handlers.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Responses;
using Xunit;

namespace Products.Tests.FootballPlayers
{
    public class DeletePlayerCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<FootballPlayerModel>> _repoMock;
        private readonly Mock<IElasticEngineService> _elasticMock;
        private readonly DeletePlayerCommandHandler _handler;

        public DeletePlayerCommandHandlerTests()
        {
            _repoMock = new Mock<IGenericRepository<FootballPlayerModel>>();
            _elasticMock = new Mock<IElasticEngineService>();

            _handler = new DeletePlayerCommandHandler(
                _repoMock.Object,
                _elasticMock.Object);
        }

        [Fact]
        public async Task Handle_ValidId_Should_DeletePlayer_And_RemoveFromElastic()
        {
            // ARRANGE
            var playerId = 1;
            var command = new DeletePlayerCommand(playerId);

            _repoMock
                .Setup(r => r.DeleteAsync(playerId))
                .ReturnsAsync(true);

            _elasticMock
                .Setup(e => e.DeleteProductAsync(playerId))
                .Returns(Task.CompletedTask);

            // ACT
            BaseResponse<bool> response = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            response.Result.Should().BeTrue();

            _repoMock.Verify(r => r.DeleteAsync(playerId), Times.Once);
            _elasticMock.Verify(e => e.DeleteProductAsync(playerId), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteFails_ShouldThrowException()
        {
            // ARRANGE
            var playerId = 1;
            var command = new DeletePlayerCommand(playerId);

            _repoMock
                .Setup(r => r.DeleteAsync(playerId))
                .ReturnsAsync(false);

            // ACT
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("player  cannot be Deleted");
        }
    }
}
