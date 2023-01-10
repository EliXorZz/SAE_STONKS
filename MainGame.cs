using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGame.Core;
using TheGame.Manager;
using TheGame.Screen;

namespace TheGame;

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;

    private ScreenStateManager _screenStateManager;
    
    private MapManager _mapManager;

    private EntityManager _entityManager;
    private MonsterManager _monsterManager;
    private PlayerManager _playerManager;

    private WaveManager _waveManager;

    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";
    }

    public ScreenStateManager ScreenStateManager
    {
        get => _screenStateManager;
    }
    
    public MapManager MapManager
    {
        get => _mapManager;
    }

    public EntityManager EntityManager
    {
        get => _entityManager;
    }

    public MonsterManager MonsterManager
    {
        get => _monsterManager;
    }

    public PlayerManager PlayerManager
    {
        get => _playerManager;
    }

    public WaveManager WaveManager
    {
        get => _waveManager;
    }

    protected override void Initialize()
    {
        _screenStateManager = new ScreenStateManager(this);
        
        _mapManager = new MapManager(this);

        _entityManager = new EntityManager(_mapManager);
        _monsterManager = new MonsterManager(_entityManager);
        _playerManager = new PlayerManager(_entityManager);

        _waveManager = new WaveManager(this);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        
        MapManager.SelectMap("default");
        ScreenStateManager.CurrentScreen = ScreenState.MainMenu;

        PlayerManager.CreatePlayer(this, new PlayerControls(Keys.Left, Keys.Right, Keys.Up, Keys.Enter), "Joueur 1");
    }
}