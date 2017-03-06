using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Engine.ParticleEmitter
{
    public class Settings
    {
        private int m_initialParticleCount = 200;           // number of particles
        private float m_particlesPerSecond = 100;        // number of particles per second
        private float m_minimumDirectionAngle = 0;       // min direction angle (in radians)
        // PiOver2 is strait up. Zero is to the Right. -PiOver2 is strait down.
        private float m_maximumDirectionAngle = Microsoft.Xna.Framework.MathHelper.TwoPi;     // max direction angle (in radians)
        private float m_minimumSpeed = 50.0f;               // min particle speed
        private float m_maximumSpeed = 100.0f;              // max particle speed
        private float m_minimumLifeTime = 1.0f;             // min life time of the particle
        private float m_maximumLifeTime = 2.5f;             // max life time of the particle
        private float m_minimumScale = 0.5f;                // min size of the particle
        private float m_maximumScale = 1.0f;                // max size of the particle
        private Texture2D texture;

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public int InitialParticleCount
        {
            get { return m_initialParticleCount; }
            set { m_initialParticleCount = value; }
        }

        public float ParticlesPerSecond
        {
            get { return m_particlesPerSecond; }
            set { m_particlesPerSecond = value; }
        }

        public float MinimumDirectionAngle
        {
            get { return m_minimumDirectionAngle; }
            set { m_minimumDirectionAngle = value; }
        }

        public float MaximumDirectionAngle
        {
            get { return m_maximumDirectionAngle; }
            set { m_maximumDirectionAngle = value; }
        }

        public float MinimumSpeed
        {
            get { return m_minimumSpeed; }
            set { m_minimumSpeed = value; }
        }

        public float MaximumSpeed
        {
            get { return m_maximumSpeed; }
            set { m_maximumSpeed = value; }
        }

        public float MinimumLifeTime
        {
            get { return m_minimumLifeTime; }
            set { m_minimumLifeTime = value; }
        }

        public float MaximumLifeTime
        {
            get { return m_maximumLifeTime; }
            set { m_maximumLifeTime = value; }
        }

        public float MinimumScale
        {
            get { return m_minimumScale; }
            set { m_minimumScale = value; }
        }

        public float MaximumScale
        {
            get { return m_maximumScale; }
            set { m_maximumScale = value; }
        }
    }
}
