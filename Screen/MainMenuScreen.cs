using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace TheGame.Screen
{
    public class MainMenuScreen : GameScreen
    {
        private MainGame _game;
        
        public MainMenuScreen(MainGame game) : base(game)
        {
            _game = game;
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}