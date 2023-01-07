using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.UI.Components
{
    public class Text : InterfaceComponent
    {
        private SpriteFont _font;

        private int _x, _y;
        
        private string _content;

        private Color _color;

        public Text(Game game, string fontName, int x, int y, string content, Color color)
        {
            _font = game.Content.Load<SpriteFont>($"fonts/{fontName}");

            _x = x;
            _y = y;
            
            _content = content;
            
            _color = color;
        }

        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public string Content
        {
            get => _content;
            set => _content = value;
        }
        
        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public Vector2 Size
        {
            get => _font.MeasureString(_content);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Content, new Vector2(X, Y), Color);
        }
    }
}