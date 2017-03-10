﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    using serv = LineEngine.Services;

    public class Explode : GameComponent
    {
        List<Dot> m_Dots;
        Game m_Game;
        bool m_Active = false;

        public bool Active
        {
            get
            {
                return m_Active;
            }
        }

        public Explode( Game game) : base(game)
        {
            game.Components.Add(this);

            m_Dots = new List<Dot>();
            m_Game = game;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool done = true;

            foreach (Dot dot in m_Dots)
            {
                if (dot.Visible)
                {
                    done = false;
                    break;
                }
            }

            if (done)
                m_Active = false;
        }

        public void Spawn(Vector3 position, float radius)
        {
            m_Active = true;
            int count = (int)serv.RandomMinMax(10, 10 + radius);
            
            if (count > m_Dots.Count)
            {
                int more = count - m_Dots.Count;

                for (int i = 0; i < more; i++)
                {
                    m_Dots.Add(new Dot(m_Game));
                }
            }

            for (int i = 0; i < count; i++)
            {
                m_Dots[i].Spawn(position, radius);
            }
        }
    }
}