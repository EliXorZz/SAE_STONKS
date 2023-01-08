using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.UI.Components;

public abstract class InterfaceComponent
{
    private int x, y;
    
    public int X
    {
        get => x;
        set => x = value;
    }
    
    public int Y
    {
        get => y;
        set => y = value;
    }

    public abstract void Draw(SpriteBatch spriteBatch);
}