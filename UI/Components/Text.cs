using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using TheGame.Manager;
using TheGame.Screen;

namespace TheGame.UI.Components
{
    public class Text : InterfaceComponent
    {
        private SpriteFont _font;

        private string _input;

        private Color _color;

        public Text(MainGame game, ScreenState state, string fontName, int x, int y, string input, Color color)
        {
            ScreenStateManager screenStateManager = game.ScreenStateManager;
            GameScreen gameScreen = screenStateManager.GetScreen(state);
            
            SpriteFont font = gameScreen.Content
                .Load<SpriteFont>($"fonts/{fontName}");

            _font = font;
            
            X = x;
            Y = y;
            
            _input = input;
            
            _color = color;
        }

        public string Input
        {
            get => _input;
            set => _input = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public Vector2 Size
        {
            get => _font.MeasureString(Input);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Input, new Vector2(X, Y), Color);
        }
    }
}