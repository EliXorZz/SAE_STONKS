using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.UI.Components
{
    public class RectangleShape : InterfaceComponent
    {
        private Texture2D _texture;

        private int _x, _y;
        private int _width, _height;

        private Color _color;
        
        public RectangleShape(Game game, int x, int y, int width, int height, Color color)
        {
            _texture = new Texture2D(game.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White });
            
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            
            _color = color;
        }

        public Texture2D Texture
        {
            get => _texture;
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

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new Rectangle(X, Y, Width, Height);
            
            spriteBatch.Draw(_texture, rectangle, Color);
        }
    }
}