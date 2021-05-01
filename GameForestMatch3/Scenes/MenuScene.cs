using GameForestMatch3.Button;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Scenes
{
    public class MenuScene : Scene
    {
        public TextureButton PlayButton;

        public MenuScene(SpriteFont font) : base(font)
        {
            PlayButton = ButtonFactory.CreateButton(font, "Play");
            PlayButton.Position = new Vector2(Game.StandardWidth / 2.0f - PlayButton.Rectangle.Width / 2.0f,
                Game.StandardHeight / 2.0f - PlayButton.Rectangle.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PlayButton.Draw(spriteBatch);
        }

        public override void MouseClick(Vector2 position)
        {
            PlayButton.MouseClick(position);
        }
    }
}