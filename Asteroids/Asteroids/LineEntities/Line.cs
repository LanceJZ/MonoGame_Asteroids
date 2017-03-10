using Microsoft.Xna.Framework;

namespace Asteroids
{
    public class Line : LineEngine.LineMesh
    {
        public Line(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeLineMesh();
        }

        void InitializeLineMesh()
        {
            Vector3[] pointPosition = new Vector3[2];

            pointPosition[0] = new Vector3(0, 1.5f, 0);
            pointPosition[1] = new Vector3(0, -1.5f, 0);

            InitializePoints(pointPosition);
        }
    }
}