using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhaseShift.Core;

public interface IScreen
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch sb);
}