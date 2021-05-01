using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Scenes
{
    public abstract class Scene
    {
        public SpriteFont Font { get; }

        public Scene(SpriteFont font)
        {
            Font = font;
        }
        
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void MouseClick(Vector2 position);
    }
}