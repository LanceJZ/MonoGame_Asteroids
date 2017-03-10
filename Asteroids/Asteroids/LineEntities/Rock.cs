using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    using Serv = LineEngine.Services;

    class Rock : LineEngine.LineMesh
    {
        Player m_Player;
        UFO m_UFO;
        Explode m_Explosion;
        int m_Points = 20;
        float m_Speed = 75;
        bool m_ExplosionDone;

        public Player Player
        {
            get
            {
                return m_Player;
            }

            set
            {
                m_Player = value;
            }
        }

        public UFO UFO
        {
            get
            {
                return m_UFO;
            }

            set
            {
                m_UFO = value;
            }
        }

        public bool ExplosionDone
        {
            get
            {
                return m_Explosion.Active;
            }
        }

        public Rock(Game game) : base(game)
        {
            m_Explosion = new Explode(game);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeLineMesh();
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

        void InitializeLineMesh()
        {
            int rockType = (int)Serv.RandomMinMax(0, 3.99f);

            switch (rockType)
            {
                case 0:
                    RockOne();
                    break;
                case 1:
                    RockTwo();
                    break;
                case 2:
                    RockThree();
                    break;
                case 3:
                    RockFour();
                    break;
            }
        }

        void CheckCollusions()
        {
            if (Player.Visible)
            {
                if (CirclesIntersect(Player.Position, Player.Radius))
                {
                    Explode();
                    Player.Hit = true;
                    Player.SetScore(m_Points);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (Player.Shots[i].Visible)
                {
                    if (CirclesIntersect(Player.Shots[i].Position, Player.Shots[i].Radius))
                    {
                        Explode();
                        Player.Shots[i].Visible = false;
                        Player.SetScore(m_Points);
                    }
                }
            }

            if (UFO.Visible)
            {
                if (CirclesIntersect(UFO.Position, UFO.Radius))
                {
                    Explode();
                    UFO.Explode();
                }

                if (UFO.Shot.Visible)
                {
                    if (CirclesIntersect(UFO.Shot.Position, UFO.Shot.Radius))
                    {
                        Explode();
                        UFO.Shot.Visible = false;
                    }
                }
            }
        }

        public void Spawn(Vector3 position, float scale, float speed, int points, Player player, UFO ufo)
        {
            ScalePercent = scale;
            Radius = Radius * scale;
            m_Points = points;
            m_Speed = speed;
            Player = player;
            UFO = ufo;
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
            Velocity = Serv.SetRandomVelocity(m_Speed);
            GameOver = Player.GameOver;
        }

        void Explode()
        {
            m_Explosion.Spawn(Position, Radius);
            Hit = true;
        }

        void RockOne()
        {
            Vector3[] pointPosition = new Vector3[13];

            //Rock One
            pointPosition[0] = new Vector3(-34, 17.6f, 0);
            pointPosition[1] = new Vector3(-17.6f, 35.2f, 0);
            pointPosition[2] = new Vector3(0, 25.8f, 0);
            pointPosition[3] = new Vector3(17.6f, 35.2f, 0);
            pointPosition[4] = new Vector3(24, 17.6f, 0);
            pointPosition[5] = new Vector3(17.6f, 8.2f, 0);
            pointPosition[6] = new Vector3(34, -8.2f, 0);
            pointPosition[7] = new Vector3(17.6f, -35.2f, 0);
            pointPosition[8] = new Vector3(-8.2f, -21, 0);
            pointPosition[9] = new Vector3(-17.6f, -35.2f, 0);
            pointPosition[10] = new Vector3(-34, -17.6f, 0);
            pointPosition[11] = new Vector3(-24.7f, 0, 0);
            pointPosition[12] = new Vector3(-34, 17.6f, 0);

            InitializePoints(pointPosition);

            Radius = 35.2f;
        }

        void RockTwo()
        {
            Vector3[] pointPosition = new Vector3[11];

            //Rock One
            pointPosition[0] = new Vector3(-34, 17.6f, 0);
            pointPosition[1] = new Vector3(-16.4f, 34, 0);
            pointPosition[2] = new Vector3(0, 17.6f, 0);
            pointPosition[3] = new Vector3(17.6f, 34, 0);
            pointPosition[4] = new Vector3(34, 17.6f, 0);
            pointPosition[5] = new Vector3(25.8f, 0, 0);
            pointPosition[6] = new Vector3(34, -17.6f, 0);
            pointPosition[7] = new Vector3(8.2f, -34, 0);
            pointPosition[8] = new Vector3(-16.4f, -34, 0);
            pointPosition[9] = new Vector3(-34, -16.4f, 0);
            pointPosition[10] = new Vector3(-34, 17.6f, 0);

            InitializePoints(pointPosition);

            Radius = 34;
        }

        void RockThree()
        {
            Vector3[] pointPosition = new Vector3[13];

            //Rock One
            pointPosition[0] = new Vector3(-34, 17.6f, 0);
            pointPosition[1] = new Vector3(-8.2f, 17.6f, 0);
            pointPosition[2] = new Vector3(-18.8f, 34, 0);
            pointPosition[3] = new Vector3(9.4f, 34, 0);
            pointPosition[4] = new Vector3(34, 17.6f, 0);
            pointPosition[5] = new Vector3(34, 9.4f, 0);
            pointPosition[6] = new Vector3(9.4f, 0, 0);
            pointPosition[7] = new Vector3(34, -16.4f, 0);
            pointPosition[8] = new Vector3(16.4f, -32.9f, 0);
            pointPosition[9] = new Vector3(8.2f, -24.7f, 0);
            pointPosition[10] = new Vector3(-17.6f, -24, 0);
            pointPosition[11] = new Vector3(-34, -9.4f, 0);
            pointPosition[12] = new Vector3(-34, 17.6f, 0);

            InitializePoints(pointPosition);

            Radius = 34;
        }

        void RockFour()
        {
            Vector3[] pointPosition = new Vector3[13];

            //Rock One
            pointPosition[0] = new Vector3(-34, 9.4f, 0);
            pointPosition[1] = new Vector3(-7, 34, 0);
            pointPosition[2] = new Vector3(17.6f, 34, 0);
            pointPosition[3] = new Vector3(35.2f, 8.2f, 0);
            pointPosition[4] = new Vector3(35.2f, -8.2f, 0);
            pointPosition[5] = new Vector3(18.8f, -34, 0);
            pointPosition[6] = new Vector3(16.4f, -34, 0);
            pointPosition[7] = new Vector3(0, -34, 0);
            pointPosition[8] = new Vector3(0, -9.4f, 0);
            pointPosition[9] = new Vector3(-16.4f, -32.9f, 0);
            pointPosition[10] = new Vector3(-34, -8.2f, 0);
            pointPosition[11] = new Vector3(-17.6f, 0, 0);
            pointPosition[12] = new Vector3(-34, 9.4f, 0);

            InitializePoints(pointPosition);

            Radius = 35.2f;
        }
    }
}
