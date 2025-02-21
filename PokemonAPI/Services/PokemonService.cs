using Newtonsoft.Json;
using PokemonAPI.Models;

namespace PokemonAPI.Services
{
    // This class handles the logic for fetching Pokemon data from the API
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<PokemonService> _logger;

        private static readonly Dictionary<string, string> fightingRulesMap = new Dictionary<string, string> {
            {"water", "fire"},
            {"fire", "grass"},
            {"grass", "electric"},
            {"electric", "water"},
            {"ghost", "psychic"},
            {"psychic","fighting"},
            {"fighting","dark"},
            {"dark", "ghost"}
        };

        // Constructor for initializing HttpClient and ILogger
        public PokemonService(ILogger<PokemonService> logger, HttpClient httpClient) {

            _logger = logger;
            _httpClient = httpClient;
        }


        // Simulates a tournament between a list of Pokémon
        public void SimulateTournament(List<PokemonDTOs> pokemonList)
        {
            for (int i = 0; i < pokemonList.Count - 1; i++)
            {
                for (int j = i + 1; j < pokemonList.Count; j++)
                {
                    UpdateBattleScore(pokemonList[i], pokemonList[j]);
                }
            }
        }

        // Updates the battle score for two Pokémon
        private void UpdateBattleScore(PokemonDTOs firstPokemon, PokemonDTOs secondPokemon)
        {
            var battleOutcome = GetBattleResult(firstPokemon, secondPokemon);
            switch (battleOutcome)
            {
                case BattleOutcome.Win:
                    firstPokemon.Wins++;
                    secondPokemon.Losses++;
                    break;
                case BattleOutcome.Loss:
                    secondPokemon.Wins++;
                    firstPokemon.Losses++;
                    break;
                case BattleOutcome.Tie:
                    firstPokemon.Ties++;
                    secondPokemon.Ties++;
                    break;
            }
        }

        // Determines the battle outcome between two Pokémon
        public static BattleOutcome GetBattleResult(PokemonDTOs firstPokemon, PokemonDTOs secondPokemon)
        {
            // Check type-based outcome first
            if (fightingRulesMap.ContainsKey(firstPokemon.Type) && fightingRulesMap[firstPokemon.Type] == secondPokemon.Type)
            {
                return BattleOutcome.Win;
            }

            if (fightingRulesMap.ContainsKey(secondPokemon.Type) && fightingRulesMap[secondPokemon.Type] == firstPokemon.Type)
            {
                return BattleOutcome.Loss;
            }

            // Compare base experience if no type advantage
            if (firstPokemon.BaseExperience > secondPokemon.BaseExperience)
            {
                return BattleOutcome.Win;
            }
            else if (firstPokemon.BaseExperience < secondPokemon.BaseExperience)
            {
                return BattleOutcome.Loss;
            }
            else
            {
                return BattleOutcome.Tie;
            }

        }

        public async Task<List<PokemonDTOs>> GetPokemonListAsync(int numOfPokemon)
        {
            var pokemonList = new List<PokemonDTOs>();
            var random = new Random();
            HashSet<int> pokemonIds = new HashSet<int>();
            int tries = 1;
            int maxRetries = 10;

            _logger.LogInformation($"Starting to fetch {numOfPokemon} Pokémon details...");

            while (pokemonList.Count < numOfPokemon && pokemonIds.Count < 151 && tries <= maxRetries)
            {
                int randomId = random.Next(1, 152); // Generate a random Pokemon ID between 1 and 151

                // Check if the Pokemon ID has already been added
                if (!pokemonIds.Contains(randomId))
                {
                    try
                    {
                        PokemonDTOs pokemonDetail = await GetPokemonAsync(randomId);
                        pokemonList.Add(pokemonDetail);
                        pokemonIds.Add(randomId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to fetch Pokémon with ID: {randomId}, Error: {ex.Message}");

                        // Increment the retry counter
                        tries++;
                    }
                }
            }

            // If the method exceeded the maximum retry attempts, throw an exception
            if (tries > maxRetries)
            {
                throw new Exception($"Failed to fetch Pokemon IDs: {pokemonIds}. Please try again.");
            }

            _logger.LogInformation($"Successfully fetched {pokemonList.Count} Pokemon details.");

            return pokemonList;
        }



        // Asynchronous method to fetch a Pokemon's details based on its ID
        public async Task<PokemonDTOs> GetPokemonAsync(int pokemonId)
        {
            _logger.LogInformation($"Fetching details for Pokémon with ID: {pokemonId}");

            try
            {
                // use URL End Point to  get data using id
                string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonId}";

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                httpResponse.EnsureSuccessStatusCode();

                // Read the response body as a string
                string responsebody = await httpResponse.Content.ReadAsStringAsync();

                // Deserialize the response body into a dynamic object for easy access
                dynamic? jsonObject = JsonConvert.DeserializeObject(responsebody);

                var result = new PokemonDTOs
                {
                    Id = pokemonId,
                    Name = jsonObject?.forms[0].name.ToString() ?? "",
                    Type = jsonObject?.types[0].type.name.ToString() ?? "",
                    BaseExperience = jsonObject?.base_experience ??  0,
                };

                // Log success
                _logger.LogInformation($"Successfully fetched Pokémon details for ID: {pokemonId}");

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Load data {ex.Message}");
                throw;
            }
        }

    }
}
