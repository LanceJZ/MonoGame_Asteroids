using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.LineEngine
{
    public interface IDrawComponent
    {
        void Draw(GameTime gametime);
    }
}
