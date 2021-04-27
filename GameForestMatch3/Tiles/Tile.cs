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
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        
        public abstract TileType Type { get; }
        public abstract int Prize { get; }
        public abstract string ContentName { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}