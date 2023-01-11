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

        private bool _attack;
        private float _deadTime;
        private int _healthSave;
        private DateTime _attackDelay;
        private bool _ragdoll;
        private DateTime _ragdollCoolDown;
        private ProgressBar _healthBar;
        private ProgressBar _cooldownBar;
        private DateTime _attend;

        public Monster(MainGame game, string spriteName, Vector2 position, float speed, int health, int damage, int damageCooldown)
            : base(game, spriteName, position, 0.15f, health, damage, damageCooldown, Color.White)
        {
            _game = game;
            _speed = speed;
            Ragdoll = false;
            _attack = false;
            _deadTime = 0;
            _healthSave = health;
            _attackDelay = DateTime.Now;
            Attend = DateTime.Now;

            _healthBar =
                new ProgressBar(_game, ScreenState.InGame, 0, 0 - 8, 50,
                    8, 1, (float) Health / MaxHealth, String.Empty,
                    Color.Transparent, Color.Transparent, Color.DarkRed
                );

            //_cooldownBar =
            //    new ProgressBar(_game, ScreenState.InGame, 0, 0, 20,
            //        4, 1, 1 - (float) (DateTime.Now.Millisecond / TempsAnimation.Millisecond), String.Empty,
            //        Color.Transparent, Color.Transparent, new Color(241, 196, 15)
            //    );
        }

        public Monster(MainGame game, string spriteName, float speed, int health, int damage, int damageCooldown)
            : this(game, spriteName, Vector2.Zero, speed, health, damage, damageCooldown) { }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public bool IsAttack
        {
            get => _attack;
        }
        public bool Ragdoll { get => _ragdoll; set => _ragdoll = value; }
        public DateTime Attend { get => _attend; set => _attend = value; }

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

                Rectangle monsterBounds = GetBounds();
                Vector2 monsterCenter = GetCenter();

                if (target != null)
                {
                    Rectangle targetBounds = target.GetBounds();
                    Vector2 targetCenter = target.GetCenter();

                    if ((int) monsterCenter.X != (int)targetCenter.X && !IsAttack && !Ragdoll)
                    {
                        if (targetCenter.X < monsterCenter.X)
                        {
                            Velocity.X = -Speed;
                            Animation = "run_left";
                        }
                        else if (targetCenter.X > monsterCenter.X)
                        {
                            Velocity.X = Speed;
                            Animation = "run_right";
                        }
                    }
                    else if (!IsAttack && !Ragdoll)
                    {
                        Velocity.X = 0;
                        Animation = "idle";
                    }

                    if (!IsAttack && monsterBounds.Intersects(targetBounds) && !Ragdoll && DateTime.Now >= Attend)
                    {
                        _attack = true;
                        _attackDelay = DateTime.Now.AddMilliseconds(1800);
                        _healthSave = Health;    
                    }

                    if (IsAttack && targetCenter.X < monsterCenter.X)
                        Animation = "attack_left";
                    else if (IsAttack && targetCenter.X > monsterCenter.X)
                        Animation = "attack_right";

                    if (IsAttack && DateTime.Now >= _attackDelay)
                    {
                        _attack = false;
                        Attend = DateTime.Now.AddMilliseconds(3000);

                        if (monsterBounds.Intersects(targetBounds))
                            Attack(gameTime, target);
                    }
                    if(_healthSave > Health && !Ragdoll && !IsAttack)
                    {
                        Ragdoll = true;
                        _healthSave = Health;
                        _ragdollCoolDown = DateTime.Now.AddMilliseconds(1800);


                    }
                    if(DateTime.Now >= _ragdollCoolDown && Ragdoll)
                    {
                        Ragdoll = false;

                    }
                    if (Ragdoll)
                    {
                        Animation = "take_hit";

                    }

                }
                else
                {
                    Velocity.X = 0;
                    Animation = "idle";
                }

                if (IsCollisionMap(map, 0, 1))
                    if (IsCollisionMap(map, -2, 0) || IsCollisionMap(map, 2, 0))
                        Velocity.Y = -3.5f;

                int barOffsetY = -4;

                _healthBar.X = (int) monsterCenter.X - 50 / 2;
                _healthBar.Y = monsterBounds.Y - barOffsetY - 8;
                _healthBar.Progress = (float) Health / MaxHealth;

                //_cooldownBar.X = monsterBounds.X;
                //_cooldownBar.Y = monsterBounds.Y - barOffsetY;
                //_cooldownBar.Progress = 1 - (float) (DateTime.Now.Millisecond / TempsAnimation.Millisecond);

                _healthBar.Update();
                //_cooldownBar.Update();
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
                _healthBar.Draw(spriteBatch);
                //_cooldownBar.Draw(spriteBatch);
            }

            base.Draw(spriteBatch, globalUIBatch);

        }
        public void Attack(GameTime gameTime, Player player)
        {
            float total = (float) gameTime.TotalGameTime.TotalMilliseconds;

            Random _random = new Random();
            int realDamage = _random.Next(1, Damage);

            player.Health -= realDamage;
            player.LastAttack = total;

            player.AddFadeInterfaceComponent(
                200,
                1500,
                new Vector2(0, -3),
                new Text(_game, ScreenState.InGame, "font_small", 0, 0, $"-{realDamage}", Color.Red));
        }

    }
}
