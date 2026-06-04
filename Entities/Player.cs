using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace PhaseShift.Entities;

public class Player
{
    public Vector2 Position;
    private Vector2 _velocity;

    public int Width  = 32;
    public int Height = 48;

    private const float Gravity   = 1800f;
    private const float MoveSpeed = 220f;
    private const float JumpForce = -620f;

    private bool _isOnGround = false;
    private KeyboardState _prevKeys;

    public PhaseState Phase { get; private set; } = PhaseState.Solid;

    private Texture2D _texture;
    private Texture2D _solidTexture;
    private Texture2D _ghostTexture;

    private static readonly Color SolidColor = new Color(55, 138, 221);
    private static readonly Color GhostColor = new Color(127, 119, 221);
    
    private Vector2 _spawnPoint;
    public bool IsDead { get; private set; } = false;

    public Player(GraphicsDevice gd, Vector2 startPosition, Texture2D solidTex = null, Texture2D ghostTex = null)
    {
        Position    = startPosition;
        _spawnPoint = startPosition;

        _solidTexture = solidTex ?? new Texture2D(gd, 1, 1);
        _ghostTexture = ghostTex ?? new Texture2D(gd, 1, 1);
        if (solidTex == null) _solidTexture.SetData(new[] { new Color(55, 138, 221) });
        if (ghostTex == null) _ghostTexture.SetData(new[] { new Color(127, 119, 221) });
    }

    public void Update(GameTime gameTime, Rectangle ground, List<Platform> platforms, List<PhaseWall> walls, int worldWidth = 3200)
{
    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    var keys = Keyboard.GetState();

    // Переключение фазы
    if (keys.IsKeyDown(Keys.Space) && _prevKeys.IsKeyUp(Keys.Space))
        Phase = Phase == PhaseState.Solid ? PhaseState.Ghost : PhaseState.Solid;

    // Горизонтальное движение
    _velocity.X = 0;
    if (keys.IsKeyDown(Keys.Left)  || keys.IsKeyDown(Keys.A)) _velocity.X = -MoveSpeed;
    if (keys.IsKeyDown(Keys.Right) || keys.IsKeyDown(Keys.D)) _velocity.X =  MoveSpeed;

    // Прыжок
    bool jumpJustPressed = (keys.IsKeyDown(Keys.W)  || keys.IsKeyDown(Keys.Up)) &&
                          !(_prevKeys.IsKeyDown(Keys.W) || _prevKeys.IsKeyDown(Keys.Up));
    if (jumpJustPressed && _isOnGround)
        _velocity.Y = JumpForce;

    // Гравитация
    _velocity.Y += Gravity * dt;

    // Разбиваем кадр на несколько подшагов чтобы не пролетать сквозь объекты
    const int steps = 5;
    float subDt = dt / steps;

    for (int i = 0; i < steps; i++)
    {
        // Шаг по X
        Position.X += _velocity.X * subDt;

        // Стены по X
        foreach (var wall in walls)
        {
            if (wall.Phase != Phase) continue;
            if (!Bounds.Intersects(wall.Bounds)) continue;

            int overlapLeft  = Bounds.Right      - wall.Bounds.Left;
            int overlapRight = wall.Bounds.Right - Bounds.Left;

            if (overlapLeft < overlapRight)
                Position.X = wall.Bounds.Left - Width;
            else
                Position.X = wall.Bounds.Right;

            _velocity.X = 0;
        }

        // Границы экрана по X
        if (Position.X < 0)                Position.X = 0;
        if (Position.X + Width > worldWidth) Position.X = worldWidth - Width;

        // Шаг по Y
        _isOnGround = false;
        Position.Y += _velocity.Y * subDt;

        // Земля
        if (Position.Y + Height >= ground.Top && _velocity.Y >= 0)
        {
            Position.Y  = ground.Top - Height;
            _velocity.Y = 0;
            _isOnGround = true;
        }

        // Платформы по Y
        foreach (var platform in platforms)
        {
            if (platform.Phase != Phase) continue;
            ResolveY(platform.Bounds);
        }

        // Стены по Y
        foreach (var wall in walls)
        {
            if (wall.Phase != Phase) continue;
            ResolveY(wall.Bounds);
        }
    }
    
    var groundCheck = new Rectangle((int)Position.X, (int)Position.Y + Height, Width, 2);

    if (groundCheck.Intersects(ground))
        _isOnGround = true;

    foreach (var platform in platforms)
    {
        if (platform.Phase != Phase) continue;
        if (groundCheck.Intersects(platform.Bounds))
            _isOnGround = true;
    }

    foreach (var wall in walls)
    {
        if (wall.Phase != Phase) continue;
        if (groundCheck.Intersects(wall.Bounds))
            _isOnGround = true;
    }

    _prevKeys = keys;
}

public void Respawn()
{
    Position  = _spawnPoint;
    _velocity = Vector2.Zero;
    IsDead    = false;
}

private void ResolveY(Rectangle rect)
{
    if (!Bounds.Intersects(rect)) return;

    int overlapTop    = Bounds.Bottom - rect.Top;
    int overlapBottom = rect.Bottom   - Bounds.Top;

    if (overlapTop <= overlapBottom)
    {
        if (_velocity.Y >= 0)
        {
            Position.Y  = rect.Top - Height;
            _velocity.Y = 0;
            _isOnGround = true;
        }
    }
    else
    {
        if (_velocity.Y < 0)
        {
            Position.Y  = rect.Bottom;
            _velocity.Y = 0;
        }
    }
}

public void Draw(SpriteBatch sb, Vector2 cameraOffset)
{
    var tex     = Phase == PhaseState.Solid ? _solidTexture : _ghostTexture;
    var drawPos = Position - cameraOffset;

    sb.Draw(tex,
        new Rectangle((int)drawPos.X, (int)drawPos.Y, Width, Height),
        Color.White);
}

    public Rectangle Bounds =>
        new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
}