#region Using
using System;
using Microsoft.Xna.Framework;
#endregion

namespace Asteroids.LineEngine
{
    public class PositionedObject : DrawableGameComponent
    {
        #region Fields
        private float m_FrameTime;
        // Doing these as fields is almost twice as fast as if they were properties. 
        // Also, sense XYZ are fields they do not get data binned as a property.
        Game m_Game;
        public Vector3 Position;
        public Vector3 Acceleration;
        public Vector3 Velocity;
        float m_RotationInRadians;
        float m_ScalePercent = 1;
        float m_RotationVelocity;
        float m_RotationAcceleration;
        float m_MaxWidth;
        float m_MaxHeight;
        float m_Radius;
        bool m_Hit = false;
        bool m_ExplosionActive = false;
        bool m_Pause = false;
        bool m_GameOver = false;
        bool m_Moveable = true;
        #endregion
        #region Properties
        public float FrameTime { get { return m_FrameTime; } }

        public float RotationInRadians
        {
            get
            {
                return m_RotationInRadians;
            }

            set
            {
                m_RotationInRadians = value;
            }
        }

        public float ScalePercent
        {
            get
            {
                return m_ScalePercent;
            }

            set
            {
                m_ScalePercent = value;
            }
        }

        public float RotationVelocity
        {
            get
            {
                return m_RotationVelocity;
            }

            set
            {
                m_RotationVelocity = value;
            }
        }

        public float RotationAcceleration
        {
            get
            {
                return m_RotationAcceleration;
            }

            set
            {
                m_RotationAcceleration = value;
            }
        }

        public float Radius
        {
            get
            {
                return m_Radius;
            }

            set
            {
                m_Radius = value;
            }
        }

        public bool Hit
        {
            get
            {
                return m_Hit;
            }

            set
            {
                m_Hit = value;
            }
        }

        public bool ExplosionActive
        {
            get
            {
                return m_ExplosionActive;
            }

            set
            {
                m_ExplosionActive = value;
            }
        }

        public bool Pause
        {
            get
            {
                return m_Pause;
            }

            set
            {
                m_Pause = value;
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

        public bool Moveable
        {
            get
            {
                return m_Moveable;
            }

            set
            {
                m_Moveable = value;
            }
        }

        #endregion
        #region Constructor
        /// <summary>
        /// This is the constructor that gets the Positioned Object ready for use and adds it to the Drawable Components list.
        /// </summary>
        /// <param name="game">The game class</param>
        public PositionedObject(Game game) : base(game)
        {
            game.Components.Add(this);
            m_Game = game;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Allows the game component to be updated.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Visible && Moveable)
            {
                base.Update(gameTime);
                m_FrameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Velocity += Acceleration * m_FrameTime;
                Position += Velocity * m_FrameTime;
                RotationVelocity += RotationAcceleration * m_FrameTime;
                RotationInRadians += RotationVelocity * m_FrameTime;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_MaxWidth = Services.WindowWidth * 0.5f;
            m_MaxHeight = Services.WindowHeight * 0.5f;
        }

        public void Remove()
        {
            m_Game.Components.Remove(this);
        }

        public virtual void CheckBorders()
        {
            if (Position.X > m_MaxWidth)
                Position.X = -m_MaxWidth;

            if (Position.X < -m_MaxWidth)
                Position.X = m_MaxWidth;

            if (Position.Y > m_MaxHeight)
                Position.Y = -m_MaxHeight;

            if (Position.Y < -m_MaxHeight)
                Position.Y = m_MaxHeight;
        }

        public bool CirclesIntersect(Vector3 Target, float TargetRadius)
        {
            float distanceX = Target.X - Position.X;
            float distanceY = Target.Y - Position.Y;
            float radius = Radius + TargetRadius;

            if ((distanceX * distanceX) + (distanceY * distanceY) < radius * radius)
                return true;

            return false;
        }
        #endregion
    }
}
