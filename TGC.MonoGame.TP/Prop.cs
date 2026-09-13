using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class Prop
{
    private Vector3 _position;
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
        _world = Matrix.CreateTranslation(_position) * Matrix.CreateScale(new Vector3(0.1f,0.1f,0.1f));
    }

    public void LoadContent(ContentManager content, string modelRoute, string shaderRoute)
    {
        _model = content.Load<Model>(modelRoute);
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
}