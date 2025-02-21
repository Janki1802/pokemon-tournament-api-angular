using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Models;
using PokemonAPI.Services;

namespace PokemonAPI.Controllers
{
    [Route("pokemon/tournament/statistics")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly Dictionary<string, string> sortOptions = new Dictionary<string, string>
        {
            { "wins", "Wins" },
            { "losses", "Losses" },
            { "ties", "Ties" },
            { "name", "Name" },
            { "id", "Id" }
        };

        private readonly PokemonService _pokemonService;

        public PokemonController(PokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonDTOs>>> Get([FromQuery] string sortBy, [FromQuery] string sortDirection = "desc")
        {
            if (string.IsNullOrEmpty(sortBy) || !sortOptions.ContainsKey(sortBy))
                return BadRequest("Invalid sortBy parameter.");

            if (sortDirection != "asc" && sortDirection != "desc")
                return BadRequest("Invalid sortDirection parameter.");

            try
            {
                var pokemonList = await _pokemonService.GetPokemonListAsync(8);

                // Simulate battles and update statistics
                _pokemonService.SimulateTournament(pokemonList);

                var propertyInfo = typeof(PokemonDTOs).GetProperty(sortOptions[sortBy]);

                if (propertyInfo == null)
                {
                    return BadRequest("Invalid sorting property.");
                }

                // Sort based on sortDirection
                var sortedList = sortDirection == "desc"
                    ? pokemonList.OrderByDescending(p => propertyInfo.GetValue(p)).ToList()
                    : pokemonList.OrderBy(p => propertyInfo.GetValue(p)).ToList();

                return Ok(sortedList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
    }

}
