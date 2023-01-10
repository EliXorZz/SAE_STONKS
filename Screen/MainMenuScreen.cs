using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using TheGame.Core;
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
                () =>
                {
                    _game.MapManager.SelectMap("default");

                    _game.MonsterManager.ClearMonsters();
                    _game.PlayerManager.ClearPlayers();

                    _game.PlayerManager.CreatePlayer(_game, new PlayerControls(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Enter), "Joueur 1", Color.DarkCyan);
                    _game.PlayerManager.CreatePlayer(_game, new PlayerControls(Keys.Q, Keys.D, Keys.Z, Keys.S,Keys.E), "Joueur 2", Color.DeepPink);

                    _game.ScreenStateManager.CurrentScreen = ScreenState.InGame;
                }
            );
            
            Button exitButton = new Button(
                _game, ScreenState.MainMenu, "main_button", "Exit",
                width / 2, 450,
                () => _game.Exit()
            );

            Button didact = new Button(
                _game, ScreenState.MainMenu, "little_button", "?", width / 2 + 450, 450, () =>
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

