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
        private static Services m_Instance = null;
        private static GraphicsDeviceManager m_GraphicsDM;
        private static Random m_RandomNumber;
        private static Matrix m_ViewMatrix;
        private static Matrix m_ProjectionMatrix;
        private static Vector2 m_ScreenSize;
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
                if (m_Instance != null)
                {
                    return m_Instance;
                }

                throw new InvalidOperationException("The Engine Services have not been started!");
            }
        }

        public static GraphicsDeviceManager GraphicsDM
        {
            get { return m_GraphicsDM; }
        }

        public static Random RandomNumber
        {
            get { return m_RandomNumber; }
        }

        public static Matrix ViewMatrix
        {
            get { return m_ViewMatrix; }
        }

        public static Matrix ProjectionMatrix
        {
            get { return m_ProjectionMatrix; }
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
        public static int WindowHeight { get => m_GraphicsDM.PreferredBackBufferHeight; }

        /// <summary>
        /// Returns the window size in pixels, of the width.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowWidth { get => m_GraphicsDM.PreferredBackBufferWidth; }

        public static Vector2 ScreenSize { get => m_ScreenSize; set => m_ScreenSize = value; }
        #endregion

        #region Constructor
        /// <summary>
        /// This is the constructor for the Services
        /// You will note that it is private that means that only the Services can only create itself.
        /// </summary>
        private Services()
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
        /// <param name="graphics">Reference to the graphic device.</param>
        /// <param name="screenSize">Reference to the size of the screen.</param>
        public static void Initialize(GraphicsDeviceManager graphics)
        {
            //First make sure there is not already an instance started
            if (m_Instance == null)
            {
                m_GraphicsDM = graphics;
                m_ScreenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                //Create the Engine Services
                m_Instance = new Services();
                m_RandomNumber = new Random();
                //Set View Matrix and Projection Matrix
                m_ViewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1f), Vector3.Zero, Vector3.Up);
                m_ProjectionMatrix = Matrix.CreateOrthographic(m_ScreenSize.X, m_ScreenSize.Y, 1, 2);

                return;
            }

            throw new Exception("The Engine Services have already been started.");
        }

        public static void Update(GameTime gametime)
        {
        }
        #endregion
    }
}
