using System;
using Microsoft.Xna.Framework;
using TheGame.Core;

namespace TheGame.Manager
{
    public class MapManager
    {
        private MainGame _game;
        
        private Map[] _maps;

        private int _mapIndex;
        private Map _currentMap;

        public MapManager(MainGame game)
        {
            _game = game;

            _maps = new Map[]
            {
                new Map(_game, "default"),
                new Map(_game, "new")
            };

            _mapIndex = 0;
            _currentMap = _maps[_mapIndex];
        }

        public Map[] Maps
        {
            get => _maps;
        }

        public Map CurrentMap
        {
            get => _currentMap;
        }

        public void SelectMap(int index)
        {
            _mapIndex = index;
            _currentMap = _maps[index];
        }

        public void Before()
        {
            SelectMap(Math.Max(0, _mapIndex - 1));
        }

        public void Next()
        {
            SelectMap(Math.Min(_maps.Length - 1, _mapIndex + 1));
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