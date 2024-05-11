using System;
using System.Collections.Generic;
using OpenTK;
using Game.Components.Renderables;

namespace Game.Components.GameObjects
{
    public class Spaceship : GameObject
    {
        private bool _moveLeft;
        private bool _moveRight;
        public Spaceship(RenderObject model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }

        public override void Update(double time, double delta)
        {
            if (_moveLeft && !(_direction.X > 0 && _velocity > 0))
            {
                _direction.X = -1;
                _velocity += 0.8f * (float)delta;
                _moveLeft = false;
            }
            else if (_moveRight && !(_direction.X < 0 && _velocity > 0))
            {
                _direction.X = 1;
                _velocity += 0.8f * (float)delta;
                _moveRight = false;
            }
            else
            {
                _velocity -= 0.9f * (float)delta;
            }

            if (_velocity > 0.8f)
            {
                _velocity = 0.8f;
            }
            if (_velocity < 0)
            {
                _velocity = 0;
                
            }
           
            base.Update(time, delta);
        }

        public void MoveLeft()
        {
            _moveLeft = true;
        }

        public void MoveRight()
        {
            _moveRight = true;
        }

        public GameObject CheckCollision(List<GameObject> gameObjects)
        {
            foreach (var x in gameObjects)
            {
                if (x.GetType() != typeof(Asteroid))
                    continue;
                // naive first object in radius
                if ((Position - x.Position).Length < (Scale.X + x.Scale.X))
                    return x;
            }
            return null;
        }
    }
}