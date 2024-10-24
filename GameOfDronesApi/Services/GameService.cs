using GameOfDronesApi.Enums;
using GameOfDronesApi.Models;

namespace GameOfDronesApi.Services
{
    public class GameService
    {
        private readonly int _winningScore = 3; // condición que determina quien ganó el juego

        public string DetermineRoundWinner(Dictionary<int, Moves> playerMoves)
        { 
            var result = new Dictionary<int, int>();
            foreach (var currentPlayer in playerMoves)
            {
                int score = 0;
                foreach (var opponent in playerMoves.Where(p=> p.Key != currentPlayer.Key))
                {
                    if (Beats(currentPlayer.Value, opponent.Value))
                    {
                        score++;
                    }
                }
                result[currentPlayer.Key] = score;
            }
            var winningPlayers = result.Where(r => r.Value == playerMoves.Count -1).ToList();
            if (winningPlayers.Count == 1)
            {
                return $"Jugador {winningPlayers.First().Key} gana la ronda";
            }
            return "Empate en la ronda";
        }

        public bool Beats(Moves move1, Moves move2)
        { 
            return 
                (move1 == Moves.Rock && move2 == Moves.Scissors) ||
                (move1 == Moves.Paper && move2 == Moves.Rock) ||
                (move1 == Moves.Scissors && move2 == Moves.Paper);
        }

        public bool CheckForGameWinner(Game game)
        {
            return game.PlayerScores.Values.Any(score => score >= _winningScore);
        }
    }
}
