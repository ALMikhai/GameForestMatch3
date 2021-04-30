using System.Collections.Generic;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3
{
    public class BoardView
    {
        public BoardModel BoardModel { get; }

        private readonly List<MoveTileAnimation> _animations;
        private readonly Vector2 _offset;

        public BoardView(BoardModel boardModel, Vector2 offset)
        {
            BoardModel = boardModel;
            _offset = offset;
            _animations = new List<MoveTileAnimation>();

            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
                BoardModel.Matrix[row, col].Position = GetTileDrawPosition(row, col);

            BoardModel.TileSwapped += BoardModelOnTileSwapped;
            BoardModel.TileSpawned += BoardModelOnTileSpawned;
            BoardModel.TileDeleted += BoardModelOnTileDeleted;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var row = 0; row < BoardModel.Rows; row++)
            for (var col = 0; col < BoardModel.Columns; col++)
                BoardModel.Matrix[row, col]?.Draw(spriteBatch, BoardModel.Matrix[row, col].Position);
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
            var destination = new Vector2(-Tile.TextureSize.X * 2, -Tile.TextureSize.Y * 2);
            _animations.Add(new MoveTileAnimation(250, tile, destination));
        }

        private void BoardModelOnTileSpawned(Tile tile, TilePosition position)
        {
            var destination = GetTileDrawPosition(position.Row, position.Col);
            tile.Position = new Vector2(destination.X, -Tile.TextureSize.Y);
            _animations.Add(new MoveTileAnimation(250, tile, destination));
        }

        private void BoardModelOnTileSwapped(TilePosition position1, TilePosition position2)
        {
            var destination = GetTileDrawPosition(position2.Row, position2.Col);
            _animations.Add(new MoveTileAnimation(750, BoardModel.Matrix[position1.Row, position1.Col],
                destination));
        }

        private Vector2 GetTileDrawPosition(int row, int col)
        {
            return new Vector2(_offset.X + Tile.TextureSize.X * col, _offset.Y + Tile.TextureSize.Y * row);
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
                if (new Rectangle(tilePosition.ToPoint(), Tile.TextureSize.ToPoint()).Contains(position))
                    return new TilePosition(row, col);
            }

            return null;
        }
    }
}