using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Shot : LineEngine.LineMesh
    {
        LineEngine.Timer m_Timer;
        float m_TimerAmount = 0;

        public Shot(Game game) : base(game)
        {
            m_Timer = new LineEngine.Timer(game);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CheckBorders();

            if (m_Timer.SecondsTimer > m_TimerAmount)
            {
                Visible = false;
            }
        }

        public void Spawn(Vector3 position, Vector3 velecity, float timer)
        {
            m_TimerAmount = timer;
            Position = position;
            Velocity = velecity;
            Visible = true;
            m_Timer.ResetTimer();
        }

        public void Reset()
        {
            Visible = false;
        }

        public void InitializeLineMesh()
        {
            Initialize();

            Vector3[] pointPosition = new Vector3[4];

            pointPosition[0] = new Vector3(-0.5f, 0.5f, 0);
            pointPosition[1] = new Vector3(0.5f, -0.5f, 0);
            pointPosition[2] = new Vector3(0.5f, 0.5f, 0);
            pointPosition[3] = new Vector3(-0.5f, -0.5f, 0);

            InitializePoints(pointPosition);

            Visible = false;
            Radius = 0.5f;
        }
    }
}
