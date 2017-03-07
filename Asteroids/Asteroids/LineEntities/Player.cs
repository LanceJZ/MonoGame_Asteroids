using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    using Serv = LineEngine.Services;
    using Timer = LineEngine.Timer;

    public class Player : LineEngine.LineMesh
    {
        Timer m_FlameTimer;
        PlayerFlame m_Flame;
        Shot[] m_Shots;
        bool m_ShotKeyDown = false;
        bool m_HyperKeyDown = false;

        public Shot[] Shots { get => m_Shots; set => m_Shots = value; }

        public Player(Game game) : base(game)
        {
            m_FlameTimer = new Timer(game);
            m_Flame = new PlayerFlame(game);
            Shots = new Shot[4];

            for (int i = 0; i < 4; i++)
            {
                Shots[i] = new Shot(game);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeLineMesh();
            m_FlameTimer.Amount = 0.06f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_Flame.Position = Position;
            m_Flame.RotationInRadians = RotationInRadians;
            CheckBorders();
            KeyInput();
        }

        void ThrustOn()
        {
            float maxPerSecond = 820;
            float thrustAmount = 225;
            float testX;
            float testY;

            if (m_FlameTimer.Seconds > m_FlameTimer.Amount)
            {
                m_FlameTimer.Reset();

                if (m_Flame.Visible)
                    m_Flame.Visible = false;
                else
                    m_Flame.Visible = true;
            }

            if (Velocity.Y < 0)
                testY = -Velocity.Y;
            else
                testY = Velocity.Y;

            if (Velocity.X < 0)
                testX = -Velocity.X;
            else
                testX = Velocity.X;

            if (testX + testY < maxPerSecond)
            {
                Acceleration = SetVelocity(RotationInRadians, thrustAmount);
            }
            else
            {
                ThrustOff();
            }
        }

        void ThrustOff()
        {
            float Deceration = 0.1f;
            Acceleration = -Velocity * Deceration;
        }

        void FireShot()
        {
            for (int shotCount = 0; shotCount < 4; shotCount++)
            {
                if (!Shots[shotCount].Visible)
                {
                    float speed = 500;
                    Vector3 offset = SetVelocity(RotationInRadians, 11);
                    Vector3 direction = SetVelocity(RotationInRadians, speed);

                    Shots[shotCount].Spawn(Position + offset, Velocity * 0.75f + direction, 1.55f);
                    break;
                }
            }
        }

        void Hyperspace()
        {
            Position.X = Serv.RandomMinMax(-Serv.WindowWidth, Serv.WindowWidth);
            Position.Y = Serv.RandomMinMax(-Serv.WindowHeight, Serv.WindowHeight);
            Velocity = new Vector3(0);
            Acceleration = new Vector3(0);
        }

        void KeyInput()
        {
            float rotationAmound = MathHelper.Pi;

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ThrustOn();
            }
            else
            {
                m_Flame.Visible = false;
                ThrustOff();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                RotationVelocity = rotationAmound;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RotationVelocity = -rotationAmound;
            }
            else
                RotationVelocity = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (!m_ShotKeyDown)
                {
                    m_ShotKeyDown = true;
                    FireShot();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.LeftControl) && Keyboard.GetState().IsKeyUp(Keys.Space) && m_ShotKeyDown)
            {
                m_ShotKeyDown = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.RightShift) && !m_HyperKeyDown)
            {
                Hyperspace();
                m_HyperKeyDown = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.RightShift))
            {
                m_HyperKeyDown = false;
            }
        }

        void InitializeLineMesh()
        {
            Vector3[] pointPosition = new Vector3[6];

            pointPosition[0] = new Vector3(-13.5f, 8f, 0);//Top back tip.
            pointPosition[1] = new Vector3(13.5f, 0, 0);//Nose pointing to the left of screen.
            pointPosition[2] = new Vector3(-13.5f, -9.4f, 0);//Bottom back tip.
            pointPosition[3] = new Vector3(-10.6f, -4.7f, 0);//Bottom inside back.
            pointPosition[4] = new Vector3(-10.6f, 4.7f, 0);//Top inside back.
            pointPosition[5] = new Vector3(-13.5f, 9.4f, 0);//Top Back Tip.

            InitializePoints(pointPosition);

            Radius = 11.5f;

            RotationInRadians = Serv.RandomMinMax(0, (float)Math.PI);
        }
    }
}
