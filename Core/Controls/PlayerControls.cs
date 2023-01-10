using Microsoft.Xna.Framework.Input;

namespace TheGame.Core
{
    public class PlayerControls
    {
        private Keys _left;
        private Keys _right;

        private Keys _jump;
        private Keys _attack;
        private Keys _transformation;

        public PlayerControls(Keys left, Keys right, Keys jump, Keys attack, Keys transformation)
        {
            _left = left;
            _right = right;
            
            _jump = jump;
            _attack = attack;

            _transformation = transformation;
        }

        public bool IsLeft()
        {
            return Keyboard.GetState().IsKeyDown(_left);
        }
        
        public bool IsRight()
        {
            return Keyboard.GetState().IsKeyDown(_right);
        }
        
        public bool IsJump()
        {
            return Keyboard.GetState().IsKeyDown(_jump);
        }
        
        public bool IsAttack()
        {
            return Keyboard.GetState().IsKeyDown(_attack);
        }

        public bool IsTransform()
        {
            return Keyboard.GetState().IsKeyDown(_transformation);
        }
    }
}