using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Screen;
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

        private float _deadTime;
        
        private ProgressBar _healthBar;

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

            _deadTime = 0;
            
            int width = 300;
            int height = 40;
            int gap = 10;
            
            _healthBar = new ProgressBar(
                _game, ScreenState.InGame, 20, 20 + Id * (height + gap), width, height, 1,
                (float) Health / MaxHealth,
                $"Player {Pseudo} | {Health}/{MaxHealth}",
                Color.White, new Color(231, 76, 60), new Color(192, 57, 43)
            );
        }

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
            if (!IsDead)
            {
                if (Controls.IsAttack())
                {
                    foreach (Monster monster in _game.MonsterManager.Monsters)
                    {
                        Rectangle playerBounds = GetBounds();
                        Rectangle monsterBounds = monster.GetBounds();

                        if (monsterBounds.Intersects(playerBounds))
                            Attack(monster);
                    }
                }
            
                if (Controls.IsLeft())
                {
                    Velocity.X = -1;
                    if (Controls.IsAttack())
                        Animation = "combat1G";
                    else
                        Animation = "courseG";
                }
                else if (Controls.IsRight())
                {
                    Velocity.X = 1;
                    if (Controls.IsAttack())
                        Animation = "combat1D";
                    else
                        Animation = "courseD";
                    
                }
                else
                {
                    Velocity.X = 0;
                    if (Controls.IsAttack())
                        Animation = "combat1D";
                    else
                        Animation = "idle";
                }

                if (Controls.IsJump() && IsCollisionMap(map, 0, 1))
                    Velocity.Y = -3;

                _healthBar.Progress = (float) Health / MaxHealth;
                _healthBar.Update();
            }
            else
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_deadTime < 800)
                {
                    _deadTime += elapsed;
                    Animation = "death";
                    
                    Sprite.Alpha = _deadTime / 800;
                }
                else
                {
                    _game.PlayerManager.RemovePlayer(this);
                }
            }

            base.Update(gameTime, map);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteBatch globalUIBatch)
        {
            _healthBar.Draw(globalUIBatch);
            base.Draw(spriteBatch, globalUIBatch);
        }
        public void Attack(Monster entity)
        {

            Random _random = new Random();
            int realDamage = _random.Next(1, Damage);

            entity.Health -= realDamage;



            entity.AddFadeInterfaceComponent(
                200,
                1500,
                new Vector2(0, -3),
                new Text(_game, ScreenState.InGame, "font", 0, 0, $"-{realDamage}", Color.Red));



        }
    }
}