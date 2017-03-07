#region Using
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
        Matrix meshMatrix;
        BasicEffect basicEffect;
        VertexPositionColor[] pointList;
        VertexBuffer vertexBuffer;
        RasterizerState rasterizerState;
        short[] lineListIndices;

        public LineMesh (Game game) : base(game)
        {            
        }

        public override void Initialize()
        {
            base.Initialize();
            meshMatrix = Matrix.CreateTranslation(0, 0, 0);
            rasterizerState = new RasterizerState();
            rasterizerState.FillMode = FillMode.WireFrame;
            rasterizerState.CullMode = CullMode.None;
            Visible = true;
            Enabled = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculate the mesh transformation by combining translation, rotation, and scaling
            meshMatrix = Matrix.CreateScale(ScalePercent) 
                * Matrix.CreateFromYawPitchRoll(0, 0, RotationInRadians) * Matrix.CreateTranslation(Position);
            // Apply to Effect
            basicEffect.World = meshMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                base.Draw(gameTime);

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                }

                Services.GraphicsDM.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, pointList,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    pointList.Length,  // number of vertices in pointList
                    lineListIndices,  // the index buffer
                    0,  // first index element to read
                    pointList.Length - 1   // number of primitives to draw
                );
            }
        }

        /// <summary>
        /// Initializes the point list.
        /// </summary>
        public void InitializePoints(Vector3[] pointPosition)
        {
            VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                }
            );

            pointList = new VertexPositionColor[pointPosition.Length];

            for (int x = 0; x < pointPosition.Length; x++)
            {
                pointList[x] = new VertexPositionColor(pointPosition[x], Color.White);
            }

            // Initialize the vertex buffer, allocating memory for each vertex.
            vertexBuffer = new VertexBuffer(Services.GraphicsDM.GraphicsDevice, vertexDeclaration,
                pointPosition.Length, BufferUsage.None);

            // Set the vertex buffer data to the array of vertices.
            vertexBuffer.SetData<VertexPositionColor>(pointList);
            InitializeLineList();
            InitializeEffect();
        }

        /// <summary>
        /// Initializes the line list.
        /// </summary>
        private void InitializeLineList()
        {
            // Initialize an array of indices of type short.
            lineListIndices = new short[(pointList.Length * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < pointList.Length - 1; i++)
            {
                lineListIndices[i * 2] = (short)(i);
                lineListIndices[(i * 2) + 1] = (short)(i + 1);
            }
        }

        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game.
        /// </summary>
        public void InitializeEffect()
        {
            basicEffect = new BasicEffect(Services.GraphicsDM.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = Services.ViewMatrix;
            basicEffect.Projection = Services.ProjectionMatrix;
            basicEffect.World = meshMatrix;
        }

        /// <summary>
        /// Load Line Mesh from file.
        /// </summary>
        public void LoadMesh()
        {
        }
    }
}
