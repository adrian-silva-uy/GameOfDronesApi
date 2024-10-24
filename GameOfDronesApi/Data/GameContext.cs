using GameOfDronesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfDronesApi.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {

        }
    }
}
