using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Screens;
using TheGame.Manager;
using TheGame.Screen;
using TheGame.UI.Components;

namespace TheGame.Core
{
    public abstract class Entity
    {
        private MainGame _game;
        private Random _random;

        private AnimatedSprite _sprite;

        private string _animation;

        private List<FadeInterfaceComponent> _interfaceComponents;

        private Vector2 _velocity;

        private Vector2 _position;
        private float _gravity;

        private int _health;
        private int _maxHealth;

        private int _damage;
        private float _damageCooldown;
        private Color _color;

        public Entity(MainGame game, string spriteName, Vector2 position,
            float gravity, int health, int damage, float damageCooldown, Color color)
        {
            _game = game;
            _random = new Random();

            ScreenStateManager screenStateManager = game.ScreenStateManager;
            GameScreen inGameScreen = screenStateManager.GetScreen(ScreenState.InGame);

            SpriteSheet spriteSheet = inGameScreen.Content
                .Load<SpriteSheet>($"sprites/{spriteName}/animations.sf", new JsonContentLoader());

            _sprite = new AnimatedSprite(spriteSheet);
            _sprite.Origin = Vector2.Zero;

            _animation = "idle";

            _interfaceComponents = new List<FadeInterfaceComponent>();

            _velocity = Vector2.Zero;

            _position = position;
            _gravity = gravity;

            _health = health;
            _maxHealth = health;

            _damage = damage;
            _damageCooldown = damageCooldown;

            _color = color;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public AnimatedSprite Sprite
        {
            get => _sprite;
        }

        public string Animation
        {
            get => _animation;
            set => _animation = value;
        }

        public ref Vector2 Velocity
        {
            get => ref _velocity;
        }

        public float Gravity
        {
            get => _gravity;
            set => _gravity = value;
        }

        public ref Vector2 Position
        {
            get => ref _position;
        }

        public int Health
        {
            get => _health;
            set => _health = Math.Max(0, value);
        }

        public int MaxHealth
        {
            get => _maxHealth;
        }

        public bool IsDead
        {
            get => _health <= 0;
        }

        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public float DamageCooldown
        {
            get => _damageCooldown;
            set => _damageCooldown = value;
        }

        public void AddFadeInterfaceComponent(float delay, float time, Vector2 offset, InterfaceComponent component)
        {
            _interfaceComponents.Add(
                new FadeInterfaceComponent(delay, time, this, offset, component)
            );
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y,
                Sprite.TextureRegion.Width, Sprite.TextureRegion.Height);
        }

        public Vector2 GetCenter()
        {
            Rectangle bounds = GetBounds();

            return new Vector2(
                Position.X + (float)bounds.Width / 2,
                Position.Y + (float)bounds.Height / 2
            );
        }

        public float GetDistanceBetweenEntity(Entity entity)
        {
            return Vector2.Distance(GetCenter(), entity.GetCenter());
        }

        public bool IsCollisionMap(Map map, int offsetX = 0, int offsetY = 0)
        {
            Rectangle bounds = GetBounds();

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    ushort tX = (ushort)((bounds.X + x + offsetX) / map.TiledMap.TileWidth);
                    ushort tY = (ushort)((bounds.Y + y + offsetY) / map.TiledMap.TileHeight);

                    if (map.GetTile(MapLayer.GROUND, tX, tY).HasValue)
                        return true;
                }
            }

            return false;
        }

        public virtual void Update(GameTime gameTime, Map map)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            

            Position += Velocity *2 ;

            if (IsCollisionMap(map))
            {
                Position.X -= Velocity.X *2;
                Velocity.X = 0;
            }

            if (IsCollisionMap(map) && Velocity.X == 0)
            {
                Position -= Velocity *2;
                Velocity.Y = 0;
            }
            else
                Velocity.Y += Gravity ;

            List<FadeInterfaceComponent> removeComponents = new List<FadeInterfaceComponent>();
            foreach (FadeInterfaceComponent component in _interfaceComponents)
            {
                bool finish = component.Update(gameTime);

                if (finish)
                    removeComponents.Add(component);
            }

            foreach (FadeInterfaceComponent component in removeComponents)
                _interfaceComponents.Remove(component);

            Sprite.Play(Animation);
            Sprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch mainSpriteBatch, SpriteBatch uiSpriteBatch)
        {
            Sprite.Color = Color;
            mainSpriteBatch.Draw(Sprite, Position);

            foreach (FadeInterfaceComponent component in _interfaceComponents)
                component.Draw(mainSpriteBatch);
        }
    }
}
