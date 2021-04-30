using System.Collections.Generic;
using System.Windows.Forms;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3
{
    public class BoardView
    {
        public BoardModel BoardModel { get; }

        private readonly TileView[,] _matrix;
        private readonly List<MoveTileAnimation> _animations;
        private readonly Vector2 _offset;

        public BoardView(BoardModel boardModel, Vector2 offset)
        {
            BoardModel = boardModel;
            _offset = offset;
            _animations = new List<MoveTileAnimation>();
            _matrix = new TileView[BoardModel.Rows, BoardModel.Columns];

            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
            {
                var tile = BoardModel.Matrix[row, col];
                var tileView = TileFactory.CreateTileView(tile);
                tileView.Position = GetTileDrawPosition(row, col);
                _matrix[row, col] = tileView;
            }

            BoardModel.TileSwapped += BoardModelOnTileSwapped;
            BoardModel.TileSpawned += BoardModelOnTileSpawned;
            BoardModel.TileDeleted += BoardModelOnTileDeleted;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
                _matrix[row, col].Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (_animations.Count != 0)
            {
                foreach (var animation in _animations) animation.Update(gameTime);

                _animations.RemoveAll(animation => animation.IsFinished);
                return;
            }

            BoardModel.Update();
        }

        public void MouseClick(Vector2 position)
        {
            if (_animations.Count != 0)
                return;
            var tilePosition = TilePositionByMouseClick(position);
            BoardModel.ClickByTile(tilePosition);
        }

        private void BoardModelOnTileDeleted(Tile tile, TilePosition position)
        {
            var destination = new Vector2(-TileView.TextureSize.X * 2, -TileView.TextureSize.Y * 2);
            _animations.Add(new MoveTileAnimation(250, _matrix[position.Row, position.Col], destination));
        }

        private void BoardModelOnTileSpawned(Tile tile, TilePosition position)
        {
            var destination = GetTileDrawPosition(position.Row, position.Col);
            var tileView = TileFactory.CreateTileView(tile);
            tileView.Position = new Vector2(destination.X, -TileView.TextureSize.Y);
            _matrix[position.Row, position.Col] = tileView;
            _animations.Add(new MoveTileAnimation(250, _matrix[position.Row, position.Col], destination));
        }

        private void BoardModelOnTileSwapped(TilePosition position1, TilePosition position2) // TODO do this simple. 
        {
            if (_matrix[position1.Row, position1.Col].Tile.IsDeleted == false)
            {
                var destination = GetTileDrawPosition(position2.Row, position2.Col);
                _animations.Add(new MoveTileAnimation(750, _matrix[position1.Row, position1.Col],
                    destination));
            }

            if (_matrix[position2.Row, position2.Col].Tile.IsDeleted == false)
            {
                var destination = GetTileDrawPosition(position1.Row, position1.Col);
                _animations.Add(new MoveTileAnimation(750, _matrix[position2.Row, position2.Col],
                    destination));
            }

            var tmp = _matrix[position1.Row, position1.Col];
            _matrix[position1.Row, position1.Col] = _matrix[position2.Row, position2.Col];
            _matrix[position2.Row, position2.Col] = tmp;
        }

        private Vector2 GetTileDrawPosition(int row, int col)
        {
            return new Vector2(_offset.X + TileView.TextureSize.X * col, _offset.Y + TileView.TextureSize.Y * row);
        }

        private TilePosition TilePositionByMouseClick(Vector2 position)
        {
            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
            {
                var tile = BoardModel.Matrix[row, col];
                if (tile.IsDeleted)
                    continue;
                var tilePosition = GetTileDrawPosition(row, col);
                if (new Rectangle(tilePosition.ToPoint(), TileView.TextureSize.ToPoint()).Contains(position))
                    return new TilePosition(row, col);
            }

            return null;
        }
    }
}