using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class Prop
{
    private Vector3 _position;
    private Vector3 _scale;

    private Color _color;

    private Matrix _world;

    private Model _model;
    private Effect _effect;
    
    public Prop(Vector3 position, Color color)
    {
        _position = position;
        _color = color;
    }

    public void Initialize()
    {
        _world = Matrix.CreateTranslation(_position);
    }

    public void LoadContent(ContentManager content, string modelRoute, string shaderRoute)
    {
        var loadedModel = content.Load<Model>(modelRoute);
        LoadContent(content, loadedModel, shaderRoute);
    }
    public void LoadContent(ContentManager content, Model model, string shaderRoute)
    {
        _model = model;
        _effect = content.Load<Effect>(shaderRoute);

        foreach (var mesh in _model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = _effect;
            }
        }
    }

    public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
    {
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["DiffuseColor"].SetValue(_color.ToVector3());
        
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }

    public void SetScale(Vector3 scale)
    {
        _scale = new Vector3(scale.X, scale.Y, scale.Z);
        RebuildWorld();
    }

    private void RebuildWorld()
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_position);
    }
}