using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhaseShift.Entities;

public enum SpikeDirection { Up, Down, Left, Right }

public class Spike
{
    public Rectangle Bounds;
    private Texture2D _texture;
    private SpikeDirection _direction;

    public Spike(GraphicsDevice gd, Rectangle bounds, Texture2D texture,
        SpikeDirection direction = SpikeDirection.Up)
    {
        Bounds     = bounds;
        _texture   = texture;
        _direction = direction;
    }

    public void Draw(SpriteBatch sb, Vector2 cameraOffset)
    {
        var drawRect = new Rectangle(
            Bounds.X - (int)cameraOffset.X,
            Bounds.Y - (int)cameraOffset.Y,
            Bounds.Width, Bounds.Height);

        float rotation = _direction switch
        {
            SpikeDirection.Down  => MathHelper.Pi,
            SpikeDirection.Left  => -MathHelper.PiOver2,
            SpikeDirection.Right => MathHelper.PiOver2,
            _                    => 0f
        };

        Vector2 origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        Vector2 center = new Vector2(
            drawRect.X + drawRect.Width  / 2f,
            drawRect.Y + drawRect.Height / 2f);

        sb.Draw(_texture, center, null, Color.White,
            rotation, origin, new Vector2(
                drawRect.Width  / (float)_texture.Width,
                drawRect.Height / (float)_texture.Height),
            SpriteEffects.None, 0f);
    }

    public bool Intersects(Rectangle playerBounds)
    {
        var shrunk = new Rectangle(
            Bounds.X + 3, Bounds.Y + 3,
            Bounds.Width - 6, Bounds.Height - 6);
        return shrunk.Intersects(playerBounds);
    }
}