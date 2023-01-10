using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TheGame.Core;

namespace TheGame.Manager
{
    public class PlayerManager
    {
        private EntityManager _entityManager;
        private List<Player> _players;

        public PlayerManager(EntityManager entityManager)
        {
            _entityManager = entityManager;
            _players = new List<Player>();
        }
        
        public List<Player> Players
        {
            get => _players;
        }

        public void CreatePlayer(MainGame game, PlayerControls controls, string pseudo, Color color)
        {
            Player player = new Player(game, controls, GetNextId(), pseudo, Vector2.Zero, color);

            _entityManager.RandomPosition(player);
            
            _entityManager.CreateEntity(player);
            _players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            _players.Remove(player);
            _entityManager.RemoveEntity(player);
        }
        
        public List<Player> GetNearbyPlayers(Entity source, float distance)
        {
            List<Player> list = new List<Player>();

            foreach (Player player in Players)
                if (source.GetDistanceBetweenEntity(player) <= distance)
                    list.Add(player);

            return list;
        }

        public void FollowCamera(Camera camera)
        {
            Vector2 vector = new Vector2(0, 0);
            int count = 0;

            foreach (var player in Players)
            {
                vector += player.GetCenter();
                count++;
            }

            if (count > 0)
                camera.Target(vector / count);
        }
        
        private int GetNextId()
        {
            int id = 0;

            while (true)
            {
                bool found = false;
                
                foreach (Player player in Players)
                {
                    if (player.Id == id)
                    {
                        found = true;
                        id++;
                        
                        break;
                    }
                }

                if (!found)
                    return id;
            }
        }
    }
}