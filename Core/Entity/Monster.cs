using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Screen;
using TheGame.UI.Components;

namespace TheGame.Core
{
    public abstract class Monster : Entity
    {
        private MainGame _game;
        private float _speed;
        bool _attack;
        private float _deadTime;
        private DateTime _tempsAnimation;
        private int _sens;
        private bool _ragdle;
        private DateTime _ragdleCoolDown;
        private int _healthCurrent;

        public Monster(MainGame game, string spriteName, Vector2 position, float speed, int health, int damage, int damageCooldown)
            : base(game, spriteName, position, 0.15f, health, damage, damageCooldown)
        {
            _game = game;
            _speed = speed;
            _attack = false;
            _deadTime = 0;
            this.HealthCurrent = health;
        }

        public Monster(MainGame game, string spriteName, float speed, int health, int damage, int damageCooldown)
            : this(game, spriteName, Vector2.Zero, speed, health, damage, damageCooldown) { }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public bool Attack1
        {
            get
            {
                return _attack;
            }

            set
            {
                _attack = value;
            }
        }

        public DateTime TempsAnimation
        {
            get
            {
                return _tempsAnimation;
            }

            set
            {
                _tempsAnimation = value;
            }
        }

        public int Sens
        {
            get
            {
                return _sens;
            }

            set
            {
                _sens = value;
            }
        }

        public bool Ragdle { get => _ragdle; set => _ragdle = value; }
        public DateTime RagdleCoolDown { get => _ragdleCoolDown; set => _ragdleCoolDown = value; }
        public int HealthCurrent { get => _healthCurrent; set => _healthCurrent = value; }

        public override void Update(GameTime gameTime, Map map)
        {
            if (!IsDead)
            {
                float distance = float.MaxValue;
                Player target = null;

                foreach (Player player in _game.PlayerManager.Players)
                {
                    if (GetDistanceBetweenEntity(player) < distance)
                    {
                        distance = GetDistanceBetweenEntity(player);
                        target = player;
                    }
                }

                if (target != null)
                {

                    Rectangle monsterBounds = GetBounds();
                    Rectangle targetBounds = target.GetBounds();

                    Vector2 targetCenter = target.GetCenter();
                    Vector2 center = GetCenter();
                    if(HealthCurrent > Health)
                    {
                        HealthCurrent = Health;
                        RagdleCoolDown = DateTime.Now.AddMilliseconds(1400);
                        Ragdle = true
                    }

                    if ((int)center.X != (int)targetCenter.X && !Attack1 && !Ragdle)
                    {
                        if (targetCenter.X < center.X)
                        {
                            Velocity.X = -Speed;
                            Animation = "run_left";
                            Sens = -1;
                        }
                        else if (targetCenter.X > center.X)
                        {
                            Velocity.X = Speed;
                            Animation = "run_right";
                            Sens = 1;
                        }
                    }
                    else if (!Ragdle && !Attack1)
                    {
                        Velocity.X = 0;
                        Animation = "idle";
                    }

                    if (monsterBounds.Intersects(targetBounds))
                    {
                         
                        if (!this.Attack1 && !Ragdle)
                        {
                            this.Attack1 = true;
                            this.TempsAnimation = DateTime.Now.AddMilliseconds(1000);

                            

                        }
                        
                        

                    }
                    if (Sens == 1 && Attack1)
                        Animation = "attack_right";
                    else if (Attack1 && Sens == -1)
                        Animation = "attack_left";
                    if (DateTime.Now >= this.TempsAnimation && Attack1)
                    {
                        this.Attack1 = false;
                        if (monsterBounds.Intersects(targetBounds))
                        {
                            Attack(target);
                        }
                    }
                    if (Ragdle)
                    {
                        Animation = "take_hit";
                    }
                    if(DateTime.Now >= this.RagdleCoolDown && Ragdle)
                    {
                        Ragdle = false;

                    }


                }
                

                if (IsCollisionMap(map, 0, 1))
                    if (IsCollisionMap(map, -1, 0) || IsCollisionMap(map, 1, 0))
                        Velocity.Y = -2.5f;
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
                    _game.MonsterManager.RemoveMonster(this);
                }
            }

            base.Update(gameTime, map);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteBatch globalUIBatch)
        {
            if (!IsDead)
            {
                Rectangle bounds = GetBounds();
                int barOffsetY = -4;

                ProgressBar healthBar =
                    new ProgressBar(_game, ScreenState.InGame, bounds.X, bounds.Y - barOffsetY - 8, bounds.Width,
                        8, 1, (float)Health / MaxHealth, String.Empty,
                        Color.Transparent, Color.Transparent, new Color(46, 204, 113)
                    );

                /*ProgressBar cooldownBar =
                    new ProgressBar(_game, ScreenState.InGame, bounds.X, bounds.Y - barOffsetY, bounds.Width,
                        4, 1, -(CurrentDamageCooldown / DamageCooldown) + 1, String.Empty,
                        Color.Transparent, Color.Transparent, new Color(241, 196, 15)
                    );*/

                healthBar.Draw(spriteBatch);
                //cooldownBar.Draw(spriteBatch);
            }

            base.Draw(spriteBatch, globalUIBatch);

        }
        public void Attack(Player entity)
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
