﻿#region Using
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asteroids.LineEngine
{
    public class LineMesh : PositionedObject
    {
        Matrix m_LocalMatrix;
        VertexPositionColor[] m_PointList;
        VertexBuffer m_VertexBuffer;
        RasterizerState m_RasterizerState;
        short[] m_LineListIndices;
        bool m_Initialized = false;

        public LineMesh (Game game) : base(game)
        {            
        }

        public override void Initialize()
        {
            base.Initialize();
            m_RasterizerState = new RasterizerState();
            m_RasterizerState.FillMode = FillMode.WireFrame;
            m_RasterizerState.CullMode = CullMode.None;
            Visible = true;
            Enabled = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Transform();

            foreach (EffectPass pass in Services.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            Services.GraphicsDM.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList, m_PointList,
                0,  // vertex buffer offset to add to each element of the index buffer
                m_PointList.Length,  // number of vertices in pointList
                m_LineListIndices,  // the index buffer
                0,  // first index element to read
                m_PointList.Length - 1   // number of primitives to draw
            );
        }

        public void Destroy()
        {
            m_VertexBuffer.Dispose();
            m_RasterizerState.Dispose();
            Dispose();
        }

        /// <summary>
        /// Initializes the point list.
        /// </summary>
        public void InitializePoints(Vector3[] pointPosition)
        {
            if (!m_Initialized)
            {
                m_Initialized = true;

                VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
                );

                m_PointList = new VertexPositionColor[pointPosition.Length];

                for (int x = 0; x < pointPosition.Length; x++)
                {
                    m_PointList[x] = new VertexPositionColor(pointPosition[x], new Color(190, 170, 255));
                }

                // Initialize the vertex buffer, allocating memory for each vertex.
                m_VertexBuffer = new VertexBuffer(Services.GraphicsDM.GraphicsDevice, vertexDeclaration,
                    pointPosition.Length, BufferUsage.None);

                // Set the vertex buffer data to the array of vertices.
                m_VertexBuffer.SetData<VertexPositionColor>(m_PointList);
                InitializeLineList();
                InitializeEffect();
                Transform();
            }
        }

        void Transform()
        {
            // Calculate the mesh transformation by combining translation, rotation, and scaling
            m_LocalMatrix = Matrix.CreateScale(ScalePercent) * Matrix.CreateFromYawPitchRoll(0, 0, RotationInRadians)
                * Matrix.CreateTranslation(Position);
            // Apply to Effect
            Services.BasicEffect.World = m_LocalMatrix;
        }

        /// <summary>
        /// Initializes the line list.
        /// </summary>
        private void InitializeLineList()
        {
            // Initialize an array of indices of type short.
            m_LineListIndices = new short[(m_PointList.Length * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < m_PointList.Length - 1; i++)
            {
                m_LineListIndices[i * 2] = (short)(i);
                m_LineListIndices[(i * 2) + 1] = (short)(i + 1);
            }
        }

        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game. Moved to Services.
        /// </summary>
        public void InitializeEffect()
        {
            Services.BasicEffect.World = m_LocalMatrix;
        }

        /// <summary>
        /// Load Line Mesh from file.
        /// </summary>
        public void LoadMesh()
        {
        }
    }
}
