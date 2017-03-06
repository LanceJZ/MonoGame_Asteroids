using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Engine.ParticleEmitter
{
    public class Emitter : PositionedObject
    {
        #region Fields
        private List<Particle> particleList;             // The list that will contain of all the particles.
        private float elapsedTime;                       // The elapsed time since the last update.
        private Settings settings;                       // The particle settings class.
        private bool isBurst;                            // If the emitter is in burst mode. Use this to turn on/off constant mode too.
        private bool endBurst;                           // End of burst, when used in burst mode. Set this to false to start a burst.
        #endregion
        #region Properties
        public Settings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public bool IsBurst
        {
            get
            {
                return isBurst;
            }
            set
            {
                isBurst = value;
            }
        }

        public bool EndBurst
        {
            get
            {
                return endBurst;
            }
            set
            {
                endBurst = value;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Particle Emitter
        /// </summary>
        /// <param name="game">Reference to the current instance of the game.</param>
        /// <param name="textureFileName">The file name of the texture to be used on the particle.</param>
        public Emitter(Game game, string textureFileName)
            : base(game)
        {
            // Create instance of Settings for the emitter.
            settings = new Settings();
            // Load particle model, and put the texture on it.
            particleList = new List<Particle>(settings.InitialParticleCount);
            settings.Texture = Game.Content.Load<Texture2D>(textureFileName);

            for (int i = 0; i < settings.InitialParticleCount; ++i)
            {
                particleList.Add(new Particle(game));
                particleList[particleList.Count - 1].Texture = settings.Texture;
            }
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();
        }

        public void SetupParticles()
        {
            for (int i = 0; i < particleList.Count - 1; ++i)
            {
                particleList[i].ApplyTexture();
            }
        }

        /// <summary>
        /// Update the particle system
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Here we update the particles based on whether the
            // particle system is in Burst mode or not.
            if (IsBurst)
            {
                UpdateParticleBurst(gameTime);
            }
            else
            {
                UpdateParticlesPerSec(gameTime);
            }

            base.Update(gameTime);
        }
        #endregion
        #region Protected Methods
        // clean up a little
        protected override void Dispose(bool disposing)
        {
            settings.Texture.Dispose();
            particleList.Clear();

            base.Dispose(disposing);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// calculate a new velocity for a newly initialized particle
        /// </summary>
        /// <returns>Vector2</returns>
        private Vector2 GetNewVelocity()
        {
            // get a random angle between min direction angle and max direction angle in radians.
            float angle = Services.RandomMinMax(settings.MinimumDirectionAngle, settings.MaximumDirectionAngle);

            // return the new velocity based on the angle in radians
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /// <summary>
        /// Update the particles based on the number of particles to generate per second
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        private void UpdateParticlesPerSec(GameTime gameTime)
        {
            // Grab the elapsed time since the last update.
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate the approximate number of particle to add this iteration.
            float particlePerSec = settings.ParticlesPerSecond * elapsedTime;

            foreach (Particle particleOn in particleList)
            {
                // Add a new particle if we still have remaining particles to add.
                if (particlePerSec > 0)
                {
                    if (particleOn.Visible == false)
                    {
                        InitializeParticle(particleOn);
                        --particlePerSec;
                    }
                }
            }
        }

        /// <summary>
        /// Update the particle system as just a single explosion
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        private void UpdateParticleBurst(GameTime gameTime)
        {
            // here we want to only initialize new particles until all the 
            // particles in the List are active. Once all particles are
            // active, the burst is complete so do not re-initialize any
            // new particles, until m_settings.EndBurst == false, again
            foreach (Particle particleOn in particleList)
            {
                if (particleOn.Visible == false)
                {
                    if (!EndBurst)
                        InitializeParticle(particleOn);
                }
                else
                {
                    EndBurst = true;
                    Enabled = false;
                }
            }
        }

        /// <summary>
        /// Initialize a particle to its default values
        /// </summary>
        /// <param name="particle">Particle to initialize</param>
        private void InitializeParticle(Particle particle)
        {
            // Get new particle velocity (based on min and max direction angles) Relative to the camera.
            Vector2 newVelocity = GetNewVelocity();

            // Grab a new speed between MinimumSpeed and MaximumSpeed
            float speed = Services.RandomMinMax(settings.MinimumSpeed, settings.MaximumSpeed);

            // Add the speed to the velocity, it could take more CPU to multiply a vector 3.
            newVelocity *= speed;

            // Translate that into a Vector 3.
            Vector3 velocity = new Vector3(newVelocity.X, newVelocity.Y, 0);

            // Grab a new amount of time the particle should remain alive
            float lifeTime = Services.RandomMinMax(settings.MinimumLifeTime, settings.MaximumLifeTime);

            // Grab a new size for the particle 
            float scale = Services.RandomMinMax(settings.MinimumScale, settings.MaximumScale);

            // initialize the particle with the new settings
            particle.Activate(Position, velocity, lifeTime, scale, true);
        }
        #endregion
    }
}
