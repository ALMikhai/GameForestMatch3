using GameForestMatch3.Board;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Scenes
{
    public class GameScene : Scene
    {
        public delegate void GameEventHandler();
        public event GameEventHandler GameOver;

        private readonly BoardView _boardView;
        private readonly Vector2 _scorePosition = new Vector2(25, 25);
        private readonly Vector2 _timerPosition = new Vector2(25, 100);
        private readonly Vector2 _boardPosition = new Vector2(440, 25);
        private readonly Timer _timer;
        private int _score;

        public GameScene(int boardRows, int boardColumns, int gameTimeMilliseconds, SpriteFont font) : base(font)
        {
            _boardView = new BoardView(new BoardModel(boardRows, boardColumns), _boardPosition);
            _boardView.BoardModel.TileDeleted += OnTileDeleted;

            _timer = new Timer(gameTimeMilliseconds);
            _timer.TimeOut += OnTimeOut;
        }

        private void OnTimeOut()
        {
            GameOver?.Invoke();
        }

        private void OnTileDeleted(Tile tile, TilePosition position)
        {
            _score += tile.Prize;
        }

        public override void Update(GameTime gameTime)
        {
            _boardView.Update(gameTime);
            _timer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _boardView.Draw(spriteBatch);
            spriteBatch.DrawString(Font, $"Score: {_score}", _scorePosition, Color.White);
            spriteBatch.DrawString(Font, $"Timer: {_timer.SecondsLeft}", _timerPosition, Color.White);
        }

        public override void MouseClick(Vector2 position)
        {
            _boardView.MouseClick(position);
        }
    }
}