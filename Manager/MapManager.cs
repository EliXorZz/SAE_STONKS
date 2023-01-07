using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TheGame.Core;

namespace TheGame.Manager
{
    public class MapManager
    {
        private MainGame _game;
        
        private Dictionary<string, Map> _maps;
        private Map _currentMap;

        public MapManager(MainGame game)
        {
            _game = game;
            _maps = new Dictionary<string, Map>();
            
            // Load all maps
            AddMap("default");
        }

        public Dictionary<string, Map>.ValueCollection Maps
        {
            get => _maps.Values;
        }

        public Map CurrentMap
        {
            get => _currentMap;
        }

        public void AddMap(string name)
        {
            Map map = new Map(_game, name);
            _maps.Add(name, map);
        }

        public void RemoveMap(string name)
        {
            _maps.Remove(name);
        }

        public void SelectMap(string name)
        {
            _currentMap = _maps[name];
        }

        public void Update(GameTime gameTime)
        {
            CurrentMap?.Update(gameTime);
        }

        public void Draw(Camera camera)
        {
            CurrentMap?.Draw(camera);
        }
    }
}