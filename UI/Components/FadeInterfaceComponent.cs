using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Core;

namespace TheGame.UI.Components
{
    public class FadeInterfaceComponent
    {
        private float _delay;
        private float _time;
        
        private Entity _entity;
        private Vector2 _offset;
        private InterfaceComponent _component;
    
        public FadeInterfaceComponent(float delay, float time, Entity entity, Vector2 offset, InterfaceComponent component)
        {
            _delay = delay;
            _time = time;
            
            _entity = entity;
            _offset = offset;
            _component = component;
        }

        public bool Update(GameTime gameTime)
        {
            float elapsed = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            _delay = Math.Max(0, _delay - elapsed);

            if (_delay <= 0)
            {
                Vector2 center = _entity.GetCenter();

                _time = Math.Max(0, _time - elapsed);

                if (_time <= 0)
                    return true;

                _component.X = (int) (center.X + _offset.X);
                _component.Y = (int) (center.Y + _offset.Y);

                _offset.Y -= 0.3f;
            }

            return false;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_delay <= 0)
                _component.Draw(spriteBatch);
        }
    }
}