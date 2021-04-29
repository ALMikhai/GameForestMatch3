using System;
using GameForestMatch3.Tiles;

namespace GameForestMatch3
{
    public class RandomTilesGenerator
    {
        private Random _random = new Random(0);
        public int MatrixRows { get; }
        public int MatrixColumns { get; }

        public RandomTilesGenerator(int matrixRows, int matrixColumns)
        {
            MatrixRows = matrixRows;
            MatrixColumns = matrixColumns;
        }
        
        public Tile[,] GetInitMatrix()
        {
            var matrix = new Tile[MatrixRows, MatrixColumns];

            for (var row = 0; row < MatrixRows; row++)
            for (var col = 0; col < MatrixColumns; col++)
            {
                matrix[row, col] = GetNext();
            }

            return matrix;
        }

        public Tile GetNext()
        {
            var tileNumber = _random.Next(0, 5);
            var tile = TileFactory.CreateTile((TileType) tileNumber);
            return tile;
        }
    }
}