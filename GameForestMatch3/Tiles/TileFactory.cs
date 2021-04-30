using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Tiles
{
    public static class TileFactory
    {
        public static ContentManager ContentManager { get; set; }

        private static readonly Dictionary<TileType, Func<Tile>> TypeToObject = new Dictionary<TileType, Func<Tile>>()
        {
            {TileType.Candy, () => new Candy()},
            {TileType.Cake, () => new Cake()},
            {TileType.Cookies, () => new Cookies()},
            {TileType.Сroissant, () => new Сroissant()},
            {TileType.IceCream, () => new IceCream()},
        };

        private static readonly Dictionary<TileType, string> TypeToContentNames = new Dictionary<TileType, string>()
        {
            {TileType.Candy, "Candy"},
            {TileType.Cake, "Cake"},
            {TileType.Cookies, "Cookies"},
            {TileType.Сroissant, "Сroissant"},
            {TileType.IceCream, "IceCream"},
        };

        private static readonly Dictionary<TileType, Texture2D> TileTextures = new Dictionary<TileType, Texture2D>()
        {
            {TileType.Candy, null},
            {TileType.Cake, null},
            {TileType.Cookies, null},
            {TileType.Сroissant, null},
            {TileType.IceCream, null},
        };

        public static Tile CreateTile(TileType tileType)
        {
            var tile = TypeToObject[tileType]();
            return tile;
        }

        public static TileView CreateTileView(Tile tile)
        {
            return new TileView() {Tile = tile, Texture = GetTexture(tile.Type)};
        }

        public static Texture2D GetTexture(TileType tileType)
        {
            if (TileTextures[tileType] == null)
                TileTextures[tileType] = ContentManager.Load<Texture2D>(TypeToContentNames[tileType]);

            return TileTextures[tileType];
        }
    }
}