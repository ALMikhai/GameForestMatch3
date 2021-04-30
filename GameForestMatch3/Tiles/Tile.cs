using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Tiles
{
    public enum TileType
    {
        Candy,
        Cake,
        Cookies,
        IceCream,
        Сroissant
    }

    public abstract class Tile
    {
        public abstract TileType Type { get; }
        public abstract int Prize { get; }
        public bool IsSelected { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}