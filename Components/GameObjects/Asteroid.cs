using System;
using OpenTK;
using Game.Components.Renderables;

namespace Game.Components.GameObjects
{
    public class Asteroid : GameObject
    {
        public int Score { get; set; }
        public Asteroid(RenderObject model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }


        public override void Update(double time, double delta)
        {
        
            _rotation.Z = (float)Math.Cos((time + _gameObjectNumber) * 0.2);
            
            base.Update(time, delta);
        }
    }
}