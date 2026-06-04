using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using PhaseShift.Core;
using PhaseShift.Entities;

namespace PhaseShift.Screens;

public class GameScreen : IScreen
{
    private readonly GraphicsDevice _gd;
    private readonly SpriteBatch    _sb;
    private readonly SpriteFont     _font;
    private readonly ScreenManager  _screenManager;
    private readonly Game           _game;
    private readonly Texture2D      _bg;
    private readonly Texture2D      _pixel;

    private Player          _player;
    private Rectangle       _ground;
    private List<Platform>  _platforms;
    private List<PhaseWall> _walls;
    private List<Spike>     _spikes;
    private LevelExit       _exit;

    private const int ScreenWidth  = 800;
    private const int ScreenHeight = 500;
    private int _worldWidth;

    private Vector2 _cameraOffset;
    private bool    _levelComplete = false;
    private float   _winTimer      = 0f;
    private float   _flashTimer    = 0f;
    private float   _textScale     = 0f;

    private KeyboardState _prevKeys;

    // Следующий уровень (-1 если финал)
    private readonly int _levelIndex;
    private readonly System.Func<int, IScreen> _levelFactory;

    public GameScreen(GraphicsDevice gd, SpriteBatch sb, SpriteFont font,
        ScreenManager screenManager, Game game,
        Texture2D bg, int levelIndex, System.Func<int, IScreen> levelFactory)
    {
        _gd            = gd;
        _sb            = sb;
        _font          = font;
        _screenManager = screenManager;
        _game          = game;
        _bg            = bg;
        _levelIndex    = levelIndex;
        _levelFactory  = levelFactory;
        _prevKeys = Keyboard.GetState();

        _pixel = new Texture2D(gd, 1, 1);
        _pixel.SetData(new[] { Color.White });

        LoadLevel(levelIndex);
    }

    private void LoadLevel(int index)
    {
        _levelComplete = false;
        _winTimer      = 0f;
        _cameraOffset  = Vector2.Zero;

        switch (index)
        {
            case 0: LoadLevel1(); break;
            case 1: LoadLevel2(); break;
            case 2: LoadLevel3(); break;
        }
    }

    private void LoadLevel1()
    {
        _worldWidth = 3200;
        _ground     = new Rectangle(0, 460, _worldWidth, 40);

        _platforms = new List<Platform>
        {
            new Platform(_gd, new Rectangle(200,  360, 120, 20), PhaseState.Solid,
                TextureFactory.CreatePlatformSolid(_gd, 120, 20)),
            new Platform(_gd, new Rectangle(400,  300, 120, 20), PhaseState.Solid,
                TextureFactory.CreatePlatformSolid(_gd, 120, 20)),
            new Platform(_gd, new Rectangle(650,  360, 120, 20), PhaseState.Ghost,
                TextureFactory.CreatePlatformGhost(_gd, 120, 20)),
            new Platform(_gd, new Rectangle(850,  300, 120, 20), PhaseState.Ghost,
                TextureFactory.CreatePlatformGhost(_gd, 120, 20)),
            new Platform(_gd, new Rectangle(1100, 380, 100, 20), PhaseState.Solid,
                TextureFactory.CreatePlatformSolid(_gd, 100, 20)),
            new Platform(_gd, new Rectangle(1280, 320, 100, 20), PhaseState.Ghost,
                TextureFactory.CreatePlatformGhost(_gd, 100, 20)),
            new Platform(_gd, new Rectangle(1450, 380, 100, 20), PhaseState.Solid,
                TextureFactory.CreatePlatformSolid(_gd, 100, 20)),
            new Platform(_gd, new Rectangle(1620, 320, 100, 20), PhaseState.Ghost,
                TextureFactory.CreatePlatformGhost(_gd, 100, 20)),
            new Platform(_gd, new Rectangle(1900, 350, 120, 20), PhaseState.Solid,
                TextureFactory.CreatePlatformSolid(_gd, 120, 20)),
            new Platform(_gd, new Rectangle(2100, 280, 120, 20), PhaseState.Ghost,
                TextureFactory.CreatePlatformGhost(_gd, 120, 20)),
            new Platform(_gd, new Rectangle(2300, 350, 120, 20), PhaseState.Solid,
                TextureFactory.CreatePlatformSolid(_gd, 120, 20)),
        };

        _walls = new List<PhaseWall>
        {
            new PhaseWall(_gd, new Rectangle(580,  330, 20, 130), PhaseState.Solid,
                TextureFactory.CreateWallSolid(_gd, 20, 130)),
            new PhaseWall(_gd, new Rectangle(1050, 330, 20, 130), PhaseState.Ghost,
                TextureFactory.CreateWallGhost(_gd, 20, 130)),
            new PhaseWall(_gd, new Rectangle(1800, 330, 20, 130), PhaseState.Solid,
                TextureFactory.CreateWallSolid(_gd, 20, 130)),
        };

        _spikes = new List<Spike>
        {
            new Spike(_gd, new Rectangle(300,  440, 40, 20), TextureFactory.CreateSpike(_gd, 40, 20)),
            new Spike(_gd, new Rectangle(750,  440, 40, 20), TextureFactory.CreateSpike(_gd, 40, 20)),
            new Spike(_gd, new Rectangle(1200, 440, 40, 20), TextureFactory.CreateSpike(_gd, 40, 20)),
            new Spike(_gd, new Rectangle(1500, 440, 40, 20), TextureFactory.CreateSpike(_gd, 40, 20)),
            new Spike(_gd, new Rectangle(2000, 440, 40, 20), TextureFactory.CreateSpike(_gd, 40, 20)),
            new Spike(_gd, new Rectangle(2200, 440, 40, 20), TextureFactory.CreateSpike(_gd, 40, 20)),
        };

        _exit   = new LevelExit(_gd, new Rectangle(2900, 360, 60, 100),
            TextureFactory.CreateExit(_gd, 60, 100));
        _player = new Player(_gd, new Vector2(50, 400),
            TextureFactory.CreatePlayerSolid(_gd),
            TextureFactory.CreatePlayerGhost(_gd));
    }

