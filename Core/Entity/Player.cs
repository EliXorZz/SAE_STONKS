﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Manager;
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

        private float _deadTime;

        private float _attackDelay;
        private float _lastAttack;

        private float _regenTime;

        private int _sens;
        private ProgressBar _healthBar;

        private float _cooldownTransformation;

        public Player(MainGame game, PlayerControls controls, int id, string pseudo, Vector2 position, Color color)
            : base(game, "blue_character", position, 0.10f, 100, 15, 2000, color)
        {
            _game = game;
            _controls = controls;

            _sens = 1;

            _id = id;
            _pseudo = pseudo;

            _swordMode = false;
            _swordDamage = 10;

            _deadTime = 0;

            _attackDelay = 0;
            _lastAttack = 0;

            _regenTime = 0;

            int width = 300;
            int height = 40;
            int gap = 10;

            _healthBar = new ProgressBar(
                _game, ScreenState.InGame, 20, 20 + Id * (height + gap), width, height, 1,
                (float)Health / MaxHealth,
                $"Player {Pseudo} | {Health}/{MaxHealth}",
                Color.White, Color, new Color(Math.Min(Color.R + 100, 255), Math.Min(Color.G + 100, 255), Math.Min(Color.B + 100, 255))
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

        public float LastAttack
        {
            get => _lastAttack;
            set => _lastAttack = value;
        }

        public float CooldownTransformation 
        { 
          get => _cooldownTransformation; 
        }

        public override void Update(GameTime gameTime, Map map)
        {
            if (!IsDead)
            {
                float total = (float)gameTime.TotalGameTime.TotalMilliseconds;
                float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;

                if (Controls.IsTransform() && !SwordMode && _cooldownTransformation >= 500)
                {
                    SwordMode = true;
                    _cooldownTransformation = 0;

                }
                else if (Controls.IsTransform() && SwordMode && _cooldownTransformation >= 500  )
                {
                    SwordMode = false;
                    _cooldownTransformation = 0;


                }

                _cooldownTransformation += elapsed;

                if (Controls.IsAttack() && !SwordMode)
                {
                    foreach (Monster monster in _game.MonsterManager.Monsters)
                    {
                        Rectangle playerBounds = GetBounds();
                        Rectangle monsterBounds = monster.GetBounds();

                        if (monsterBounds.Intersects(playerBounds))
                            Attack(monster, gameTime);
                    }
                }

                if (Controls.IsLeft() && !SwordMode)
                {
                    _sens = -1;
                    Velocity.X = -1;

                    

                    if (Controls.IsAttack())
                        Animation = "combatG";
                        _game.SoundManager.PlayEffect("sword", gameTime);

                    }
                    else
                    {
                        Animation = "courseG";
                        if (IsCollisionMap(map, 0, 1) && !SwordMode) _game.SoundManager.PlayEffect("pas", gameTime);
                    }
                        

                }
                else if (Controls.IsRight() && !SwordMode)
                {
                    _sens = 1;
                    Velocity.X = 1;

                    
                    if (Controls.IsAttack())
                        Animation = "combatD";
                        _game.SoundManager.PlayEffect("sword", gameTime);
                    }

                    else
                    {
                        Animation = "courseD";
                        if (IsCollisionMap(map, 0, 1) && !SwordMode) _game.SoundManager.PlayEffect("pas", gameTime);
                    }
                        
                }
                else if (!SwordMode)
                {
                    Velocity.X = 0;

                    if (Controls.IsAttack() && _sens == 1)
                    {
                        _game.SoundManager.PlayEffect("sword", gameTime);
                        Animation = "combatD";
                    else if (Controls.IsAttack() && _sens == -1)
                    {
                        _game.SoundManager.PlayEffect("sword", gameTime);
                        Animation = "combatG";
                    else
                        Animation = "idle";
                }

                if (Controls.IsJump() && IsCollisionMap(map, 0, 1) && !SwordMode)
                    Velocity.Y = -3;
                
                    

                if (Health < MaxHealth && _lastAttack + 6000 < total)
                    {
                    _regenTime += elapsed;

                    if (_regenTime >= 1500)
                    {
                        int newHealth = 2;

                        Health = Math.Min(MaxHealth, Health + newHealth);

                        AddFadeInterfaceComponent(
                        200,
                        1500,
                        new Vector2(0, -3),
                        new Text(_game, ScreenState.InGame, "font_small", 0, 0, $"+{newHealth}", Color.Green));

                        _regenTime = 0;
                    }
                }

                _healthBar.Progress = (float)Health / MaxHealth;
                _healthBar.Input = $"Player {Pseudo} | {Health}/{MaxHealth}";

                _healthBar.Update();
            }
            else if(!SwordMode)
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

        public void Attack(Monster entity, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _attackDelay += elapsed;

            if (_attackDelay >= 1800)
            {
                _attackDelay = 0;

                Random _random = new Random();
                int realDamage = _random.Next(1, Damage);

                entity.Health -= realDamage;

                entity.AddFadeInterfaceComponent(
                    200,
                    1500,
                    new Vector2(0, -3),
                    new Text(_game, ScreenState.InGame, "font_small", 0, 0, $"-{realDamage}", Color.Red));
            }
        }

    }
}