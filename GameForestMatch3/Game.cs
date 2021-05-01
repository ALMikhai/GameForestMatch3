using GameForestMatch3.Button;
using GameForestMatch3.Scenes;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameForestMatch3
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static readonly float StandardWidth = 1920;
        public static readonly float StandardHeight = 1080;
        private readonly Matrix _inputScaleMatrix;
        private readonly Matrix _screenScaleMatrix;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MouseState _previousMouseState;
        private SpriteFont _generalFont;

        private MenuScene _menuScene;
        private GameOverScene _gameOverScene;
        private GameScene _gameScene;
        private Scene _currentScene;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            var heightScreenScale = _graphics.PreferredBackBufferHeight / StandardHeight;
            var widthScreenScale = _graphics.PreferredBackBufferWidth / StandardWidth;
            var heightInputScale = StandardHeight / _graphics.PreferredBackBufferHeight;
            var widthInputScale = StandardWidth / _graphics.PreferredBackBufferWidth;
            _screenScaleMatrix = Matrix.CreateScale(widthScreenScale, heightScreenScale, 1.0f);
            _inputScaleMatrix = Matrix.CreateScale(widthInputScale, heightInputScale, 1.0f);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _previousMouseState = Mouse.GetState();
        }

        protected override void LoadContent()
        {
            TileFactory.ContentManager = Content;
            ButtonFactory.ContentManager = Content;
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _generalFont = Content.Load<SpriteFont>("GeneralFont");

            _menuScene = new MenuScene(_generalFont);
            _menuScene.PlayButton.Click += OnPlayButtonClick;

            _gameOverScene = new GameOverScene(_generalFont);
            _gameOverScene.OkButton.Click += OnOkButtonClick;

            _currentScene = _menuScene;
        }

        private void OnOkButtonClick()
        {
            _currentScene = _menuScene;
        }

        private void OnPlayButtonClick()
        {
            _gameScene = new GameScene(8, 8, 60000, _generalFont);
            _gameScene.GameOver += OnGameOver;
            _currentScene = _gameScene;
        }

        private void OnGameOver()
        {
            _gameScene.GameOver -= OnGameOver;
            _currentScene = _gameOverScene;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_previousMouseState.LeftButton == ButtonState.Pressed
                && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                var correctPosition = Vector2.Transform(Mouse.GetState().Position.ToVector2(), _inputScaleMatrix);
                _currentScene.MouseClick(correctPosition);
            }

            _previousMouseState = Mouse.GetState();

            _currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _screenScaleMatrix);

            _currentScene.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}