    private void LoadLevel2()
{
    _worldWidth = 3600;
    _ground     = new Rectangle(0, 460, _worldWidth, 40);

    _platforms = new List<Platform>
    {
        new Platform(_gd, new Rectangle(50, 420, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(220, 380, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(400, 340, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(600, 380, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(780, 320, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(980, 370, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(1160, 320, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),
        
        new Platform(_gd, new Rectangle(1380, 370, 120, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 120, 20)),
        
        new Platform(_gd, new Rectangle(1600, 320, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),

        new Platform(_gd, new Rectangle(1800, 370, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),

        new Platform(_gd, new Rectangle(2000, 320, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),

        new Platform(_gd, new Rectangle(2200, 370, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),

        new Platform(_gd, new Rectangle(2400, 320, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),

        new Platform(_gd, new Rectangle(2600, 370, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),

        new Platform(_gd, new Rectangle(2850, 340, 100, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 100, 20)),

        new Platform(_gd, new Rectangle(3100, 380, 100, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 100, 20)),
    };

    _walls = new List<PhaseWall>
    {
        new PhaseWall(_gd, new Rectangle(540, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
        
        new PhaseWall(_gd, new Rectangle(920, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),
        
        new PhaseWall(_gd, new Rectangle(1320, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),

        
        new PhaseWall(_gd, new Rectangle(1740, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),


        new PhaseWall(_gd, new Rectangle(2150, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
        
        new PhaseWall(_gd, new Rectangle(2550, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),
        
        new PhaseWall(_gd, new Rectangle(2790, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
    };

    _spikes = new List<Spike>
    {
        // Земля расставлена между платформами
        new Spike(_gd, new Rectangle(160, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(340, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(700, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1050, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1500, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1900, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(2300, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(2650, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(3000, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),

        // Шипы на платформе x=1380 по краям, середина безопасна
        new Spike(_gd, new Rectangle(1382, 350, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1460, 350, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),

        // Шипы на стене справа смотрят влево, прижиматься нельзя
        new Spike(_gd, new Rectangle(535, 350, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Left),
        new Spike(_gd, new Rectangle(535, 390, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Left),

        
    };

    _exit   = new LevelExit(_gd, new Rectangle(3300, 360, 60, 100),
        TextureFactory.CreateExit(_gd, 60, 100));
    _player = new Player(_gd, new Vector2(60, 370),
        TextureFactory.CreatePlayerSolid(_gd),
        TextureFactory.CreatePlayerGhost(_gd));
}

private void LoadLevel3()
{
    _worldWidth = 4200;
    _ground     = new Rectangle(0, 460, _worldWidth, 40);

    _platforms = new List<Platform>
    {
        // Старт два пути сразу.
        new Platform(_gd, new Rectangle(150, 280, 100, 20), PhaseState.Ghost,  // ЛОВУШКА
            TextureFactory.CreatePlatformGhost(_gd, 100, 20)),
        new Platform(_gd, new Rectangle(150, 390, 100, 20), PhaseState.Solid,  // верный путь
            TextureFactory.CreatePlatformSolid(_gd, 100, 20)),

        // После нижней Solid нужно переключиться в Ghost
        new Platform(_gd, new Rectangle(370, 350, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),

        // Развилка: две Ghost платформы рядом одна ведёт в тупик
        new Platform(_gd, new Rectangle(560, 280, 70, 20), PhaseState.Ghost,   // ЛОВУШКА — тупик
            TextureFactory.CreatePlatformGhost(_gd, 70, 20)),
        new Platform(_gd, new Rectangle(560, 380, 70, 20), PhaseState.Solid,   // верный путь
            TextureFactory.CreatePlatformSolid(_gd, 70, 20)),

        // Узкий коридор нужно быть Solid
        new Platform(_gd, new Rectangle(750, 370, 60, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 60, 20)),
        new Platform(_gd, new Rectangle(930, 370, 60, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 60, 20)),

        // Переключение в Ghost единственный способ пройти стену
        new Platform(_gd, new Rectangle(1130, 340, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),

        // Снова развилка три платформы, только средняя верная
        new Platform(_gd, new Rectangle(1400, 240, 60, 20), PhaseState.Solid,  // ЛОВУШКА — слишком высоко
            TextureFactory.CreatePlatformSolid(_gd, 60, 20)),
        new Platform(_gd, new Rectangle(1400, 350, 60, 20), PhaseState.Ghost,  // верный путь
            TextureFactory.CreatePlatformGhost(_gd, 60, 20)),
        new Platform(_gd, new Rectangle(1400, 430, 60, 20), PhaseState.Solid,  // ЛОВУШКА — шипы рядом
            TextureFactory.CreatePlatformSolid(_gd, 60, 20)),

        // Длинный участок чередования
        new Platform(_gd, new Rectangle(1620, 370, 60, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 60, 20)),
        new Platform(_gd, new Rectangle(1800, 310, 60, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 60, 20)),
        new Platform(_gd, new Rectangle(1980, 370, 60, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 60, 20)),
        new Platform(_gd, new Rectangle(2160, 310, 60, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 60, 20)),

        // Финальная секция нужно быть Ghost чтобы пройти сквозь синие стены
        new Platform(_gd, new Rectangle(2400, 360, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),
        new Platform(_gd, new Rectangle(2650, 360, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),
        new Platform(_gd, new Rectangle(2900, 360, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),
        new Platform(_gd, new Rectangle(3150, 360, 80, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 80, 20)),
        new Platform(_gd, new Rectangle(3400, 360, 80, 20), PhaseState.Ghost,
            TextureFactory.CreatePlatformGhost(_gd, 80, 20)),
        new Platform(_gd, new Rectangle(3650, 380, 100, 20), PhaseState.Solid,
            TextureFactory.CreatePlatformSolid(_gd, 100, 20)),
    };

    _walls = new List<PhaseWall>
    {
        // Блокирует верхний ложный путь
        new PhaseWall(_gd, new Rectangle(340, 260, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),

        // Основные стены на пути
        new PhaseWall(_gd, new Rectangle(510, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(700, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(1070, 320, 20, 140), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 140)),

        // Двойные стены нужно дважды переключиться
        new PhaseWall(_gd, new Rectangle(1340, 320, 20, 140), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 140)),
        new PhaseWall(_gd, new Rectangle(1370, 320, 20, 140), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 140)),

        // Финальный лабиринт стен
        new PhaseWall(_gd, new Rectangle(2340, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(2590, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(2840, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(3090, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(3340, 330, 20, 130), PhaseState.Solid,
            TextureFactory.CreateWallSolid(_gd, 20, 130)),
        new PhaseWall(_gd, new Rectangle(3590, 330, 20, 130), PhaseState.Ghost,
            TextureFactory.CreateWallGhost(_gd, 20, 130)),
    };

    _spikes = new List<Spike>
    {
        // Ловушка на верхнем пути шипы на Ghost платформе x=150
        new Spike(_gd, new Rectangle(170, 260, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(200, 260, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(230, 260, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),

        // Ловушка тупик наверху x=560
        new Spike(_gd, new Rectangle(570, 260, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(600, 260, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),

        // Шипы на нижней ловушке x=1400 y=430
        new Spike(_gd, new Rectangle(1405, 410, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1430, 410, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Up),

        // Шипы на стенах смотрят в сторону игрока
        new Spike(_gd, new Rectangle(505, 340, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Right),
        new Spike(_gd, new Rectangle(505, 370, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Right),
        new Spike(_gd, new Rectangle(695, 340, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Right),
        new Spike(_gd, new Rectangle(695, 370, 20, 20),
            TextureFactory.CreateSpike(_gd, 20, 20), SpikeDirection.Right),

        // Земля
        new Spike(_gd, new Rectangle(300,  440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(480,  440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(680,  440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1000, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1200, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1530, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(1750, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(2050, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(2300, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(2700, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(3000, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(3250, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
        new Spike(_gd, new Rectangle(3500, 440, 40, 20),
            TextureFactory.CreateSpike(_gd, 40, 20), SpikeDirection.Up),
    };

    _exit   = new LevelExit(_gd, new Rectangle(3900, 360, 60, 100),
        TextureFactory.CreateExit(_gd, 60, 100));
    _player = new Player(_gd, new Vector2(50, 400),
        TextureFactory.CreatePlayerSolid(_gd),
        TextureFactory.CreatePlayerGhost(_gd));
}

    public void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState();

        // Выход из уровня по Escape
        if (keys.IsKeyDown(Keys.Escape) && _prevKeys.IsKeyUp(Keys.Escape))
        {
            _screenManager.SetScreen(new MenuScreen(
                _gd, _sb, _font, _screenManager, _game, _levelFactory, _bg));
            _prevKeys = keys;
            return;
        }

        if (!_levelComplete)
        {
            _player.Update(gameTime, _ground, _platforms, _walls, _worldWidth);

            foreach (var spike in _spikes)
                if (spike.Intersects(_player.Bounds)) { _player.Respawn(); break; }

            if (_exit.Intersects(_player.Bounds))
                _levelComplete = true;

            float targetX = _player.Position.X - ScreenWidth / 2f + _player.Width / 2f;
            targetX = MathHelper.Clamp(targetX, 0, _worldWidth - ScreenWidth);
            _cameraOffset.X = MathHelper.Lerp(_cameraOffset.X, targetX, 0.1f);
        }
        else
        {
            _winTimer   += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _flashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _textScale   = MathHelper.Clamp(_winTimer / 0.5f, 0f, 1f);
            if (_winTimer > 0.5f)
                _textScale = 1f + 0.05f * (float)System.Math.Sin(_flashTimer * 5f);

            // Переход на следующий уровень по Enter
            if (_winTimer > 1f &&
                keys.IsKeyDown(Keys.Enter) && _prevKeys.IsKeyUp(Keys.Enter))
            {
                if (_levelIndex < 2)
                    _screenManager.SetScreen(_levelFactory(_levelIndex + 1));
                else
                    _screenManager.SetScreen(new MenuScreen(
                        _gd, _sb, _font, _screenManager, _game, _levelFactory, _bg));
            }
        }

        _prevKeys = keys;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(_bg, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);

        // Земля
        sb.Draw(_pixel,
            new Rectangle(-((int)_cameraOffset.X), 460, _worldWidth, 40),
            new Color(25, 25, 40));

        foreach (var p in _platforms) p.Draw(sb, _player.Phase, _cameraOffset);
        foreach (var w in _walls)     w.Draw(sb, _player.Phase, _cameraOffset);
        foreach (var s in _spikes)    s.Draw(sb, _cameraOffset);
        _exit.Draw(sb, _cameraOffset);
        _player.Draw(sb, _cameraOffset);
        
        string phaseText = _player.Phase == PhaseState.Solid ? "SOLID" : "GHOST";
        Color  phaseCol  = _player.Phase == PhaseState.Solid
            ? new Color(80, 150, 255) : new Color(150, 100, 255);
        sb.DrawString(_font, $"Phase: {phaseText}",
            new Vector2(10, 10), phaseCol, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
        sb.DrawString(_font, $"Level {_levelIndex + 1}",
            new Vector2(10, 35), Color.Gray, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);
        sb.DrawString(_font, "Esc - Menu",
            new Vector2(10, 55), Color.DarkGray, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);

        // Победный экран
        if (_levelComplete)
        {
            sb.Draw(_pixel, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.Black * 0.65f);

            bool isFinal   = _levelIndex >= 2;
            string mainTxt = isFinal ? "YOU ESCAPED THE ABYSS!" : "LEVEL COMPLETE!";
            string subTxt  = isFinal
                ? "The darkness couldn't hold you."
                : $"Press Enter for Level {_levelIndex + 2}";

            Vector2 mainOr = _font.MeasureString(mainTxt) / 2f;
            Vector2 subOr  = _font.MeasureString(subTxt)  / 2f;
            Vector2 center = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f - 30);

            sb.DrawString(_font, mainTxt, center + new Vector2(3, 3),
                Color.Black * 0.8f, 0f, mainOr, _textScale, SpriteEffects.None, 0f);
            sb.DrawString(_font, mainTxt, center,
                new Color(20, 220, 140), 0f, mainOr, _textScale, SpriteEffects.None, 0f);

            if (_winTimer > 0.8f)
            {
                float a = MathHelper.Clamp((_winTimer - 0.8f) / 0.4f, 0f, 1f);
                sb.DrawString(_font, subTxt,
                    new Vector2(ScreenWidth / 2f, ScreenHeight / 2f + 40),
                    Color.LightGray * a, 0f, subOr, 0.45f, SpriteEffects.None, 0f);
            }
        }
    }
}