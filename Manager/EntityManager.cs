﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using TheGame.Core;

namespace TheGame.Manager
{
    public class EntityManager
    {
        private MapManager _mapManager;
        
        private List<Entity> _entities;
        private Random _random;

        public EntityManager(MapManager mapManager)
        {
            _mapManager = mapManager;
            
            _entities = new List<Entity>();
            _random = new Random();
        }

        public List<Entity> Entities
        {
            get => _entities;
        }

        public void CreateEntity(Entity entity)
        {
            _entities.Add(entity);
        }
        
        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public void RandomPosition(Entity entity)
        {
            Rectangle bounds = entity.GetBounds();
            Map map = _mapManager.CurrentMap;
            
            entity.Position.X = _random.Next(map.TiledMap.WidthInPixels - bounds.Width);
            entity.Position.Y = map.TiledMap.HeightInPixels - bounds.Height;

            while (entity.IsCollisionMap(map))
                entity.Position.Y -= map.TiledMap.TileHeight;
        }

        public void Update(GameTime gameTime)
        {
            Map map = _mapManager.CurrentMap;
            
            if (map != null)
            {
                foreach (Entity entity in Entities)
                {
                    if (entity.Health > 0)
                        entity.Update(gameTime, map);  
                    else
                        RemoveEntity(entity);
                }   
            }
        }

        public void Draw(SpriteBatch mainSpriteBatch, SpriteBatch uiSpriteBatch)
        {
            Map map = _mapManager.CurrentMap;
            
            if (map != null)
                foreach (Entity entity in Entities)
                    entity.Draw(mainSpriteBatch, uiSpriteBatch);
        }
    }
}
