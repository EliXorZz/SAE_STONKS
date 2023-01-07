using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Manager;
using TheGame.UI.Components;

namespace TheGame.Core
{
    public abstract class Monster : Entity
    {
        private MainGame _game;

        public Monster(MainGame game, string spriteName, Vector2 position, int health)
            : base(game, spriteName, position, 0.15f, health, 1, 5000)
        {
            _game = game;
        }

        public Monster(MainGame game, string spriteName, int health)
            : this(game, spriteName, Vector2.Zero, health) {}

        public override void Update(GameTime gameTime, Map map)
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

                Rectangle monsterBounds = GetBounds();
                Rectangle playerBounds = player.GetBounds();
                
                if (monsterBounds.Intersects(playerBounds))
                    Attack(player);
            }

            if (target != null)
            {
                Vector2 targetCenter = target.GetCenter();
                Vector2 center = GetCenter();
                
                if ((int) center.X != (int) targetCenter.X)
                {
                    if (targetCenter.X < center.X)
                    {
                        Velocity.X = -0.2f;
                        Animation = "run_left";
                    }
                    else if (targetCenter.X > center.X)
                    {
                        Velocity.X = 0.2f;
                        Animation = "run_right";
                    }
                }
                else
                {
                    Velocity.X = 0;
                    Animation = "idle";
                }
            }

            base.Update(gameTime, map);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteBatch globalUIBatch)
        {
            Rectangle bounds = GetBounds();
            int barOffsetY = -4;
            
            ProgressBar healthBar =
                new ProgressBar(_game, bounds.X, bounds.Y - barOffsetY - 8, bounds.Width,
                    8, 1, (float) Health / MaxHealth, String.Empty,
                    Color.Transparent, Color.Transparent, new Color(46, 204, 113)
                );
            
            ProgressBar cooldownBar =
                new ProgressBar(_game, bounds.X, bounds.Y - barOffsetY, bounds.Width,
                    4, 1, -(CurrentDamageCooldown / DamageCooldown) + 1, String.Empty,
                    Color.Transparent, Color.Transparent, new Color(241, 196, 15)
                );

            healthBar.Draw(spriteBatch);
            cooldownBar.Draw(spriteBatch);
            
            base.Draw(spriteBatch, globalUIBatch);
        }
    }
}
