using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PhaseShift.Core;
using PhaseShift.Screens;

namespace PhaseShift;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch           _spriteBatch;
    private ScreenManager         _screenManager;
    private SpriteFont            _font;
    private Song                  _bgMusic;
    private Texture2D             _bg;


    private const int ScreenWidth  = 800;
    private const int ScreenHeight = 600;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth  = ScreenWidth;
        _graphics.PreferredBackBufferHeight = ScreenHeight;
    }

    protected override void LoadContent()
    {
        _spriteBatch   = new SpriteBatch(GraphicsDevice);
        _screenManager = new ScreenManager();
        _font          = Content.Load<SpriteFont>("fonts/GameFont");
        _bgMusic       = Content.Load<Song>("music/action_02");
        _bg            = TextureFactory.CreateBackground(GraphicsDevice, ScreenWidth, ScreenHeight);

        MediaPlayer.Play(_bgMusic);
        MediaPlayer.IsRepeating = true;

        // Фабрика уровней создаёт GameScreen по индексу
        IScreen LevelFactory(int index) => new GameScreen(
            GraphicsDevice, _spriteBatch, _font,
            _screenManager, this, _bg, index, LevelFactory);

        _screenManager.SetScreen(new MenuScreen(
            GraphicsDevice, _spriteBatch, _font,
            _screenManager, this, LevelFactory, _bg));
    }

    protected override void Update(GameTime gameTime)
    {
        _screenManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(5, 5, 20));
        _spriteBatch.Begin();
        _screenManager.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}