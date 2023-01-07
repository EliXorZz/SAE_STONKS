using TheGame.UI.Components;

namespace TheGame.UI;

public class QueueComponent
{
    private InterfaceComponent _component;
    private float _time;
    
    public QueueComponent(InterfaceComponent component, float time)
    {
        _component = component;
        _time = time;
    }
    
    public InterfaceComponent Component
    {
        get => _component;
    }
    
    public float Time
    {
        get => _time;
    }
}