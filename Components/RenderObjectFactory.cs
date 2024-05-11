using OpenTK;
using OpenTK.Graphics;
using Game.Components.Renderables;

namespace Game.Components
{
    public class RenderObjectFactory
    {

        public static TexturedVertex[] CreateTexturedSprite(float side, float textureWidth, float textureHeight)
        {
            float h = textureHeight;
            float w = textureWidth;
            side = side / 2f; // half side - and other half

            TexturedVertex[] vertices =
            {

                new TexturedVertex(new Vector4(-side, -side, 0f, 1.0f),    new Vector2(0, h)),
                new TexturedVertex(new Vector4(side, -side, 0f, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(-side, side, 0f, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, side, 0f, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, 0f, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, side, 0f, 1.0f),      new Vector2(w, 0)),
            };
            return vertices;
        }

       
    }
}