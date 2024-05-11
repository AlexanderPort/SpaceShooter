using System;
using System.Collections.Generic;
using OpenTK;
using Game.Components.GameObjects;
using Game.Components.Renderables;

namespace Game.Components
{
    public class GameObjectFactory : IDisposable
    {
        private const float Z = -3f;
        private int _screen_width, _screen_height;
        private readonly Random _random = new Random();
        private readonly Dictionary<string, RenderObject> _models;
        public GameObjectFactory(int screen_width, int screen_height, 
            Dictionary<string, RenderObject> models)
        {
            _models = models;
            _screen_width = screen_width;
            _screen_height = screen_height;
        }


        public Spaceship CreateSpacecraft()
        {
            var spacecraft = new Spaceship(_models["Spacecraft"], new Vector4(0, -1f, Z, 0), Vector4.Zero, Vector4.Zero, 0);
            spacecraft.SetScale(new Vector3(0.2f, 0.2f, 0.01f));
            return spacecraft;
        }
        public Asteroid CreateAsteroid(string model, Vector4 position)
        {
            Random random = new Random();
            var obj = new Asteroid(_models[model], position, Vector4.Zero, Vector4.Zero, 0.3f);
            obj.SetScale(new Vector3(0.2f));
            obj.setDirection(new Vector4(2f * (float)random.NextDouble() - 1f, 
                -Math.Abs((float)random.NextDouble()), 0.0f, 0.0f));
            switch (model)
            {
                case "Asteroid":
                    obj.Score = 1;
                    break;
              
            }
            return obj;
        }
        public GameObject CreateRandomAsteroid()
        {
            
            var position = GetRandomPosition();
           
            return CreateAsteroid("Asteroid", position);
        }
        public Asteroid CreateAsteroid()
        {
            return CreateAsteroid("Asteroid", GetRandomPosition());
        }
       
        public Bullet CreateBullet(Vector4 position)
        {
            var bullet = new Bullet(_models["Bullet"], position, Vector4.UnitY, Vector4.Zero, 0.8f);
            bullet.SetScale(new Vector3(0.1f));
            return bullet;
        }
        public GameObject CreateGameOver()
        {
            var obj = new GameOver(_models["Gameover"], new Vector4(0, 0, Z, 0), Vector4.Zero, Vector4.Zero, 0.0f);
            obj.SetScale(new Vector3(0.8f));
            return obj;
        }

        private Vector4 GetRandomPosition()
        {
            var position = new Vector4(
                ((float) _random.NextDouble() - 0.5f) * 2f,
                1.5f,
                Z,
                0);
            return position;
        }
        public void Dispose()
        {
            foreach (var obj in _models)
                obj.Value.Dispose();
        }

    }
}