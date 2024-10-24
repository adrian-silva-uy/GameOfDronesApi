namespace GameOfDronesApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public List<Player> Players { get; set; } = new List<Player>(); // soporte para múltiples jugadores
        public Dictionary<int, int> PlayerScores { get; set; } = new();
        public bool IsFinished { get; set; }
    }
}
