using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Asteroids.LineEngine
{
    class Timer : GameComponent
    {
        private float m_FrameTime;
        private float m_SecondsTimer;

        public float SecondsTimer
        {
            get { return m_SecondsTimer; }
        }

        public Timer(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            m_FrameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_SecondsTimer += m_FrameTime;
        }

        public void ResetTimer()
        {
            m_SecondsTimer = 0;
        }
    }
}
