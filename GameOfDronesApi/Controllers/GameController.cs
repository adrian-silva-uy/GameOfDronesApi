using GameOfDronesApi.Data;
using GameOfDronesApi.Enums;
using GameOfDronesApi.Models;
using GameOfDronesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOfDronesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly GameContext _context;

        public GameController(GameService gameService, GameContext gameContext)
        {
            _gameService = gameService;
            _context = gameContext;
        }

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] List<Player> players)
        {
            if (players.Count < 2)
                return BadRequest("Se requiere exactamente 2 jugadores");

            var game = new Game
            {
                Players = players
            };

            foreach (var player in players)
            {
                game.PlayerScores[player.Id] = 0;
            }

            _context.Games.Add(game);
            _context.SaveChanges();
            return Ok(game);

        }

        [HttpPost("move")]
        public IActionResult MakeMove([FromBody] dynamic payload)
        {
            int gameId = payload.GameId;
            var playerMoves = ((IDictionary<string, object>)payload.PlayerMoves)
                .ToDictionary(p => int.Parse(p.Key), p => (Moves)Enum.Parse(typeof(Moves), (string)p.Value));

            var game = _context.Games.Find(gameId);
            if (game == null || game.IsFinished)
                return BadRequest("Juego no encontrado o ya terminado");

            string result = _gameService.DetermineRoundWinner(playerMoves);

            foreach (var playerMove in playerMoves)
            {
                if (_gameService.Beats(playerMove.Value, playerMoves.Values.First()))
                    game.PlayerScores[playerMove.Key]++;
            }

            if (_gameService.CheckForGameWinner(game))
                game.IsFinished = true;

            _context.SaveChanges();
            return Ok(new { result, game.PlayerScores });
        }

    }
}
