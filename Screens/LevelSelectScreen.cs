using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhaseShift.Core;

namespace PhaseShift.Screens;

public class LevelSelectScreen : IScreen
{
    private readonly GraphicsDevice _gd;
    private readonly SpriteBatch    _sb;
    private readonly SpriteFont     _font;
    private readonly ScreenManager  _screenManager;
    private readonly Game           _game;
    private readonly Texture2D      _pixel;
    private readonly Texture2D      _bg;
    private readonly System.Func<int, IScreen> _levelFactory;

    private int _selected = 0;
    private readonly string[] _levels = { "Level 1 - Tutorial", "Level 2 - Shadows", "Level 3 - Abyss" };
    private KeyboardState _prevKeys;

    public LevelSelectScreen(GraphicsDevice gd, SpriteBatch sb, SpriteFont font,
        ScreenManager screenManager, Game game,
        System.Func<int, IScreen> levelFactory, Texture2D pixel, Texture2D bg)
    {
        _gd = gd; _sb = sb; _font = font;
        _screenManager = screenManager; _game = game;
        _levelFactory = levelFactory; _pixel = pixel; _bg = bg;
        _prevKeys = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState();

        if (keys.IsKeyDown(Keys.Up)   && _prevKeys.IsKeyUp(Keys.Up))
            _selected = (_selected - 1 + _levels.Length) % _levels.Length;
        if (keys.IsKeyDown(Keys.Down) && _prevKeys.IsKeyUp(Keys.Down))
            _selected = (_selected + 1) % _levels.Length;

        if (keys.IsKeyDown(Keys.Enter) && _prevKeys.IsKeyUp(Keys.Enter))
            _screenManager.SetScreen(_levelFactory(_selected));

        // Назад в меню
        if (keys.IsKeyDown(Keys.Escape) && _prevKeys.IsKeyUp(Keys.Escape))
            _screenManager.SetScreen(new MenuScreen(
                _gd, _sb, _font, _screenManager, _game, _levelFactory, _bg));

        _prevKeys = keys;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(_bg,    new Rectangle(0, 0, 800, 500), Color.White);
        sb.Draw(_pixel, new Rectangle(0, 0, 800, 500), Color.Black * 0.55f);

        string title     = "SELECT LEVEL";
        Vector2 titleSz  = _font.MeasureString(title) * 0.7f;
        sb.DrawString(_font, title,
            new Vector2(400 - titleSz.X / 2, 80),
            new Color(80, 150, 255), 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

        for (int i = 0; i < _levels.Length; i++)
        {
            bool isSelected = i == _selected;
            float scale = isSelected ? 0.6f : 0.5f;
            Color color = isSelected ? new Color(80, 220, 150) : Color.Gray;
            Vector2 sz  = _font.MeasureString(_levels[i]) * scale;

            if (isSelected)
                sb.DrawString(_font, ">", new Vector2(180, 200 + i * 70),
                    color, 0f, Vector2.Zero, 0.55f, SpriteEffects.None, 0f);

            sb.DrawString(_font, _levels[i],
                new Vector2(400 - sz.X / 2, 200 + i * 70),
                color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        string hint = "Up/Down select   Enter play   Esc back";
        Vector2 hintSz = _font.MeasureString(hint) * 0.3f;
        sb.DrawString(_font, hint,
            new Vector2(400 - hintSz.X / 2, 460),
            Color.DarkGray * 0.8f, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
    }
}