using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using TheGame.Manager;
using TheGame.Screen;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace TheGame.UI.Components
{
    public class Button : InterfaceComponent
    {
        private MainGame _game;
        private AnimatedSprite _sprite;

        private int _width, _height;
        private Action _action;
        
        private Text _text;

        private float _delay;

        public Button(MainGame game, ScreenState state, string button, string input, int x, int y, Action action)
        {
            _game = game;

            ScreenStateManager screenStateManager = game.ScreenStateManager;
            GameScreen gameScreen = screenStateManager.GetScreen(state);
            
            SpriteSheet spriteSheet = gameScreen.Content
                .Load<SpriteSheet>($"ui/buttons/{button}.sf", new JsonContentLoader());
            
            AnimatedSprite sprite = new AnimatedSprite(spriteSheet);
            
            _sprite = sprite;
            _sprite.Origin = Vector2.Zero;

            _width = _sprite.TextureRegion.Width;
            _height = _sprite.TextureRegion.Height;

            X = x - Width / 2;
            Y = y;
            
            _action = action;

            _text = new Text(game, state, "menu_font", 0, 0, input, Color.Black);
            
            _text.X = (int) (X + Width / 2 - _text.Size.X / 2);
            _text.Y = (int) (Y + Height / 2 - _text.Size.Y / 2);

            _delay = 0;
        }

        public int Width
        {
            get => _width;
        }

        public int Height
        {
            get => _height;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        public bool IsHovered()
        {
            Point mousePosition = Mouse.GetState().Position;
            return GetBounds().Contains(mousePosition);
        }
        
        public void Update(GameTime gameTime)
        {
            bool hovered = IsHovered();

            float elapsed = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            _delay += elapsed;

            if (hovered && Mouse.GetState().LeftButton == ButtonState.Pressed) _game.SoundManager.PlayEffect("click", gameTime);

            if (hovered)
            {
                
                _sprite.Play("true");
                _text.Y = (int) (Y + Height / 2 - _text.Size.Y / 2) + 5;
            }
            else
            {
                _sprite.Play("false");
                _text.Y = (int) (Y + Height / 2 - _text.Size.Y / 2) - 5;
            }

            if (hovered && _delay >= 300 && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                
                _delay = 0;
                _action();
            }
            
            _sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Vector2(X, Y));
            _text.Draw(spriteBatch);
        }
    }
}