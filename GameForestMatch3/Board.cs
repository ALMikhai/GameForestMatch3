using System;
using System.Collections.Generic;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3
{
    public class Board
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly Vector2 _offset;
        private List<TilePosition> _currentMatch;
        private State _currentState;

        private Tile[,] _matrix;
        private TilePosition _firstSelectedTile;
        private TilePosition _secondSelectedTile;

        private readonly RandomTilesGenerator _tilesGenerator;

        public Board(int rows, int columns, Vector2 offset)
        {
            _rows = rows;
            _columns = columns;
            _offset = offset;
            _tilesGenerator = new RandomTilesGenerator(rows, columns);
            FillMatrix();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _columns; col++)
                _matrix[row, col]?.Draw(spriteBatch, GetTileDrawPosition(row, col));
        }

        public void Update()
        {
            switch (_currentState)
            {
                case State.SwapTiles:
                {
                    SwapTiles(_firstSelectedTile, _secondSelectedTile);
                    ChangeState(State.Backward);
                    break;
                }
                case State.Backward:
                {
                    _currentMatch = FindMatch(_secondSelectedTile);
                    if (_currentMatch == null)
                    {
                        SwapTiles(_firstSelectedTile, _secondSelectedTile);
                        ChangeState(State.Start);
                    }
                    else
                    {
                        ChangeState(State.DeleteMatch);
                    }

                    break;
                }
                case State.DeleteMatch:
                {
                    foreach (var tilePosition in _currentMatch) _matrix[tilePosition.Row, tilePosition.Col] = null;
                    ChangeState(State.ShiftTiles);
                    break;
                }
                case State.FillTiles:
                {
                    for (var row = 0; row < _rows; row++)
                    for (var col = 0; col < _columns; col++)
                        if (_matrix[row, col] == null)
                            _matrix[row, col] = _tilesGenerator.GetNext();
                    ChangeState(State.Start);
                }
                    break;
                case State.ShiftTiles:
                {
                    for (var row = _rows - 1; row > 0; row--)
                    for (var col = 0; col < _columns; col++)
                        if (_matrix[row, col] == null)
                            SwapTiles(new TilePosition(row, col), new TilePosition(row - 1, col));
                    ChangeState(State.FillTiles);
                    break;
                }
            }
        }

        public void MouseClick(Vector2 position)
        {
            switch (_currentState)
            {
                case State.Start:
                {
                    var tilePosition = TileByMouseClick(position);
                    if (tilePosition != null)
                    {
                        _firstSelectedTile = tilePosition;
                        _matrix[_firstSelectedTile.Row, _firstSelectedTile.Col].IsSelected = true;
                        ChangeState(State.FirstSelected);
                    }

                    break;
                }
                case State.FirstSelected:
                {
                    _matrix[_firstSelectedTile.Row, _firstSelectedTile.Col].IsSelected = false;
                    var tilePosition = TileByMouseClick(position);
                    if (tilePosition != null && IsNearTile(tilePosition, _firstSelectedTile))
                    {
                        _secondSelectedTile = tilePosition;
                        ChangeState(State.SwapTiles);
                    }
                    else
                    {
                        ChangeState(State.Start);
                    }

                    break;
                }
            }
        }

        private void FillMatrix()
        {
            _matrix = _tilesGenerator.GetInitMatrix();
        }

        private TilePosition TileByMouseClick(Vector2 position)
        {
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _columns; col++)
            {
                var tile = _matrix[row, col];
                if (tile == null)
                    continue;
                var tilePosition = GetTileDrawPosition(row, col);
                if (new Rectangle(tilePosition.ToPoint(), Tile.TextureSize.ToPoint()).Contains(position))
                    return new TilePosition(row, col);
            }

            return null;
        }

        private void ChangeState(State state)
        {
            _currentState = state;
        }

        private bool IsNearTile(TilePosition position1, TilePosition position2)
        {
            return Math.Abs(position1.Row - position2.Row) + Math.Abs(position1.Col - position2.Col) < 2;
        }

        private void SwapTiles(TilePosition position1, TilePosition position2)
        {
            var tmp = _matrix[position1.Row, position1.Col];
            _matrix[position1.Row, position1.Col] = _matrix[position2.Row, position2.Col];
            _matrix[position2.Row, position2.Col] = tmp;
        }

        private List<TilePosition> FindMatch(TilePosition position) // TODO do this simple.
        {
            var positions = new List<TilePosition> {position};
            var checkedTile = _matrix[position.Row, position.Col];
            for (var i = position.Col + 1; i < _columns; i++)
            {
                if (_matrix[position.Row, i]?.Type == checkedTile.Type)
                {
                    positions.Add(new TilePosition(position.Row, i));
                    continue;
                }

                break;
            }

            for (var i = position.Col - 1; i >= 0; i--)
            {
                if (_matrix[position.Row, i]?.Type == checkedTile.Type)
                {
                    positions.Add(new TilePosition(position.Row, i));
                    continue;
                }

                break;
            }

            if (positions.Count >= 3) return positions;

            return null;
        }

        private Vector2 GetTileDrawPosition(int row, int col)
        {
            return new Vector2(_offset.X + Tile.TextureSize.X * col, _offset.Y + Tile.TextureSize.Y * row);
        }

        private class TilePosition
        {
            public TilePosition(int row, int col)
            {
                Row = row;
                Col = col;
            }

            public int Row { get; }
            public int Col { get; }
        }

        private enum State
        {
            Start,
            FirstSelected,
            SwapTiles,
            Backward,
            DeleteMatch,
            FillTiles,
            ShiftTiles
        }
    }
}