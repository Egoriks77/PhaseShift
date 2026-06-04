using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhaseShift.Entities;

public class PhaseWall
{
    public Rectangle Bounds;
    public PhaseState Phase;

    private Texture2D _texture;

    private static readonly Color SolidColor = new Color(37, 99, 168);
    private static readonly Color GhostColor = new Color(91, 79, 199);

    public PhaseWall(GraphicsDevice gd, Rectangle bounds, PhaseState phase, Texture2D texture = null)
    {
        Bounds = bounds;
        Phase  = phase;
        _texture = texture ?? new Texture2D(gd, 1, 1);
        if (texture == null)
        {
            _texture.SetData(new[] { Phase == PhaseState.Solid
                ? new Color(37, 99, 168)
                : new Color(91, 79, 199) });
        }
    }

    public void Draw(SpriteBatch sb, PhaseState playerPhase, Vector2 cameraOffset)
    {
        var drawRect = new Rectangle(
            Bounds.X - (int)cameraOffset.X,
            Bounds.Y - (int)cameraOffset.Y,
            Bounds.Width, Bounds.Height);

        float alpha = Phase == playerPhase ? 1.0f : 0.3f;
        sb.Draw(_texture, drawRect, Color.White * alpha);
    }
}