using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PokemonAPI.Models;
using PokemonAPI.Services;
using System.Net;
using Xunit;

namespace PokemonAPI.PokemonAPI.Tests
{
    public class PokemonServiceTests
    {
        [Fact]
        public async Task GetPokemonAsync_ReturnsExpectedPokemonDTO()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PokemonService>>();
            var mockHttpClient = new Mock<HttpClient>();

            var pokemonId = 1;

            // Create a mock response data
            var jsonResponse = new
            {
                forms = new[] { new { name = "bulbasaur" } },
                types = new[] { new { type = new { name = "grass" } } },
                base_experience = 64
            };

            var responseMessage = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(jsonResponse))
            };


            var service = new PokemonService(mockLogger.Object, mockHttpClient.Object);

            // Act
            var result = await service.GetPokemonAsync(pokemonId);

            // Assert
            Assert.Equal(pokemonId, result.Id);
            Assert.Equal("bulbasaur", result.Name);
            Assert.Equal("grass", result.Type);
            Assert.Equal(64, result.BaseExperience);
        }

        [Fact]
        public async Task GetPokemonListAsync_ReturnsCorrectPokemonList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PokemonService>>();
            var mockHttpClient = new Mock<HttpClient>();

            var numOfPokemon = 3;

            var service = new PokemonService(mockLogger.Object, mockHttpClient.Object);

            // Act
            var result = await service.GetPokemonListAsync(numOfPokemon);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(numOfPokemon, result.Count);
        }

        [Fact]
        public void GetBattleResult_ShouldReturnFirstPokemonWins_WhenFirstPokemonHasTypeAdvantage()
        {
            // Arrange
            var firstPokemon = new PokemonDTOs
            {
                Id = 1,
                Name = "Electricmon",
                Type = "fire",
                BaseExperience = 50
            };

            var secondPokemon = new PokemonDTOs
            {
                Id = 2,
                Name = "Watermon",
                Type = "grass",
                BaseExperience = 40
            };

            // Act
            var result = PokemonService.GetBattleResult(firstPokemon, secondPokemon);

            // Assert
            Assert.Equal(BattleOutcome.Win, result);
        }

        [Fact]
        public void GetBattleResult_ShouldReturnSecondPokemonWins_WhenSecondPokemonHasTypeAdvantage()
        {
            // Arrange
            var firstPokemon = new PokemonDTOs
            {
                Id = 1,
                Name = "Charmander",
                Type = "water",
                BaseExperience = 40
            };

            var secondPokemon = new PokemonDTOs
            {
                Id = 2,
                Name = "Squirtle",
                Type = "electric",
                BaseExperience = 50
            };

            // Act
            var result = PokemonService.GetBattleResult(firstPokemon, secondPokemon);

            // Assert
            Assert.Equal(BattleOutcome.Loss, result);
        }
    }
}
