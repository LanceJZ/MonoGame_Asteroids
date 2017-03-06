using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asteroids
{
    using Services = LineEngine.Services;

    /// <summary>
    /// After 40,000 points only small UFOs spawn.
    /// A steadily decreasing timer that shortens intervals between saucer spawns on each UFO.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        Player m_Player;
        List<Rock> m_LargeRocks;
        List<Rock> m_MedRocks;
        List<Rock> m_SmallRocks;
        int m_Wave = 0;
        int m_LargeRockCount = 4;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = false;            
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            Content.RootDirectory = "Media";
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here
            LineEngine.Services.Initialize(this, graphics.GraphicsDevice);

            m_Player = new Player(this);
            m_LargeRocks = new List<Rock>();
            m_MedRocks = new List<Rock>();
            m_SmallRocks = new List<Rock>();

            m_Player.InitializeLineMesh();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            CountRocks();

            base.Update(gameTime);
            // TODO: Add your update logic here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(new Vector3(0.025f, 0, 0.20f)));

            base.Draw(gameTime);
        }

        void CountRocks()
        {
            int rockCount = 0;

            foreach (Rock rock in m_LargeRocks)
            {
                if (rock.Visible)
                {
                    rockCount++;

                    if (rock.Hit)
                    {
                        SpawnMedRocks(rock.Position);
                        rock.Visible = false;
                        rock.Hit = false;
                    }
                }

            }

            foreach (Rock rock in m_MedRocks)
            {
                if (rock.Visible)
                {
                    rockCount++;

                    if (rock.Hit)
                    {
                        SpawnSmallRocks(rock.Position);
                        rock.Visible = false;
                        rock.Hit = false;
                    }
                }
                    
            }

            foreach (Rock rock in m_SmallRocks)
            {
                if (rock.Visible)
                {
                    rockCount++;

                    if (rock.Hit)
                    {
                        rock.Visible = false;
                        rock.Hit = false;
                    }
                }
            }

            if (rockCount == 0)
            {
                SpawnLargeRocks(m_LargeRockCount);
                m_LargeRockCount += 2;

                if (m_LargeRockCount > 11)
                    m_LargeRockCount = 11;

                m_Wave++;
            }
        }

        void SpawnLargeRocks(int count)
        {
            m_Wave++;

            for (int i = 0; i < count; i++)
            {
                bool spawnNewRock = true;

                foreach (Rock rock in m_LargeRocks)
                {
                    if (!rock.Visible && !rock.ExplosionActive)
                    {
                        spawnNewRock = false;
                        rock.Spawn();
                        rock.SetRandomEdge();
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_LargeRocks.Count;
                    m_LargeRocks.Add(new Rock(this));
                    m_LargeRocks[rock].InitializeLineMesh();
                    m_LargeRocks[rock].Player = m_Player;
                    m_LargeRocks[rock].Spawn();
                    m_LargeRocks[rock].SetRandomEdge();
                }
            }
        }

        void SpawnMedRocks(Vector3 position)
        {
            for (int i = 0; i < 2; i++)
            {
                bool spawnNewRock = true;

                foreach (Rock rock in m_MedRocks)
                {
                    if (!rock.Visible && !rock.ExplosionActive)
                    {
                        spawnNewRock = false;
                        rock.Spawn(position);
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_MedRocks.Count;
                    m_MedRocks.Add(new Rock(this));
                    m_MedRocks[rock].InitializeLineMesh();
                    m_MedRocks[rock].Spawn(position, 0.5f, 150, 50, m_Player);
                }
            }
        }

        void SpawnSmallRocks(Vector3 position)
        {
            for (int i = 0; i < 2; i++)
            {
                bool spawnNewRock = true;

                foreach (Rock rock in m_SmallRocks)
                {
                    if (!rock.Visible && !rock.ExplosionActive)
                    {
                        spawnNewRock = false;
                        rock.Spawn(position);
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_SmallRocks.Count;
                    m_SmallRocks.Add(new Rock(this));
                    m_SmallRocks[rock].InitializeLineMesh();
                    m_SmallRocks[rock].Spawn(position, 0.25f, 300, 100, m_Player);
                }
            }

        }
    }
}
