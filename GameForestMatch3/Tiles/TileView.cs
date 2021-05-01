using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Tiles
{
    public class TileView
    {
        public static Vector2 TextureSize { get; } = new Vector2(132, 132); // Size of texture.
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Tile Tile { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Tile.IsSelected ? Color.Gray : Color.White);
        }
    }
}