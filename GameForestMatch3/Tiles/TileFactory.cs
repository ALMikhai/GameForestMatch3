using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Tiles
{
    public static class TileFactory
    {
        public static ContentManager ContentManager { get; set; }

        private static readonly Dictionary<TileType, Func<Tile>> TileToObject = new Dictionary<TileType, Func<Tile>>()
        {
            {TileType.Candy, () => new Candy()},
            {TileType.Cake, () => new Cake()},
            {TileType.Cookies, () => new Cookies()},
            {TileType.Сroissant, () => new Сroissant()},
            {TileType.IceCream, () => new IceCream()},
        };
        
        public static Tile CreateTile(TileType tileType)
        {
            var tile = TileToObject[tileType]();
            tile.Texture = ContentManager.Load<Texture2D>(tile.ContentName);
            return tile;
        }
    }
}