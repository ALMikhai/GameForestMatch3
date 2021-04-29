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
        public static Vector2 TextureSize { get; } = new Vector2(130, 124); // Size of texture.
        public Texture2D Texture { get; set; }

        public abstract TileType Type { get; }
        public abstract int Prize { get; }
        public abstract string ContentName { get; }
        public bool IsSelected { get; set; } = false;

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, IsSelected ? Color.Gray : Color.White);
        }
    }
}