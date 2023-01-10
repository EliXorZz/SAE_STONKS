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

        private float _deadTime;

        private ProgressBar _healthBar;
        private ProgressBar _cooldownBar;

        public Monster(MainGame game, string spriteName, Vector2 position, float speed, int health, int damage, int damageCooldown)
            : base(game, spriteName, position, 0.15f, health, damage, damageCooldown)
        {
            _game = game;
            _speed = speed;

            _deadTime = 0;

            Rectangle bounds = GetBounds();

            _healthBar =
                new ProgressBar(_game, ScreenState.InGame, 0, 0 - 8, bounds.Width,
                    8, 1, (float)Health / MaxHealth, String.Empty,
                    Color.Transparent, Color.Transparent, new Color(46, 204, 113)
                );

            _cooldownBar =
                new ProgressBar(_game, ScreenState.InGame, 0, 0, bounds.Width,
                    4, 1, -(CurrentDamageCooldown / DamageCooldown) + 1, String.Empty,
                    Color.Transparent, Color.Transparent, new Color(241, 196, 15)
                );
        }

        public Monster(MainGame game, string spriteName, float speed, int health, int damage, int damageCooldown)
            : this(game, spriteName, Vector2.Zero, speed, health, damage, damageCooldown) {}

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

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

                if (target != null)
                {
                    Rectangle targetBounds = target.GetBounds();

                    Vector2 targetCenter = target.GetCenter();
                    Vector2 center = GetCenter();

                    if ((int)center.X != (int)targetCenter.X)
                    {
                        if (targetCenter.X < center.X)
                        {
                            Velocity.X = -Speed;
                            Animation = "run_left";
                        }
                        else if (targetCenter.X > center.X)
                        {
                            Velocity.X = Speed;
                            Animation = "run_right";
                        }
                    }
                    else
                    {
                        Velocity.X = 0;
                        Animation = "idle";
                    }
                    
                    if (monsterBounds.Intersects(targetBounds))
                    {
                        if (Attack(target))
                            if (center.X < targetCenter.X)
                                Animation = "attack_right";
                            else
                                Animation = "attack_left";
                    }
                }
                else
                {
                    Velocity.X = 0;
                    Animation = "idle";
                }

                if (IsCollisionMap(map, 1, 0))
                    if (IsCollisionMap(map, -1, 0) || IsCollisionMap(map, 1, 0))
                        Velocity.Y = -3.5f;

                int barOffsetY = -4;

                _healthBar.X = monsterBounds.X;
                _healthBar.Y = monsterBounds.Y - barOffsetY - 8;
                _healthBar.Progress = (float) Health / MaxHealth;

                _cooldownBar.X = monsterBounds.X;
                _cooldownBar.Y = monsterBounds.Y - barOffsetY;
                _cooldownBar.Progress = 1 - (float) CurrentDamageCooldown / DamageCooldown;

                _healthBar.Update();
                _cooldownBar.Update();
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
                _cooldownBar.Draw(spriteBatch);
            }
            
            base.Draw(spriteBatch, globalUIBatch);
        }
    }
}
