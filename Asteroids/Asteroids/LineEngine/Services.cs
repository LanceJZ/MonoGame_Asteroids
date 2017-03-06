#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Asteroids.LineEngine
{
    public sealed class Services
    {
        #region Fields
        private static Services instance = null;
        private static GraphicsDevice graphics;
        private static Random randomNumber;
        private static Matrix viewMatrix;
        private static Matrix projectionMatrix;
        #endregion
        #region Properties
        /// <summary>
        /// This is used to get the Services Instance
        /// Instead of using the mInstance this will do the check to see if the Instance is valid
        /// where ever you use it. It is also private so it will only get used inside the engine services.
        /// </summary>
        private static Services Instance
        {
            get
            {
                //Make sure the Instance is valid
                if (instance != null)
                {
                    return instance;
                }

                throw new InvalidOperationException("The Engine Services have not been started!");
            }
        }

        public static GraphicsDevice Graphics
        {
            get { return graphics; }
        }

        public static Random RandomNumber
        {
            get { return randomNumber; }
        }

        public static Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public static Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        /// <summary>
        /// Get a random float between min and max
        /// </summary>
        /// <param name="min">the minimum random value</param>
        /// <param name="max">the maximum random value</param>
        /// <returns>float</returns>
        public static float RandomMinMax(float min, float max)
        {
            return min + (float)RandomNumber.NextDouble() * (max - min);
        }

        /// <summary>
        /// Returns the window size in pixels, of the height.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowHeight
        {
            get { return graphics.ScissorRectangle.Height; }
        }

        /// <summary>
        /// Returns the window size in pixels, of the width.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowWidth
        {
            get { return graphics.ScissorRectangle.Width; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// This is the constructor for the Services
        /// You will note that it is private that means that only the Services can only create itself.
        /// </summary>
        private Services(Game game)
        {            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This is used to start up Panther Engine Services.
        /// It makes sure that it has not already been started if it has been it will throw and exception
        /// to let the user know.
        /// 
        /// You pass in the game class so you can get information needed.
        /// </summary>
        /// <param name="game">Reference to the game class.</param>
        /// <param name="graphicsDevice">Reference to the graphic device.</param>
        /// <param name="near">Sets the near plane for the camera.</param>
        /// <param name="far">Sets the far plane of the camera.</param>
        public static void Initialize(Game game, GraphicsDevice graphicsDevice)
        {
            //First make sure there is not already an instance started
            if (instance == null)
            {
                //Create the Engine Services
                instance = new Services(game);
                graphics = graphicsDevice;
                randomNumber = new Random();
                viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1f), Vector3.Zero, Vector3.Up);
                projectionMatrix = Matrix.CreateOrthographic((float)graphicsDevice.Viewport.Width,
                    (float)graphicsDevice.Viewport.Height, 1, 2);

                return;
            }

            throw new Exception("The Engine Services have already been started.");
        }

        public static void Update(GameTime gametime)
        {
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
