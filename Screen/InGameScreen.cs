using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using TheGame.Core;

namespace TheGame.Screen
{
    public class InGameScreen : GameScreen
    {
        private MainGame _game;

        private SpriteBatch _mainSpriteBatch;
        private SpriteBatch _uiSpriteBatch;

        private Camera _camera;

        public InGameScreen(MainGame game) : base(game)
        {
            _game = game;

            _mainSpriteBatch = new SpriteBatch(game.GraphicsDevice);
            _uiSpriteBatch = new SpriteBatch(game.GraphicsDevice);
            
            _camera = new Camera(1);
        }

        public override void LoadContent()
        {
            Player player = new Player(_game, new PlayerControls(Keys.Left, Keys.Right, Keys.Up), "Joueur 1");
            _game.PlayerManager.CreatePlayer(player);

            Goblin goblin = new Goblin(_game);
            _game.MonsterManager.CreateMonster(goblin);
        }

        public override void Update(GameTime gameTime)
        {
            _game.MapManager.Update(gameTime);
            _game.EntityManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _uiSpriteBatch.Begin();
            _mainSpriteBatch.Begin(transformMatrix: _camera.Matrix);
            
            _game.MapManager.Draw(_camera);
            _game.EntityManager.Draw(_mainSpriteBatch, _uiSpriteBatch);
            
            _mainSpriteBatch.End();
            _uiSpriteBatch.End();
        }
    }
}