using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Engine.ParticleEmitter
{
    public class Particle : PositionedObject
    {
        #region Fields
        private float timeToDie;    // How long this particle with live, in seconds. At what time the particle dies.
        private float currentTime;   // The length of time this particle is running.
        private float lifeTime; // This is how long it lasts.
        private float birthTime; // This is the time the particle was created.
        private int counter;
        private Texture2D texture;
        private float alpha;
        private float remainingLife;
        // Vertex and Basic Effect:
        VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;
        BasicEffect basicEffect;
        // Matrix Identity:
        private Matrix worldTranslate = Matrix.Identity;
        private Matrix worldRotate = Matrix.Identity;
        private Matrix worldScale = Matrix.CreateScale(1);
        private Matrix worldMatrix = Matrix.Identity;

        #endregion
        #region Properties
        public float LifeTime
        {
            get { return timeToDie; }
            set { timeToDie = value; }
        }

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
        #endregion
        #region Constructor
        public Particle(Game game)
            : base(game)
        {
            Visible = false;
            Enabled = false;
            // Initialize Vertex Position Texture
            verts = new VertexPositionTexture[4];
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Initialize the particle
        /// </summary>
        /// <param name="position">Initial position of the particle.</param>
        /// <param name="velocity">Initial velocity of the particle.</param>
        /// <param name="lifeTime">How long the particle will live.</param>
        /// <param name="scale">How big will the particle be. This is based on the percent of one unit.</param>
        /// <param name="active">Is the particle active (or alive)?</param>
        public void Activate(Vector3 position, Vector3 velocity, float lifetime, float scale, bool active)
        {
            Position = position;
            Velocity = velocity;
            currentTime = TotalSeconds;
            lifeTime = lifetime;
            birthTime = currentTime;
            timeToDie = lifetime + currentTime;
            ScalePercent = new Vector3(scale);
            Enabled = active;
            Visible = active;
            counter = 0;


            SetupVerts();
        }

        public override void Initialize()
        {
            base.Initialize();
            // Initialize the vertex buffer
            vertexBuffer = new VertexBuffer(Services.Graphics, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
            // Initialize the BasicEffect
            basicEffect = new BasicEffect(Services.Graphics);

            ApplyTexture();
        }

        /// <summary>
        /// Update the particles position and update the particles life time
        /// </summary>
        /// <param name="deltaTime">time between function calls</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the position of the plane that the particle texture lives on.
            UpdateMatrixes();

            // update the life time of this particle
            currentTime = TotalSeconds; //How long the game has been running in seconds, by milliseconds.

            alpha = (timeToDie - currentTime) * 10 * (float)Math.Pow(0.98f, (Double)counter);

            counter++;

            // The older the particle, the more transparent it becomes or basically it fades away the older it gets
            //alpha = (timeToDie - currentTime) * 10; //TODO: make this work better, this is just a quick fix.


            // If the life of the particle has expired this particle is no longer alive.
            if (currentTime > timeToDie)
            {
                Visible = false;
                Enabled = false;
            }                
        }

        public override void Draw(GameTime gameTime)
        {
            Services.Graphics.SetVertexBuffer(vertexBuffer);

            // Set object and camera info
            basicEffect.World = worldMatrix;
            basicEffect.View = Services.Camera.View;
            basicEffect.Projection = Services.Camera.Projection;
            basicEffect.Alpha = alpha;
            basicEffect.AmbientLightColor = new Vector3(0.5f);

            // Begin effect and draw for each frame
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Services.Graphics.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, verts, 0, 2);
            }            

            base.Draw(gameTime);
        }

        public void ApplyTexture()
        {
            if (texture != null)
                basicEffect.Texture = texture;

            basicEffect.TextureEnabled = true;
        }
        #endregion
        #region Private Methods
        private void SetupVerts()
        {
            float Width = texture.Width * ScalePercent.X;
            float Hieght = texture.Height * ScalePercent.Y;
            // Setup plane
            verts[0] = new VertexPositionTexture(new Vector3(-Width, Hieght, 0), new Vector2(0, 0));
            verts[1] = new VertexPositionTexture(new Vector3(Width, Hieght, 0), new Vector2(1, 0));
            verts[2] = new VertexPositionTexture(new Vector3(-Width, -Hieght, 0), new Vector2(0, 1));
            verts[3] = new VertexPositionTexture(new Vector3(Width, -Hieght, 0), new Vector2(1, 1));
            vertexBuffer.SetData(verts);
        }


        private void UpdateMatrixes()
        {
            worldTranslate = Matrix.CreateTranslation(Position);
            worldScale = Matrix.CreateScale(ScalePercent);
            worldRotate = Matrix.CreateRotationX(RotationInRadians.X);
            worldRotate *= Matrix.CreateRotationY(RotationInRadians.Y);
            worldRotate *= Matrix.CreateRotationZ(RotationInRadians.Z);
            worldMatrix = worldScale * worldRotate * worldTranslate;
        }

        #endregion
    }
}
