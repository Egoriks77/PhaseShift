using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhaseShift.Entities;

public class LevelExit
{
    public Rectangle Bounds;
    private Texture2D _texture;

    public LevelExit(GraphicsDevice gd, Rectangle bounds, Texture2D texture = null)
    {
        Bounds = bounds;
        _texture = texture ?? new Texture2D(gd, 1, 1);
        if (texture == null)
            _texture.SetData(new[] { new Color(20, 180, 120) });
    }

    public void Draw(SpriteBatch sb, Vector2 cameraOffset)
    {
        var drawRect = new Rectangle(
            Bounds.X - (int)cameraOffset.X,
            Bounds.Y - (int)cameraOffset.Y,
            Bounds.Width, Bounds.Height);
        sb.Draw(_texture, drawRect, Color.White);
    }

    public bool Intersects(Rectangle playerBounds) => Bounds.Intersects(playerBounds);
}