namespace PokemonAPI.Models
{

    // This class is used to store data related to a pokemon
    public class PokemonDTOs
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required string Type {  get; set; }

        public  int BaseExperience { get; set; }

        public int Wins { get; set; } = 0;

        public int Losses { get; set; } = 0;

        public int Ties { get; set; } = 0;
    }
}
