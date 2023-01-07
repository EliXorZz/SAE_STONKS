using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.UI.Components;

namespace TheGame.Core
{
    public class Player : Entity
    {
        private MainGame _game;
        private PlayerControls _controls;

        private int _id;
        private string _pseudo;

        private bool _swordMode;
        private int _swordDamage;

        private int _experience;
        private int _level;

        public Player(MainGame game, PlayerControls controls, int id, string pseudo, Vector2 position)
            : base(game, "blue_character", position, 0.05f, 100, 5, 2000)
        {
            _game = game;
            _controls = controls;

            _id = id;
            _pseudo = pseudo;

            _swordMode = false;
            _swordDamage = 10;

            _experience = 0;
            _level = 0;
        }
        
        public Player(MainGame game, PlayerControls controls, string pseudo)
            : this(game, controls, -1, pseudo, Vector2.Zero) {}

        public PlayerControls Controls
        {
            get => _controls;
        }
        
        public int Id
        {
            get => _id;
            set => _id = value;
        } 

        public string Pseudo
        {
            get => _pseudo;
        }

        public bool SwordMode
        {
            get => _swordMode;
            set => _swordMode = value;
        }

        public int SwordDamage
        {
            get => _swordDamage;
            set
            {
                if (value < 0) 
                    throw new ArgumentOutOfRangeException(nameof(value), "Sword damage must be positive or null");
                
                _swordDamage = value;
            }
        }

        public int Experience
        {
            get => _experience;     
        }

        public int Level
        {
            get => _level;   
        }
        
        public override void Update(GameTime gameTime, Map map)
        {
            if (Controls.IsLeft())
            {
                Velocity.X = -1;
                Animation = "courseG";
            }
            else if (Controls.IsRight())
            {
                Velocity.X = 1;
                Animation = "courseD";
            }
            else
            {
                Velocity.X = 0;
                Animation = "idle";
            }

            if (Controls.IsJump() && IsCollisionMap(map, 1))
                Velocity.Y = -3;

            base.Update(gameTime, map);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteBatch globalUIBatch)
        {
            int width = 300;
            int height = 40;

            int gap = 10;
            
            ProgressBar progressBar =
                new ProgressBar(_game, 20, 20 + Id * (height + gap), width, height, 1,
                    (float) Health / MaxHealth,
                    $"Player {Id} - {Pseudo} - {Health}/{MaxHealth}",
                    Color.White, new Color(231, 76, 60), new Color(192, 57, 43)
                );

            progressBar.Draw(globalUIBatch);   
            
            base.Draw(spriteBatch, globalUIBatch);
        }
    }
}