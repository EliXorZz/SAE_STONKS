using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using TheGame.Core;
using TheGame.UI.Components;

namespace TheGame.Screen
{
    public class MapSelectorScreen : GameScreen
    {
        private MainGame _game;
        private SpriteBatch _spriteBatch;
        
        private Button[] _buttons;
        private Text _text;

        private Texture2D _background;
        
        public MapSelectorScreen(MainGame game) : base(game)
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
                _game, ScreenState.MainMenu, "main_button", "Valider",
                width / 2, 100,
                () =>
                {
                    _game.MonsterManager.ClearMonsters();
                    _game.PlayerManager.ClearPlayers();

                    _game.PlayerManager.CreatePlayer(_game, new PlayerControls(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Enter), "Joueur 1", Color.DarkCyan);
                    _game.PlayerManager.CreatePlayer(_game, new PlayerControls(Keys.Q, Keys.D, Keys.Z, Keys.S, Keys.E), "Joueur 2", Color.DeepPink);

                    _game.ScreenStateManager.CurrentScreen = ScreenState.InGame;
                }
            );

            Button before = new Button(
                _game, ScreenState.MainMenu, "little_button", "<", width / 2 - 150, 350,
                () => _game.MapManager.Before());


            Button next = new Button(
                _game, ScreenState.MainMenu, "little_button", ">", width / 2 + 150, 350,
                () => _game.MapManager.Next());

            _buttons = new Button[]{ playButton, before, next};

            _text = new Text(_game, ScreenState.MapSelector, "menu_font", 0, 570, string.Empty, Color.Black);

            _background = _game.Content.Load<Texture2D>("ui/main_background");
        }
        
        public override void Update(GameTime gameTime)
        {
            int width = GraphicsDevice.Viewport.Width;

            foreach (Button button in _buttons)
                button.Update(gameTime);

            string mapName = _game.MapManager.CurrentMap.Name;

            _text.Input = mapName;
            _text.X = (int)(width / 2 - _text.Size.X / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            
            foreach (Button button in _buttons)
                button.Draw(_spriteBatch);

            _text.Draw(_spriteBatch);
            
            _spriteBatch.End();
        }
    }
}

