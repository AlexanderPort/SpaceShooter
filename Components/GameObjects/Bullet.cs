using System;
using System.Collections.Generic;
using OpenTK;
using Game.Components.Renderables;

namespace Game.Components.GameObjects
{
    public class Bullet : GameObject
    {
        public Bullet(RenderObject model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }

        public override void Update(double time, double delta)
        {
           
            base.Update(time, delta);
        }

        public GameObject CheckCollision(List<GameObject> gameObjects)
        {
            foreach (var x in gameObjects)
            {
                if(x.GetType() != typeof(Asteroid))
                    continue;
                // naive first object in radius
                if ((Position - x.Position).Length < 0.1f)
                    return x;
            }
            return null;
        }
    }
}