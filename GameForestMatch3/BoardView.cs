using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3
{
    public class BoardView
    {
        private readonly Vector2 _offset;

        public BoardView(BoardModel boardModel, Vector2 offset)
        {
            BoardModel = boardModel;
            _offset = offset;
        }

        public BoardModel BoardModel { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
                BoardModel.Matrix[row, col]?.Draw(spriteBatch, GetTileDrawPosition(row, col));
        }

        public void Update(GameTime gameTime)
        {
            BoardModel.Update();
        }

        private Vector2 GetTileDrawPosition(int row, int col)
        {
            return new Vector2(_offset.X + Tile.TextureSize.X * col, _offset.Y + Tile.TextureSize.Y * row);
        }

        public void MouseClick(Vector2 position)
        {
            var tilePosition = TilePositionByMouseClick(position);
            BoardModel.ClickByTile(tilePosition);
        }

        private BoardModel.TilePosition TilePositionByMouseClick(Vector2 position)
        {
            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
            {
                var tile = BoardModel.Matrix[row, col];
                if (tile == null)
                    continue;
                var tilePosition = GetTileDrawPosition(row, col);
                if (new Rectangle(tilePosition.ToPoint(), Tile.TextureSize.ToPoint()).Contains(position))
                    return new BoardModel.TilePosition(row, col);
            }

            return null;
        }
    }
}