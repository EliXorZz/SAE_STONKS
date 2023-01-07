using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using TheGame.Manager;
using TheGame.Screen;

namespace TheGame.Core
{
    public class Map
    {
        private string _name;
        
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        public Map(MainGame game, string name)
        {
            _name = name;

            ScreenStateManager screenStateManager = game.ScreenStateManager;
            GameScreen inGameScreen = screenStateManager.GetScreen(ScreenState.InGame);
            
            _tiledMap = inGameScreen.Content
                .Load<TiledMap>($"maps/{name}/map_{name}");
            
            _tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, TiledMap);
        }

        public string Name
        {
            get => _name;
        }

        public TiledMap TiledMap
        {
            get => _tiledMap;
        }

        public TiledMapRenderer TiledMapRenderer
        {
            get => _tiledMapRenderer;
        }

        public TiledMapTile? GetTile(string layer, ushort x, ushort y)
        {
            TiledMapTile? tile;
            TiledMapTileLayer tileLayer = _tiledMap.GetLayer<TiledMapTileLayer>(layer);
            
            tileLayer.TryGetTile(x, y, out tile);

            if (tile.HasValue && !tile.Value.IsBlank)
                return tile;

            return null;
        }

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void Draw(Camera camera)
        {
            _tiledMapRenderer.Draw(camera.Matrix);
        }
    }
}