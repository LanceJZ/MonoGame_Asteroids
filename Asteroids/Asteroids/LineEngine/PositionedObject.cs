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

        public float FrameTime { get { return m_FrameTime; } }
        public float RotationInRadians { get => m_RotationInRadians; set => m_RotationInRadians = value; }
        public float ScalePercent { get => m_ScalePercent; set => m_ScalePercent = value; }
        public float RotationVelocity { get => m_RotationVelocity; set => m_RotationVelocity = value; }
        public float RotationAcceleration { get => m_RotationAcceleration; set => m_RotationAcceleration = value; }
        public float Radius { get => m_Radius; set => m_Radius = value; }
        public bool Hit { get => m_Hit; set => m_Hit = value; }
        public bool Pause { get => m_Pause; set => m_Pause = value; }
        public bool GameOver { get => m_GameOver; set => m_GameOver = value; }
        public bool ExplosionActive { get => m_ExplosionActive;}
        #endregion
        #region Constructor
        /// <summary>
        /// This gets the Positioned Object ready for use, initializing all the fields.
        /// </summary>
        /// <param name="game">The game class</param>
        public PositionedObject(Game game) : base(game)
        {
            game.Components.Add(this);
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Allows the game component to be updated.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Visible)
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

        public float RandomRadian()
        {
            return Services.RandomMinMax(0, (float)Math.PI * 2);
        }

        /// <summary>
        /// Returns a Vector3 direction of travel from angle and magnitude.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public static Vector3 SetVelocity(float angle, float magnitude)
        {
            Vector3 Vector = new Vector3(0);
            Vector.Y = (float)(Math.Sin(angle) * magnitude);
            Vector.X = (float)(Math.Cos(angle) * magnitude);
            return Vector;
        }

        public void SetRandomVelocity(float speed)
        {
            float rad = RandomRadian();
            float amt = Services.RandomMinMax(speed * 0.15f, speed);
            Velocity = new Vector3((float)Math.Cos(rad) * amt, (float)Math.Sin(rad) * amt, 0);
        }

        public void SetRandomVelocity(float speed, float radianDirection)
        {            
            float amt = Services.RandomMinMax(speed * 0.15f, speed);
            Velocity = new Vector3((float)Math.Cos(radianDirection) * amt, (float)Math.Sin(radianDirection) * amt, 0);
        }

        public void SetRandomEdge()
        {
            Position.X = Services.WindowWidth * 0.5f;
            Position.Y = Services.RandomMinMax(-Services.WindowHeight * 0.45f, Services.WindowHeight * 0.45f);
        }

        /// <summary>
        /// Returns a float of the angle in radians derived from two Vector3 passed into it, using only the X and Y.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        public static float AngleFromVectors(Vector3 origin, Vector3 target)
        {
            return (float)(Math.Atan2(target.Y - origin.Y, target.X - origin.X));
        }
        #endregion
    }
}
