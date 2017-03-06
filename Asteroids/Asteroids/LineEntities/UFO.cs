using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    /// <summary>
    /// As the player's score increases, the angle range of the shots from the small saucer diminishes
    /// until the saucer fires extremely accurately.
    /// The small saucer will fire immediately when spawned.
    /// </summary>
    class UFO : LineEngine.LineMesh
    {
        int m_Points;
        int m_PlayerScore;
        bool m_SmallSoucer;

        UFO(Game game) : base(game)
        {

        }

        public void InitializeLineMesh()
        {
            Initialize();

            Vector3[] pointPosition = new Vector3[9];

            pointPosition[0] = new Vector3(19, -4, 0);
            pointPosition[1] = new Vector3(7, 4, 0);
            pointPosition[2] = new Vector3(3, 11, 0);
            pointPosition[3] = new Vector3(-3, 11, 0);
            pointPosition[4] = new Vector3(-7, 4, 0);
            pointPosition[5] = new Vector3(-19, -4, 0);
            pointPosition[6] = new Vector3(-19, -4, 0);
            pointPosition[7] = new Vector3(8, -11, 0);
            pointPosition[8] = new Vector3(19, -4, 0);

            InitializePoints(pointPosition);
        }

    }
}
