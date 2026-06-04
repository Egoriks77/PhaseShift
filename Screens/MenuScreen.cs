using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PhaseShift.Core;

namespace PhaseShift.Screens;

public class MenuScreen : IScreen
{
    private readonly GraphicsDevice _gd;
    private readonly SpriteBatch    _sb;
    private readonly SpriteFont     _font;
    private readonly ScreenManager  _screenManager;
    private readonly Game           _game;
    private readonly Texture2D      _pixel;
    private readonly Texture2D      _bg;
    
    private readonly string[] _items = { "Play", "Level Select", "Music Volume", "Exit" };
    private int _selected = 0;
    
    private int _musicVolume = 2;

    private KeyboardState _prevKeys;

    // Фабрика уровней передаётся снаружи
    private readonly System.Func<int, IScreen> _levelFactory;

    public MenuScreen(GraphicsDevice gd, SpriteBatch sb, SpriteFont font,
        ScreenManager screenManager, Game game,
        System.Func<int, IScreen> levelFactory, Texture2D bg)
    {
        _gd            = gd;
        _sb            = sb;
        _font          = font;
        _screenManager = screenManager;
        _game          = game;
        _levelFactory  = levelFactory;
        _bg            = bg;

        _pixel = new Texture2D(gd, 1, 1);
        _pixel.SetData(new[] { Color.White });

        MediaPlayer.Volume = _musicVolume / 10f;
        
    }

    public void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState();

        if (keys.IsKeyDown(Keys.Up) && _prevKeys.IsKeyUp(Keys.Up))
            _selected = (_selected - 1 + _items.Length) % _items.Length;

        if (keys.IsKeyDown(Keys.Down) && _prevKeys.IsKeyUp(Keys.Down))
            _selected = (_selected + 1) % _items.Length;

        // Громкость музыки только когда выбран пункт Music Volume
        if (_selected == 2)
        {
            if (keys.IsKeyDown(Keys.Left) && _prevKeys.IsKeyUp(Keys.Left))
            {
                _musicVolume = System.Math.Max(0, _musicVolume - 1);
                MediaPlayer.Volume = _musicVolume / 10f;
            }
            if (keys.IsKeyDown(Keys.Right) && _prevKeys.IsKeyUp(Keys.Right))
            {
                _musicVolume = System.Math.Min(10, _musicVolume + 1);
                MediaPlayer.Volume = _musicVolume / 10f;
            }
        }

        if (keys.IsKeyDown(Keys.Enter) && _prevKeys.IsKeyUp(Keys.Enter))
        {
            System.Console.WriteLine($"Enter pressed, selected = {_selected}");
            switch (_selected)
            {
                case 0: _screenManager.SetScreen(_levelFactory(0)); break; // Play — уровень 1
                case 1:
                    try {
                        var ls = new LevelSelectScreen(
                            _gd, _sb, _font, _screenManager, _game, _levelFactory, _pixel, _bg);
                        _screenManager.SetScreen(ls);
                        System.Console.WriteLine("LevelSelectScreen set OK");
                    } catch (System.Exception e) {
                        System.Console.WriteLine($"Error: {e.Message}");
                    }
                    break;
                case 3: _game.Exit(); break;
            }
        }

        _prevKeys = keys;
    }

    public void Draw(SpriteBatch sb)
    {
        // Фон
        sb.Draw(_bg, new Rectangle(0, 0, 800, 500), Color.White);
        // Затемнение
        sb.Draw(_pixel, new Rectangle(0, 0, 800, 500), Color.Black * 0.5f);

        // Заголовок
        string title      = "PHASE SHIFT";
        Vector2 titleSize = _font.MeasureString(title);
        // Тень
        sb.DrawString(_font, title,
            new Vector2(400 - titleSize.X / 2 + 3, 80 + 3),
            Color.Black * 0.8f);
        // Текст
        sb.DrawString(_font, title,
            new Vector2(400 - titleSize.X / 2, 80),
            new Color(80, 150, 255));

        // Пункты меню
        for (int i = 0; i < _items.Length; i++)
        {
            bool isSelected = i == _selected;
            float y = 200 + i * 60;

            string label = _items[i];

            // Для пункта громкости показываем значение
            if (i == 2)
                label = $"Music Volume: {_musicVolume}/10";

            Vector2 size   = _font.MeasureString(label) * (isSelected ? 0.6f : 0.5f);
            Color   color  = isSelected ? new Color(80, 220, 150) : Color.Gray;

            // Стрелка у выбранного
            if (isSelected)
                sb.DrawString(_font, ">", new Vector2(180, y), color, 0f,
                    Vector2.Zero, 0.55f, SpriteEffects.None, 0f);

            sb.DrawString(_font, label,
                new Vector2(400 - size.X / 2, y),
                color, 0f, Vector2.Zero,
                isSelected ? 0.6f : 0.5f,
                SpriteEffects.None, 0f);
        }

        // Подсказка
        string hint = _selected == 2 ? "Up/Down navigate  Enter select  Left/Right volume" : "Up/Down navigate   Enter select";
        Vector2 hintSize = _font.MeasureString(hint) * 0.3f;
        sb.DrawString(_font, hint,
            new Vector2(400 - hintSize.X / 2, 460),
            Color.DarkGray * 0.8f, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
    }
}