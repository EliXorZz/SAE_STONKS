using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.UI.Components
{
    public class ProgressBar : InterfaceComponent
    {
        private Game _game;
        
        private int _x, _y;
        private int _width, _height;

        private int _outline;

        private float _progress;
        private string _txt;

        private Color _outlineColor;
        private Color _backgroundColor;
        private Color _fillColor;

        public ProgressBar(Game game, int x, int y, int width, int height, int outline, float progress, string txt,
            Color outlineColor, Color backgroundColor, Color fillColor)
        {
            _game = game;
            
            _x = x;
            _y = y;

            _width = width;
            _height = height;

            _outline = outline;
            
            _progress = progress;
            _txt = txt;
            
            _outlineColor = outlineColor;
            _backgroundColor = backgroundColor;
            _fillColor = fillColor;
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
        }

        public int Height
        {
            get => _height;   
        }

        public int Outline
        {
            get => _outline;
            set => _outline = value;
        }

        public string Txt
        {
            get => _txt;
            set => _txt = value;
        }

        public float Progress
        {
            get => _progress;
            set => _progress = value;
        }

        public Color OutlineColor
        {
            get => _outlineColor;
            set => _outlineColor = value;
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public Color FillColor
        {
            get => _fillColor;
            set => _fillColor = value;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            RectangleShape outline = new RectangleShape(_game, X , Y, Width, Height, OutlineColor);
            
            RectangleShape background = 
                new RectangleShape(_game, X + Outline , Y + Outline, Width - Outline * 2, Height - Outline * 2, BackgroundColor);
            
            RectangleShape progress = 
                new RectangleShape(_game, X + Outline , Y + Outline, (int) ((Width - Outline * 2) * Progress), Height - Outline * 2, FillColor);

            Text text = new Text(_game, "font", X, Y, Txt, Color.White);
            
            text.X = (int) (X + Width / 2 - text.Size.X / 2);
            text.Y = (int) (Y + Height / 2 - text.Size.Y / 2);
            
            outline.Draw(spriteBatch);
            background.Draw(spriteBatch);
            progress.Draw(spriteBatch);
            
            text.Draw(spriteBatch);
        }
    }
}