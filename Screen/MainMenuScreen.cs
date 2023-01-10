using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using TheGame.UI.Components;

namespace TheGame.Screen
{
    public class MainMenuScreen : GameScreen
    {
        private MainGame _game;
        private SpriteBatch _spriteBatch;
        
        private Button[] _buttons;
        private Texture2D _background;
        
        public MainMenuScreen(MainGame game) : base(game)
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

            Button playButton = new Button(
                _game, ScreenState.MainMenu, "main_button", "Start",
                width / 2, 200,
                () => _game.ScreenStateManager.CurrentScreen = ScreenState.InGame
            );
            
            Button exitButton = new Button(
                _game, ScreenState.MainMenu, "main_button", "Exit",
                width / 2, 450,
                () => _game.Exit()
            );

            Button didact = new Button(
                _game, ScreenState.MainMenu, "little_button", "?", width / 2 + 450, 200, () =>
                {
                    _game.ScreenStateManager.CurrentScreen = ScreenState.DidactMenu;
                });
            _buttons = new Button[]{ playButton, exitButton, didact};
            _background = _game.Content.Load<Texture2D>("ui/main_background");
        }
        
        public override void Update(GameTime gameTime)
        {
            foreach (Button button in _buttons)
                button.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            
            foreach (Button button in _buttons)
                button.Draw(_spriteBatch);
            
            _spriteBatch.End();
        }
    }
}