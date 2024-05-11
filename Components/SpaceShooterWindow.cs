using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Game.Components.GameObjects;
using Game.Components.Renderables;

namespace Game.Components
{
    public sealed class SpaceShooterWindow : GameWindow
    {
        private static int screen_width = 750;
        private static int screen_height = 500;
        private readonly string _title;
        private GameObjectFactory _gameObjectFactory;
        private List<GameObject> _gameObjects = new List<GameObject>();
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;
        private ShaderProgram _texturedProgram;
        private KeyboardState _lastKeyboardState;
        private Spaceship _spacecraft;
        private Spaceship _player;
        private int _score;
        public bool _gameOver;


        public SpaceShooterWindow()
            : base(screen_width, // initial width
                screen_height, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4, // OpenGL major version
                5, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            _title += "SpaceShooter";
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjection();
        }


        protected override void OnLoad(EventArgs e)
        {
            Debug.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();
            

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, @"Components\shaders\vert\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, @"Components\shaders\frag\simplePipeTexFrag.c");
            _texturedProgram.Link();
            
            var models = new Dictionary<string, RenderObject>();

            models.Add("Asteroid", new TexturedRenderObject(RenderObjectFactory.CreateTexturedSprite(1, 612, 593), _texturedProgram.Id, @"Components\Textures\asteroid1.png"));
            models.Add("Spacecraft", new TexturedRenderObject(RenderObjectFactory.CreateTexturedSprite(2, 1600, 1600), _texturedProgram.Id, @"Components\Textures\spaceship1.png"));
            models.Add("Gameover", new TexturedRenderObject(RenderObjectFactory.CreateTexturedSprite(2, 1200, 1100), _texturedProgram.Id, @"Components\Textures\gameover1.jpg"));
            models.Add("Bullet", new TexturedRenderObject(RenderObjectFactory.CreateTexturedSprite(1, 225, 347), _texturedProgram.Id, @"Components\Textures\bullet1.png"));

            _gameObjectFactory = new GameObjectFactory(screen_width, screen_height, models);

            _player = _gameObjectFactory.CreateSpacecraft();
            _gameObjects.Add(_player);
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());



            CursorVisible = true;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Closed += OnClosed;
            Debug.WriteLine("OnLoad .. done");
        }
        
        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        public override void Exit()
        {
            Debug.WriteLine("Exit called");
            _gameObjectFactory.Dispose();
            _texturedProgram.Dispose();
            base.Exit();
        }
        
        private void CreateProjection()
        {
            
            var aspectRatio = (float)Width/Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov*((float) Math.PI / 180f), // view angle
                aspectRatio,                // window aspect ratio
                0.1f,                       // near plane
                4000f);                     // far plane
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _time += e.Time;
            var remove = new HashSet<GameObject>();
            
            int removedAsteroids = 0;
            int outOfBoundsAsteroids = 0;
            foreach (var item in _gameObjects)
            {
                item.Update(_time, e.Time);
                if ((item.Position.Y > 2f || item.Position.X > 2f || item.Position.X < -2f))
                {
                    remove.Add(item);
                    outOfBoundsAsteroids++;
                }
                if (item.GetType() == typeof (Bullet))
                {
                    var collide = ((Bullet) item).CheckCollision(_gameObjects);
                    if (collide != null)
                    {
                        remove.Add(item);
                        if (remove.Add(collide))
                        {
                            _score += ((Asteroid)collide).Score;
                            removedAsteroids++;
                        }
                    }
                }
                if (item.GetType() == typeof(Spaceship))
                {
                    var collide = ((Spaceship)item).CheckCollision(_gameObjects);
                    if (collide != null)
                    {
                        foreach (var x in _gameObjects)
                            remove.Add(x);
                        _gameObjects.Add(_gameObjectFactory.CreateGameOver());
                        _gameOver = true;
                        removedAsteroids = 0;
                        break;
                    }
                }
            }
            foreach (var r in remove)
                _gameObjects.Remove(r);
            for (int i = 0; i < removedAsteroids; i++)
            {
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
            }
            for (int i = 0; i < outOfBoundsAsteroids; i++)
            {
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
            }
            HandleKeyboard(e.Time);
        }

        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.M))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            }
            if (keyState.IsKeyDown(Key.Comma))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (keyState.IsKeyDown(Key.Period))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            
            if (keyState.IsKeyDown(Key.A))
            {
                _player.MoveLeft();
            }
            if (keyState.IsKeyDown(Key.D))
            {
                _player.MoveRight();
            }
            if (!_gameOver && keyState.IsKeyDown(Key.Space) && _lastKeyboardState.IsKeyUp(Key.Space))
            {
                _gameObjects.Add(_gameObjectFactory.CreateBullet(_player.Position));
            }
            _lastKeyboardState = keyState;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {

            Title = $"{_title}: FPS:{1f / e.Time:0000.0}, obj:{_gameObjects.Count}, score:{_score}";
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;
            foreach (var obj in _gameObjects)
            {
                var program = obj.Model.Program;
                if (lastProgram != program)
                    GL.UniformMatrix4(20, false, ref _projectionMatrix);
                lastProgram = obj.Model.Program;
                obj.Render();

            }
            SwapBuffers();
        }
        
    }
}
