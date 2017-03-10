using Microsoft.Xna.Framework;

namespace Asteroids
{
    public class Shot : LineEngine.LineMesh
    {
        LineEngine.Timer m_LifeTimer;

        public Shot(Game game) : base(game)
        {
            m_LifeTimer = new LineEngine.Timer(game);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeLineMesh();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CheckBorders();

            if (m_LifeTimer.Seconds > m_LifeTimer.Amount)
            {
                Visible = false;
            }
        }

        public void Spawn(Vector3 position, Vector3 velecity, float timer)
        {
            m_LifeTimer.Reset();
            m_LifeTimer.Amount = timer;
            Position = position;
            Velocity = velecity;
            Visible = true;
        }

        public void Reset()
        {
            Visible = false;
        }

        void InitializeLineMesh()
        {
            Vector3[] pointPosition = new Vector3[4];

            pointPosition[0] = new Vector3(-0.5f, 0.5f, 0);
            pointPosition[1] = new Vector3(0.5f, -0.5f, 0);
            pointPosition[2] = new Vector3(0.5f, 0.5f, 0);
            pointPosition[3] = new Vector3(-0.5f, -0.5f, 0);

            InitializePoints(pointPosition);

            Visible = false;
            Radius = 0.5f;
        }
    }
}
