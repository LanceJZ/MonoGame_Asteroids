using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Rock : LineEngine.LineMesh
    {
        Player m_Player;
        int m_Points = 20;
        float m_Speed = 75;

        public Player Player { set => m_Player = value; }

        public Rock(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                base.Update(gameTime);
                CheckBorders();
                CheckCollusions();
            }
        }

        public void InitializeLineMesh()
        {
            Initialize();

            Vector3[] pointPosition = new Vector3[13];

            //Rock One
            pointPosition[0] = new Vector3(29, 15, 0);
            pointPosition[1] = new Vector3(15, 30, 0);
            pointPosition[2] = new Vector3(0, 22, 0);
            pointPosition[3] = new Vector3(-15, 30, 0);
            pointPosition[4] = new Vector3(-29, 15, 0);
            pointPosition[5] = new Vector3(-15, 07, 0);
            pointPosition[6] = new Vector3(-29, -07, 0);
            pointPosition[7] = new Vector3(-15, -30, 0);
            pointPosition[8] = new Vector3(7, -21, 0);
            pointPosition[9] = new Vector3(15, -30, 0);
            pointPosition[10] = new Vector3(29, -15, 0);
            pointPosition[11] = new Vector3(21, 0, 0);
            pointPosition[12] = new Vector3(29, 15, 0);

            InitializePoints(pointPosition);

            Radius = 30;
        }

        void CheckCollusions()
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_Player.Shots[i].Visible)
                {
                    if (CirclesIntersect(m_Player.Shots[i].Position, m_Player.Shots[i].Radius))
                    {
                        Hit = true;
                        m_Player.Shots[i].Visible = false;
                    }
                }
            }
        }

        public void Spawn(Vector3 position, float scale, float speed, int points, Player player)
        {
            ScalePercent = scale;
            Radius = Radius * scale;
            m_Points = points;
            m_Speed = speed;
            m_Player = player;
            Spawn(position);
        }

        public void Spawn(Vector3 position)
        {
            Position = position;
            Spawn();
        }

        public void Spawn()
        {
            Visible = true;
            SetRandomVelocity(m_Speed);
            GameOver = m_Player.GameOver;
        }
    }
}
