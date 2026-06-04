using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhaseShift.Core;

public class ScreenManager
{
    private IScreen _current;

    public void SetScreen(IScreen screen) => _current = screen;

    public void Update(GameTime gameTime) => _current?.Update(gameTime);
    public void Draw(SpriteBatch sb)      => _current?.Draw(sb);
}