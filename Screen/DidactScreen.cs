using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.UI.Components;

namespace TheGame.Screen
{
    internal class DidactScreen : GameScreen
    {
        MainGame _game;
        Texture2D _background;
        SpriteBatch _spriteBatch;
        private Button[] _buttons;

        public DidactScreen(MainGame game) : base(game)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            foreach (Button button in _buttons)
                button.Draw(_spriteBatch);
            
            _spriteBatch.End();
        }

        public override void LoadContent()
        {
            int width = GraphicsDevice.Viewport.Width;

            Button didact = new Button(
                _game, ScreenState.MainMenu, "little_button", "X", width-120, 20, () =>
                {
                    _game.ScreenStateManager.CurrentScreen = ScreenState.MainMenu;
                });
            _buttons = new Button[] { didact };

            _background = _game.Content.Load<Texture2D>("ui/didact");
        }

        public override void Update(GameTime gameTime)
        {
            
            foreach (Button button in _buttons)
                button.Update(gameTime);
        }
    }
}
