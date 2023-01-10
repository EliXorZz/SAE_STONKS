using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Screen;

namespace TheGame.UI.Components
{
    public class ProgressBar : InterfaceComponent
    {      
        private float _progress;

        private int _width, _height;
        private int _outline;

        private string _input;

        private RectangleShape _outlineShape;
        private RectangleShape _backgroundShape;
        private RectangleShape _progressShape;

        private Text _text;

        public ProgressBar(MainGame game, ScreenState state, int x, int y, int width, int height, int outline, float progress, string input,
            Color outlineColor, Color backgroundColor, Color fillColor)
        {
            X = x;
            Y = y;
            
            _progress = progress;

            _width = width;
            _height = height;
            _outline = outline;

            _input = input;

            _outlineShape = 
                new RectangleShape(game, X, Y, width, height, outlineColor);

            _backgroundShape =
                new RectangleShape(game, X + outline, Y + outline, width - outline * 2, height - outline * 2, backgroundColor);

            _progressShape =
                new RectangleShape(game, X + outline, Y + outline, (int) ((width - outline * 2) * Progress), height - outline * 2, fillColor);

            _text = new Text(game, state, "font", 0, 0, input, Color.White);
        }

        public string Input
        {
            get => _input;
            set => _input = value;
        }

        public float Progress
        {
            get => _progress;
            set => _progress = value;
        }

        public void Update()
        {
            _outlineShape.X = X;
            _outlineShape.Y = Y;

            _backgroundShape.X = X + _outline;
            _backgroundShape.Y = Y + _outline;

            _progressShape.X = X + _outline;
            _progressShape.Y = Y + _outline;

            _text.X = (int)(X + _width / 2 - _text.Size.X / 2);
            _text.Y = (int)(Y + _height / 2 - _text.Size.Y / 2);
            _text.Input = Input;

            _progressShape.Width = (int)((_width - _outline * 2) * Progress);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {   
            _outlineShape.Draw(spriteBatch);
            _backgroundShape.Draw(spriteBatch);
            _progressShape.Draw(spriteBatch);

            _text.Draw(spriteBatch);
        }
    }
}