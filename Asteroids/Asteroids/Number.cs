using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    struct NumberData
    {
        public bool[] Lines;
    };

    public class Number : LineEngine.PositionedObject
    {
        Game m_Game;
        NumberData[] Numbers = new NumberData[10];
        Vector3[] m_NumberLineStart = new Vector3[7];
        Vector3[] m_NumberLineEnd = new Vector3[7];
        public List<LineEngine.PositionedObject> m_Numbers;
        List<LineEngine.LineMesh> m_EachLine;

        public Number(Game game) : base(game)
        {
            m_Game = game;
        }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < 10; i++)
            {
                Numbers[i].Lines = new bool[7];
            }

            InitializeNumberLines();
            m_Numbers = new List<LineEngine.PositionedObject>();
            m_EachLine = new List<LineEngine.LineMesh>();
        }

        public override void Update(GameTime gametime)
        {
            // Do stuff every new frame
        }

        public void ProcessNumber(int number, Vector3 locationStart, float size)
        {
            DeleteNumbers();
            Position = locationStart;
            int numberIn = number;
            float space = 0;

            do
            {
                //Make digit the modulus of 10 from number.
                int digit = numberIn % 10;
                //This sends a digit to the draw function with the location and size.
                MakeNumberMesh(space, digit, size);
                // Dividing the int by 10, we discard the digit that was derived from the modulus operation.
                numberIn /= 10;
                // Move the location for the next digit location to the left. We start on the right hand side
                // with the lowest digit.
                space += size * 2;
            } while (numberIn > 0);
        }

        public void DeleteNumbers()
        {
            if (m_Numbers.Count > 0)
                m_Numbers.Clear();

            if (m_EachLine.Count > 0)
            {
                for (int i = 0; i < m_EachLine.Count; i++)
                {
                    m_EachLine[i].Visible = false;
                    m_EachLine[i].Enabled = false;
                    m_EachLine[i].Remove();
                    m_EachLine[i].Destroy();
                }

                m_EachLine.Clear();
            }                
        }

        public void HideNumbers()
        {
            if (m_EachLine != null)
            {
                foreach (LineEngine.LineMesh num in m_EachLine)
                {
                    num.Visible = false;
                }
            }
        }

        public void ShowNumbers()
        {
            if (m_EachLine != null)
            {
                foreach (LineEngine.LineMesh num in m_EachLine)
                {
                    num.Visible = true;
                }
            }
        }

        void MakeNumberMesh(float location, int number, float size)
        {
            Vector3[] numberLines = new Vector3[2];

            if (number > -1 && number < 10)
            {
                for (int line = 0; line < 7; line++)
                {
                    if (Numbers[number].Lines[line])
                    {
                        float Xstart = m_NumberLineStart[line].X * (size * 1.25f);
                        float Ystart = m_NumberLineStart[line].Y * size;

                        float Xend = m_NumberLineEnd[line].X * (size * 1.25f);
                        float Yend = m_NumberLineEnd[line].Y * size;

                        numberLines[0] = new Vector3(Xstart, Ystart, 0);
                        numberLines[1] = new Vector3(Xend, Yend, 0);

                        m_EachLine.Add(new LineEngine.LineMesh(m_Game));
                        m_EachLine[m_EachLine.Count - 1].Position = Position - new Vector3(location, 0, 0);
                        m_EachLine[m_EachLine.Count - 1].InitializePoints(numberLines);
                    }
                }
            }
        }

        void InitializeNumberLines()
        {
            // LED Grid location of line start and end points. 0, 0 is the top left corner.
            //The left of screen, positive X is the direction for rotation zero.
            //The top of the screen, positive Y is the direction for rotation zero.
            m_NumberLineStart[0] = new Vector3(0, 0, 0);
            m_NumberLineStart[1] = new Vector3(1, 0, 0);
            m_NumberLineStart[2] = new Vector3(1, -1, 0);
            m_NumberLineStart[3] = new Vector3(0, -2, 0);
            m_NumberLineStart[4] = new Vector3(0, -1, 0);
            m_NumberLineStart[5] = new Vector3(0, 0, 0);
            m_NumberLineStart[6] = new Vector3(0, -1, 0);

            m_NumberLineEnd[0] = new Vector3(1, 0, 0);
            m_NumberLineEnd[1] = new Vector3(1, -1, 0);
            m_NumberLineEnd[2] = new Vector3(1, -2, 0);
            m_NumberLineEnd[3] = new Vector3(1, -2, 0);
            m_NumberLineEnd[4] = new Vector3(0, -2, 0);
            m_NumberLineEnd[5] = new Vector3(0, -1, 0);
            m_NumberLineEnd[6] = new Vector3(1, -1, 0);

            // LED Grid, what lines are on for each number.
            // Line 0 is the top line.
            // Line 1 is upper right side line.
            // Line 2 is lower right side line.
            // Line 3 is bottom line.
            // Line 4 is lower left side line.
            // Line 5 is upper left side line.
            // Line 6 is the middle line.

            Numbers[0].Lines[0] = true;
            Numbers[0].Lines[1] = true;
            Numbers[0].Lines[2] = true;
            Numbers[0].Lines[3] = true;
            Numbers[0].Lines[4] = true;
            Numbers[0].Lines[5] = true;
            Numbers[0].Lines[6] = false;

            Numbers[1].Lines[0] = false;
            Numbers[1].Lines[1] = true;
            Numbers[1].Lines[2] = true;
            Numbers[1].Lines[3] = false;
            Numbers[1].Lines[4] = false;
            Numbers[1].Lines[5] = false;
            Numbers[1].Lines[6] = false;

            Numbers[2].Lines[0] = true;
            Numbers[2].Lines[1] = true;
            Numbers[2].Lines[2] = false;
            Numbers[2].Lines[3] = true;
            Numbers[2].Lines[4] = true;
            Numbers[2].Lines[5] = false;
            Numbers[2].Lines[6] = true;

            Numbers[3].Lines[0] = true;
            Numbers[3].Lines[1] = true;
            Numbers[3].Lines[2] = true;
            Numbers[3].Lines[3] = true;
            Numbers[3].Lines[4] = false;
            Numbers[3].Lines[5] = false;
            Numbers[3].Lines[6] = true;

            Numbers[4].Lines[0] = false;
            Numbers[4].Lines[1] = true;
            Numbers[4].Lines[2] = true;
            Numbers[4].Lines[3] = false;
            Numbers[4].Lines[4] = false;
            Numbers[4].Lines[5] = true;
            Numbers[4].Lines[6] = true;

            Numbers[5].Lines[0] = true;
            Numbers[5].Lines[1] = false;
            Numbers[5].Lines[2] = true;
            Numbers[5].Lines[3] = true;
            Numbers[5].Lines[4] = false;
            Numbers[5].Lines[5] = true;
            Numbers[5].Lines[6] = true;

            Numbers[6].Lines[0] = true;
            Numbers[6].Lines[1] = false;
            Numbers[6].Lines[2] = true;
            Numbers[6].Lines[3] = true;
            Numbers[6].Lines[4] = true;
            Numbers[6].Lines[5] = true;
            Numbers[6].Lines[6] = true;

            Numbers[7].Lines[0] = true;
            Numbers[7].Lines[1] = true;
            Numbers[7].Lines[2] = true;
            Numbers[7].Lines[3] = false;
            Numbers[7].Lines[4] = false;
            Numbers[7].Lines[5] = false;
            Numbers[7].Lines[6] = false;

            Numbers[8].Lines[0] = true;
            Numbers[8].Lines[1] = true;
            Numbers[8].Lines[2] = true;
            Numbers[8].Lines[3] = true;
            Numbers[8].Lines[4] = true;
            Numbers[8].Lines[5] = true;
            Numbers[8].Lines[6] = true;

            Numbers[9].Lines[0] = true;
            Numbers[9].Lines[1] = true;
            Numbers[9].Lines[2] = true;
            Numbers[9].Lines[3] = false;
            Numbers[9].Lines[4] = false;
            Numbers[9].Lines[5] = true;
            Numbers[9].Lines[6] = true;
        }
    }
}
