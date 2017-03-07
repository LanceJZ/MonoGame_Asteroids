using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    using Serv = LineEngine.Services;
    using Timer = LineEngine.Timer;
    /// <summary>
    /// As the player's score increases, the angle range of the shots from the small saucer diminishes
    /// until the saucer fires extremely accurately.
    /// The small saucer will fire immediately when spawned. (Revision 3 of orginal arcade.)
    /// </summary>
    public class UFO : LineEngine.LineMesh
    {
        Player m_Player;
        Shot m_Shot;
        Timer m_ShotTimer;
        Timer m_VectorTimer;
        float m_Speed = 66;
        int m_Points;
        int m_PlayerScore;
        bool m_SmallSoucer;
        bool m_Done;

        public int PlayerScore { set => m_PlayerScore = value; }
        public bool Done { get => m_Done; set => m_Done = value; }
        public Shot Shot { get => m_Shot; set => m_Shot = value; }

        public UFO(Game game) : base(game)
        {
            m_ShotTimer = new Timer(game);
            m_VectorTimer = new Timer(game);
            Shot = new Shot(game);
        }

        public void Initialize(Player player)
        {
            m_Player = player;
            Visible = false;
            m_ShotTimer.Amount = 2.75f;
            m_VectorTimer.Amount = 3.15f;
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeLineMesh();

            Position.X = Serv.WindowWidth;
        }

        void InitializeLineMesh()
        {
            Vector3[] pointPosition = new Vector3[12];

            pointPosition[0] = new Vector3(8.2f, 4.7f, 0);// Upper right
            pointPosition[1] = new Vector3(22.3f, -4.7f, 0);// Lower Left
            pointPosition[2] = new Vector3(9.4f, -12.9f, 0);// Bottom Right
            pointPosition[3] = new Vector3(-9.4f, -12.9f, 0);// Bottom Left
            pointPosition[4] = new Vector3(-22.3f, -4.7f, 0);// Upper Left
            pointPosition[5] = new Vector3(-8.2f, 4.7f, 0);// Lower left
            pointPosition[6] = new Vector3(-3.5f, 12.9f, 0);// Left Top
            pointPosition[7] = new Vector3(3.5f, 12.9f, 0);// Right Top
            pointPosition[8] = new Vector3(8.2f, 4.7f, 0); // Upper right
            pointPosition[9] = new Vector3(-8.2f, 4.7f, 0);// Upper left
            pointPosition[10] = new Vector3(-22.3f, -4.7f, 0);// Lower Left
            pointPosition[11] = new Vector3(22.3f, -4.7f, 0);// Lower Right

            InitializePoints(pointPosition);
            Radius = 19;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Visible)
            {

                if (Position.X > Serv.WindowWidth * 0.5f || Position.X < -Serv.WindowWidth * 0.5f)
                {
                    m_Done = true;
                }

                CheckBorders();
                TimeToChangeVectorYet();
                TimeToShotYet();
            }
        }

        public void Spawn(int SpawnCount, int Wave)
        {
            Visible = true;
            m_Done = false;
            Hit = false;
            m_ShotTimer.Reset();
            m_VectorTimer.Reset();

            float spawnPercent = (float)(Math.Pow(0.915, (SpawnCount) / (Wave + 1)));

            if (Serv.RandomMinMax(0, 99) < (m_PlayerScore / 400) + (spawnPercent * 100))
            {
                m_SmallSoucer = false;
                m_Points = 200;
                ScalePercent = 1;
            }
            else
            {
                m_SmallSoucer = true;
                m_Points = 1000;
                ScalePercent = 0.5f;
            }

            Position.Y = Serv.RandomMinMax(-Serv.WindowHeight * 0.25f, Serv.WindowHeight * 0.25f);

            if (Serv.RandomMinMax(0, 10) > 5)
            {
                Position.X = -Serv.WindowWidth * 0.5f + 1;
                Velocity.X = m_Speed;
            }
            else
            {
                Position.X = Serv.WindowWidth * 0.5f - 1;
                Velocity.X = -m_Speed;
            }            
        }

        void TimeToShotYet()
        {
            if (m_ShotTimer.Seconds > m_ShotTimer.Amount)
            {
                FireShot();
            }
        }

        void TimeToChangeVectorYet()
        {
            if (m_VectorTimer.Seconds > m_VectorTimer.Amount)
            {
                ChangeVector();
            }
        }

        void FireShot()
        {
            //if (!m_GameOver)
            //    m_FireSoundInstance.Play();

            m_ShotTimer.Reset();
            float speed = 400;
            float rad = 0;

            if (!m_SmallSoucer)
                rad = RandomRadian();
            else
            {
                //Adjust according to score.
                rad = AngleFromVectors(Position, m_Player.Position) + Serv.RandomMinMax(-0.1f, 0.1f);
            }

            Vector3 dir = SetVelocity(rad, speed);
            Vector3 offset = SetVelocity(rad, Radius);
            Shot.Spawn(Position + offset, dir + Velocity * 0.25f, 1.15f);
        }

        void ChangeVector()
        {
            m_VectorTimer.Reset();
            float vChange = Serv.RandomMinMax(0, 9);

            if (vChange < 5)
            {
                if ((int)Velocity.Y == 0 && vChange < 2.5)
                    Velocity.Y = m_Speed;
                else if ((int)Velocity.Y == 0)
                    Velocity.Y = -m_Speed;
                else
                    Velocity.Y = 0;
            }
        }
    }
}
