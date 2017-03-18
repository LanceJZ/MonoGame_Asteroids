using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.VectorEngine
{
    public interface IDrawComponent
    {
        void Draw(GameTime gametime);
    }
}
