using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class Forest
{
    private List<Prop> _trees;
    private Model _treeModel;


    public void Initialize(Vector2 mapSize, float spacing = 5f)
    {
        var random = new Random();
        _trees = new List<Prop>();

        int countX = (int)MathF.Floor(mapSize.X / spacing);
        int countZ = (int)MathF.Floor(mapSize.Y / spacing);

        float gridWidth = countX * spacing;
        float gridLength = countZ * spacing;

        float startX = -gridWidth / 2f + spacing * 0.5f;
        float startZ = -gridLength / 2f + spacing * 0.5f;
        float maxPositionVariation = spacing * 0.35f;

        for (int x = 0; x < countX; x++)
        {
            for (int z = 0; z < countZ; z++)
            {
                float posX = startX + (x * spacing);
                float posZ = startZ + (z * spacing);

                posX += ((float)random.NextDouble() * 2f - 1f) * maxPositionVariation;
                posZ += ((float)random.NextDouble() * 2f - 1f) * maxPositionVariation;

                float rColor = (float)random.NextDouble();
                Color color = Color.Lerp(Color.SaddleBrown, Color.DarkGreen, rColor);

                float scaleVariationY = (float)random.NextDouble() * 2;
                float scaleVariationXZ = (float)random.NextDouble() + 1;
                Vector3 scale = new Vector3(scaleVariationXZ, scaleVariationY, scaleVariationXZ);

                var tree = new Prop(new Vector3(posX, 0, posZ), scale, new Vector3(0,0,0), color);
                tree.Initialize();
                _trees.Add(tree);
            }
        }
    }

    public void LoadContent(ContentManager content, string contentFolder3D, string shaderRoute)
    {
        _treeModel = content.Load<Model>(contentFolder3D + "forest/Tree/Tree");
        _trees.ForEach(tree => tree.LoadContent(content, _treeModel, shaderRoute));
    }

    public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
    {
        _trees.ForEach(tree => tree.Draw(device, view, projection));
    }
}