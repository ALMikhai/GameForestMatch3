using System;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3
{
    public class Board
    {
        private readonly int _columns;
        private readonly Vector2 _offset;
        private readonly int _rows;
        private State _currentState;

        private Tile[,] _matrix;

        private Tuple<int, int> _selectedTileIndex;

        public Board(int rows, int columns, Vector2 offset)
        {
            _rows = rows;
            _columns = columns;
            _offset = offset;
            FillMatrix();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in _matrix) tile.Draw(spriteBatch);
        }

        public void Update()
        {
        }

        public void MouseClick(Vector2 position)
        {
            switch (_currentState)
            {
                case State.Start:
                {
                    var tileIndex = TileByMouseClick(position);
                    if (tileIndex != null)
                    {
                        _selectedTileIndex = tileIndex;
                        _matrix[_selectedTileIndex.Item1, _selectedTileIndex.Item2].IsSelected = true;
                        ChangeState(State.FirstSelected);
                    }

                    break;
                }
                case State.FirstSelected:
                {
                    _matrix[_selectedTileIndex.Item1, _selectedTileIndex.Item2].IsSelected = false;
                    ChangeState(State.Start);

                    var tileIndex = TileByMouseClick(position);
                    if (tileIndex != null && tileIndex != _selectedTileIndex)
                        if (IsNearTile(tileIndex, _selectedTileIndex))
                            SwapTiles(tileIndex, _selectedTileIndex);

                    break;
                }
                case State.Backward:
                    break;
                case State.Matches:
                    break;
                case State.ShiftTiles:
                    break;
            }
        }

        private void FillMatrix()
        {
            _matrix = new Tile[_rows, _columns];
            var rand = new Random();
            var drawPos = _offset;

            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _columns; j++)
                {
                    var tileNumber = rand.Next(0, 5);
                    var tile = TileFactory.CreateTile((TileType) tileNumber);
                    tile.Position = drawPos;
                    drawPos.X += Tile.TextureSize.X;
                    _matrix[i, j] = tile;
                }

                drawPos.X = _offset.X;
                drawPos.Y += Tile.TextureSize.Y;
            }
        }

        private Tuple<int, int> TileByMouseClick(Vector2 position)
        {
            for (var i = 0; i < _rows; i++)
            for (var j = 0; j < _columns; j++)
            {
                var tile = _matrix[i, j];
                if (new Rectangle(tile.Position.ToPoint(), Tile.TextureSize.ToPoint()).Contains(position))
                    return new Tuple<int, int>(i, j);
            }

            return null;
        }

        private void ChangeState(State state)
        {
            _currentState = state;
        }

        private bool IsNearTile(Tuple<int, int> index1, Tuple<int, int> index2)
        {
            return Math.Abs(index1.Item1 - index2.Item1) + Math.Abs(index1.Item2 - index2.Item2) < 2;
        }

        private void SwapTiles(Tuple<int, int> index1, Tuple<int, int> index2)
        {
            var pos1 = _matrix[index1.Item1, index1.Item2].Position;
            var pos2 = _matrix[index2.Item1, index2.Item2].Position;

            var tmp = _matrix[index1.Item1, index1.Item2];
            _matrix[index1.Item1, index1.Item2] = _matrix[index2.Item1, index2.Item2];
            _matrix[index2.Item1, index2.Item2] = tmp;

            _matrix[index1.Item1, index1.Item2].Position = pos1;
            _matrix[index2.Item1, index2.Item2].Position = pos2;
        }

        private enum State
        {
            Start,
            FirstSelected,
            Backward,
            Matches,
            ShiftTiles
        }
    }
}