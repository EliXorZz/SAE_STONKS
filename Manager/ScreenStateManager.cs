using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using TheGame.Screen;

namespace TheGame.Manager;

public class ScreenStateManager
{
    private MainGame _game;
    
    private ScreenManager _screenManager;
    private Dictionary<ScreenState, GameScreen> _gameScreens;
    private ScreenState _currentScreen;

    public ScreenStateManager(MainGame game)
    {
        _game = game;
        
        _screenManager = new ScreenManager();
        game.Components.Add(_screenManager);

        _gameScreens = new Dictionary<ScreenState, GameScreen>();
        
        _gameScreens.Add(ScreenState.PauseMenu, new PauseMenuScreen(game));
        _gameScreens.Add(ScreenState.MainMenu, new MainMenuScreen(game));
        _gameScreens.Add(ScreenState.InGame, new InGameScreen(game));
    }

    public ScreenState CurrentScreen
    {
        get => _currentScreen;
        set
        {
            _screenManager.LoadScreen(
                _gameScreens[value],
                new FadeTransition(_game.GraphicsDevice, Color.Black, 0.5f)
            );
            
            _currentScreen = value;
        }
    }

    public GameScreen GetScreen(ScreenState state)
    {
        return _gameScreens[state];
    }
}