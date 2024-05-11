using System;
using OpenTK;
using Game.Components.Renderables;

namespace Game.Components.GameObjects
{
    public class GameOver : GameObject
    {
        public GameOver(RenderObject model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }

        public override void Update(double time, double delta)
        {
            base.Update(time, delta);
        }
        
    }
}