using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TheGame.Core
{
    public class Camera
    {
        private int _scale;
        private Matrix _matrix; 
        
        public Camera(int scale)
        {
            _scale = scale;
            Target(Vector2.Zero);
        }

        public int Scale
        {
            get => _scale;
            set => _scale = value;
        }
        
        public Matrix Matrix
        {
            get => _matrix;
        }

        public void Target(Vector2 position)
        {
            Matrix translationX = Matrix.CreateTranslation(0, 0, 0);
            Matrix translationY = Matrix.CreateTranslation(0, 0, 0);
            
            if (position.X > (float) 1280 / (2 * Scale) || position.X < (float) 1280 / (2 * Scale))
                translationX = Matrix.CreateTranslation(position.X - (float) 1280 / (2 * Scale), 0, 0);

            if (position.Y > (float) 720 / (2 * Scale) || position.Y < (float) 720 / (2 * Scale))
                translationY = Matrix.CreateTranslation(0, position.Y - (float) 720 / (2 * Scale), 0);

            _matrix =
                Matrix.CreateTranslation(-position.X, -position.Y, 0) * 
                Matrix.CreateTranslation((float) 1280 / (2 * Scale), (float) 720 / (2 * Scale), 0) * 
                Matrix.CreateScale(Scale, Scale, 1) *
                translationX * translationY;
        }
    }
}