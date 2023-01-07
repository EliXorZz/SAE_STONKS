using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.UI.Components;

namespace TheGame.UI;

public class UIQueue
{
    private Queue<QueueComponent> _queue;
    private QueueComponent _currentComponent;

    private float cooldown;
    
    public UIQueue()
    {
        _queue = new Queue<QueueComponent>();
        _currentComponent = null;
    }
    
    public void AddToQueue(InterfaceComponent component, float time)
    {
        QueueComponent queueComponent = new QueueComponent(component, time);
        _queue.Enqueue(queueComponent);
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        cooldown = Math.Max(0, cooldown - elapsed);
        
        if (_currentComponent == null || cooldown > _currentComponent.Time)
            if (_queue.Count > 0)
                _currentComponent = _queue.Dequeue();
            else
                _currentComponent = null;
    }
    
    public void Draw(SpriteBatch mainSpriteBatch)
    {
        _currentComponent?.Component.Draw(mainSpriteBatch);
    }
}