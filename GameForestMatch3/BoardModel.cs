﻿using System;
using System.Collections.Generic;
using GameForestMatch3.Tiles;

namespace GameForestMatch3
{
    public class TilePosition
    {
        public TilePosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }
        public int Col { get; }
    }

    public class BoardModel
    {
        public delegate void TileEditEventHandler(Tile tile, TilePosition position);
        public delegate void SwapTileEventHandler(TilePosition position1, TilePosition position2);
        
        public int Rows { get; }
        public int Columns { get; }

        public Tile[,] Matrix { get; }
        public event SwapTileEventHandler TileSwapped;
        public event TileEditEventHandler TileSpawned;
        public event TileEditEventHandler TileDeleted;

        private readonly RandomTilesGenerator _tilesGenerator;
        private List<TilePosition> _currentMatch;
        private State _currentState;
        private TilePosition _firstSelectedTile;
        private TilePosition _secondSelectedTile;

        public BoardModel(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _tilesGenerator = new RandomTilesGenerator(rows, columns);
            Matrix = _tilesGenerator.GetInitMatrix();
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
                    foreach (var tilePosition in _currentMatch)
                    {
                        TileDeleted?.Invoke(Matrix[tilePosition.Row, tilePosition.Col], tilePosition);
                        Matrix[tilePosition.Row, tilePosition.Col].IsDeleted = true;
                    }
                    ChangeState(State.ShiftTiles);
                    break;
                }
                case State.FillTiles:
                {
                    for (var row = 0; row < Rows; row++)
                    for (var col = 0; col < Columns; col++)
                        if (Matrix[row, col].IsDeleted)
                        {
                            Matrix[row, col] = _tilesGenerator.GetNext();
                            TileSpawned?.Invoke(Matrix[row, col], new TilePosition(row, col));
                        }

                    ChangeState(State.Start);
                }
                    break;
                case State.ShiftTiles:
                {
                    for (var row = Rows - 1; row > 0; row--)
                    for (var col = 0; col < Columns; col++)
                        if (Matrix[row, col].IsDeleted)
                            SwapTiles(new TilePosition(row - 1, col), new TilePosition(row, col));
                    ChangeState(State.FillTiles);
                    break;
                }
            }
        }

        public void ClickByTile(TilePosition tilePosition)
        {
            switch (_currentState)
            {
                case State.Start:
                {
                    if (tilePosition != null)
                    {
                        _firstSelectedTile = tilePosition;
                        Matrix[_firstSelectedTile.Row, _firstSelectedTile.Col].IsSelected = true;
                        ChangeState(State.FirstSelected);
                    }

                    break;
                }
                case State.FirstSelected:
                {
                    Matrix[_firstSelectedTile.Row, _firstSelectedTile.Col].IsSelected = false;
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
            if (Matrix[position1.Row, position1.Col].IsDeleted == false)
                TileSwapped?.Invoke(position1, position2);
            if (Matrix[position2.Row, position2.Col].IsDeleted == false)
                TileSwapped?.Invoke(position2, position1);

            var tmp = Matrix[position1.Row, position1.Col];
            Matrix[position1.Row, position1.Col] = Matrix[position2.Row, position2.Col];
            Matrix[position2.Row, position2.Col] = tmp;
        }

        private List<TilePosition> FindMatch(TilePosition position) // TODO do this simple.
        {
            var positions = new List<TilePosition> {position};
            var checkedTile = Matrix[position.Row, position.Col];
            for (var i = position.Col + 1; i < Columns; i++)
            {
                if (Matrix[position.Row, i]?.Type == checkedTile.Type)
                {
                    positions.Add(new TilePosition(position.Row, i));
                    continue;
                }

                break;
            }

            for (var i = position.Col - 1; i >= 0; i--)
            {
                if (Matrix[position.Row, i]?.Type == checkedTile.Type)
                {
                    positions.Add(new TilePosition(position.Row, i));
                    continue;
                }

                break;
            }

            if (positions.Count >= 3) return positions;

            return null;
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