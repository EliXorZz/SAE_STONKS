using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Screen;

namespace TheGame.UI.Components
{
    public class ProgressBar : InterfaceComponent
    {
        private MainGame _game;
        private ScreenState _state;
        
        private int _width, _height;

        private int _outline;

        private float _progress;

        private Color _outlineColor;
        private Color _backgroundColor;
        private Color _fillColor;
        
        private Text _text;

        public ProgressBar(MainGame game, ScreenState state, int x, int y, int width, int height, int outline, float progress, string input,
            Color outlineColor, Color backgroundColor, Color fillColor)
        {
            _game = game;
            _state = state;

            _width = width;
            _height = height;
            
            X = x;
            Y = y;

            _outline = outline;
            
            _progress = progress;

            _outlineColor = outlineColor;
            _backgroundColor = backgroundColor;
            _fillColor = fillColor;
            
            UpdateText(input);
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

        public void UpdateText(string input)
        {
            _text = new Text(_game, _state, "font", 0, 0, input, Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            RectangleShape outline = new RectangleShape(_game, X , Y, Width, Height, OutlineColor);
            
            RectangleShape background = 
                new RectangleShape(_game, X + Outline , Y + Outline, Width - Outline * 2, Height - Outline * 2, BackgroundColor);
            
            RectangleShape progress = 
                new RectangleShape(_game, X + Outline , Y + Outline, (int) ((Width - Outline * 2) * Progress), Height - Outline * 2, FillColor);

            _text.X = (int) (X + Width / 2 - _text.Size.X / 2);
            _text.Y = (int) (Y + Height / 2 - _text.Size.Y / 2);
            
            outline.Draw(spriteBatch);
            background.Draw(spriteBatch);
            progress.Draw(spriteBatch);
            
            _text.Draw(spriteBatch);
        }
    }
}