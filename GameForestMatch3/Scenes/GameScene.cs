using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Scenes
{
    public class GameScene : Scene
    {
        private BoardView _boardView;
        private int _score;
        private Vector2 _scorePosition = new Vector2(25, 25);

        public GameScene(int boardRows, int boardColumns, SpriteFont font) : base(font)
        {
            _boardView = new BoardView(new BoardModel(boardRows, boardColumns), new Vector2(440, 25));
            _boardView.BoardModel.TileDeleted += OnTileDeleted;
        }

        private void OnTileDeleted(Tile tile, TilePosition position)
        {
            _score += tile.Prize;
        }

        public override void Update(GameTime gameTime)
        {
            _boardView.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _boardView.Draw(spriteBatch);
            spriteBatch.DrawString(Font, $"Score: {_score}", _scorePosition, Color.White);
        }

        public override void MouseClick(Vector2 position)
        {
            _boardView.MouseClick(position);
        }
    }
}