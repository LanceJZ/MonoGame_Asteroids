using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids
{
    using Serv = VectorEngine.Services;
    using Timer = VectorEngine.Timer;

    public class Player : VectorEngine.PositionedObject
    {
        Game m_Game;
        Timer m_FlameTimer;
        Timer m_ThrustTimer;
        PlayerFlame m_Flame;
        PlayerShip m_Ship;
        List<PlayerShip> m_ShipLives;
        Shot[] m_Shots;
        UFO m_UFO;
        SoundEffect m_FireShot;
        SoundEffect m_Explode;
        SoundEffect m_Bonus;
        SoundEffect m_Thrust;
        LineExplode m_Explosion;
        Number m_ScoreHUD;
        Number m_HiScoreHUD;
        HighScores m_HighScoreList;
        int m_Score;
        int m_HiScore;
        int m_NextBonusLife;
        int m_BaseBonusLife = 5000;
        int m_Lives;
        bool m_ShotKeyDown = false;
        bool m_HyperKeyDown = false;
        bool m_Exploding = false;
        bool m_Spawn = true;
        bool m_CheckClear = false;
        bool m_GameOver = false;

        public Shot[] Shots
        {
            get
            {
                return m_Shots;
            }

            set
            {
                m_Shots = value;
            }
        }

        public UFO UFO
        {
            set
            {
                m_UFO = value;
            }
        }

        public bool Spawn
        {
            set
            {
                m_Spawn = value;
            }
        }

        public bool CheckClear
        {
            get
            {
                return m_CheckClear;
            }
        }

        public bool GameOver
        {
            get
            {
                return m_GameOver;
            }

            set
            {
                m_GameOver = value;
            }
        }

        public Player(Game game) : base(game)
        {
            m_Game = game;
            m_FlameTimer = new Timer(game);
            m_ThrustTimer = new Timer(game);
            m_Flame = new PlayerFlame(game);
            m_Ship = new PlayerShip(game);
            m_Shots = new Shot[4];
            m_ScoreHUD = new Number(game);
            m_ScoreHUD.Moveable = false;
            m_HiScoreHUD = new Number(game);
            m_HiScoreHUD.Moveable = false;
            m_ShipLives = new List<PlayerShip>();
            m_Explosion = new LineExplode(game);
            m_HighScoreList = new HighScores(game);

            AddChild(m_Flame, false, true);
            AddChild(m_Ship, true, true);

            for (int i = 0; i < 4; i++)
            {
                MakeShipLives(i);
            }

            for (int i = 0; i < 4; i++)
            {
                Shots[i] = new Shot(game);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_FlameTimer.Amount = 0.01666f;
        }

        /// <summary>
        /// Executed after initialization is complete
        /// </summary>
        public override void BeginRun()
        {
            Active = false;
            m_Flame.Active = false;
            Radius = m_Ship.Radius;
            ShipLivesDisplay();
            m_HighScoreList.BeginRun();
            m_HiScore = m_HighScoreList.HighScore;
            m_HiScoreHUD.ProcessNumber(m_HiScore, new Vector3(0, 440, 0), 8);
            m_ScoreHUD.ProcessNumber(m_Score, new Vector3(-400, 440, 0), 10);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_Exploding)
            {
                if (!m_Explosion.Active)
                {
                    m_Exploding = false;
                    m_CheckClear = true;
                }
            }
            else if (m_Spawn)
            {
                m_CheckClear = false;

                if (!GameOver)
                {
                    Active = true;
                    m_Spawn = false;
                }
            }

            if (Active)
            {
                if (Hit)
                {
                    LostLife();
                    Hit = false;
                }

                CheckBorders();
                KeyInput();
            }
        }

        public void SetScore(int add)
        {
            m_Score += add;

            m_ScoreHUD.ProcessNumber(m_Score, new Vector3(-400, 440, 0), 10);

            if (m_HiScore < m_Score)
            {
                m_HiScore = m_Score;
                m_HiScoreHUD.ProcessNumber(m_HiScore, new Vector3(0, 440, 0), 8);
            }

            if (m_Score > m_NextBonusLife)
            {
                m_Bonus.Play(0.15f, 0, 0);
                m_Lives++;
                m_NextBonusLife += 10000;
                ShipLivesDisplay();
            }
        }

        public void NewGame()
        {
            m_Lives = 4;
            m_Score = 0;
            m_NextBonusLife = m_BaseBonusLife;
            Active = true;
            ResetShip();
            SetScore(0);
            m_HighScoreList.NewGame();
            ShipLivesDisplay();
        }

        public void LoadSounds(SoundEffect fireshot, SoundEffect explode, SoundEffect bonus, SoundEffect thurst)
        {
            m_FireShot = fireshot;
            m_Explode = explode;
            m_Bonus = bonus;
            m_Thrust = thurst;

            m_ThrustTimer.Amount = (float)m_Thrust.Duration.TotalSeconds;
        }

        void MakeShipLives(int count)
        {
            m_ShipLives.Add(new PlayerShip(m_Game));
            m_ShipLives[count].Position = new Vector3((-count * 15) + -400, 400, 0);
            m_ShipLives[count].RotationInRadians = (float)Math.PI * 0.5f;
            m_ShipLives[count].Scale = 0.5f;
        }

        void Explode()
        {
            m_Flame.Active = false;
            Active = false;
            m_Explode.Play(0.5f, 0, 0);
            m_Explosion.Spawn(Position, Radius);
            m_Exploding = true;
        }

        void LostLife()
        {
            m_Lives--;
            Active = false;
            m_Spawn = false;

            if (m_Lives < 1)
            {
                GameOver = true;
                m_HighScoreList.GameOver(m_Score);
                m_HiScore = m_HighScoreList.HighScore;
            }

            ShipLivesDisplay();
            Explode();
            ResetShip();
        }

        void ShipLivesDisplay()
        {
            for (int i = 0; i < m_ShipLives.Count; i++)
            {
                m_ShipLives[i].Active = false;
            }

            if (m_Lives > m_ShipLives.Count)
            {
                MakeShipLives(m_Lives - 1);
            }

            for (int i = 0; i < m_Lives; i++)
            {
                m_ShipLives[i].Active = true;
            }
        }

        void ResetShip()
        {
            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            Acceleration = Vector3.Zero;
            RotationInRadians = Serv.RandomMinMax(0, (float)MathHelper.TwoPi);
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

                if (Active)
                {
                    m_Flame.Active = !m_Flame.Active;
                }
                else
                    m_Flame.Active = false;
            }

            if (m_ThrustTimer.Seconds > m_ThrustTimer.Amount)
            {
                m_ThrustTimer.Reset();

                m_Thrust.Play(0.9f, 0, 0);
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
                Acceleration = Serv.SetVelocity(RotationInRadians, thrustAmount);
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
                if (!Shots[shotCount].Active)
                {
                    m_FireShot.Play(0.15f, 0, 0);

                    float speed = 500;
                    Vector3 offset = Serv.SetVelocity(RotationInRadians, 11);
                    Vector3 direction = Serv.SetVelocity(RotationInRadians, speed);

                    Shots[shotCount].Spawn(Position + offset, Velocity * 0.75f + direction, 1.55f);
                    break;
                }
            }
        }

        void Hyperspace()
        {
            Position.X = Serv.RandomMinMax(-Serv.WindowWidth * 0.5F, Serv.WindowWidth * 0.5F);
            Position.Y = Serv.RandomMinMax(-Serv.WindowHeight * 0.5F, Serv.WindowHeight * 0.5F);
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
                m_Flame.Active = false;
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
    }
}
