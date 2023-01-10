using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using TheGame.UI.Components;

namespace TheGame.Screen
{
    public class PauseMenuScreen : GameScreen
    {
        private MainGame _game;
        private SpriteBatch _spriteBatch;
        
        private Button[] _buttons;
        
        public PauseMenuScreen(MainGame game) : base(game)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }
        
        public override void Initialize()
        {
            _game.IsMouseVisible = true;
        }
        
        public override void LoadContent()
        {
            int width = GraphicsDevice.Viewport.Width;

            Button exitButton = new Button(
                _game, ScreenState.MainMenu, "main_button", "Menu",
                width / 2, 240,
                () => _game.ScreenStateManager.CurrentScreen = ScreenState.MainMenu
            );
            
            _buttons = new Button[]{ exitButton };
        }
        
        public override void Update(GameTime gameTime)
        {
            foreach (Button button in _buttons)
                button.Update(gameTime);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ScreenStateManager.CurrentScreen = ScreenState.InGame;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
            foreach (Button button in _buttons)
                button.Draw(_spriteBatch);
            
            _spriteBatch.End();
        }
    }
}