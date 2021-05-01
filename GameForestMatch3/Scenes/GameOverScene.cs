using GameForestMatch3.Button;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameForestMatch3.Scenes
{
    public class GameOverScene : Scene
    {
        public TextureButton OkButton { get; }

        private string _message = "GameOver";
        private readonly Vector2 _messagePosition;

        public GameOverScene(SpriteFont font) : base(font)
        {
            OkButton = ButtonFactory.CreateButton(font, "Ok");
            OkButton.Position = new Vector2(Game.StandardWidth / 2.0f - OkButton.Rectangle.Width / 2.0f,
                Game.StandardHeight / 2.0f - OkButton.Rectangle.Height / 2.0f);

            _messagePosition = new Vector2(Game.StandardWidth / 2.0f - font.MeasureString(_message).X / 2.0f,
                Game.StandardHeight / 2.0f - font.MeasureString(_message).X / 2.0f);
            _messagePosition -= new Vector2(0, 350);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            OkButton.Draw(spriteBatch);
            spriteBatch.DrawString(Font, _message, _messagePosition, Color.White);
        }

        public override void MouseClick(Vector2 position)
        {
            OkButton.MouseClick(position);
        }
    }
}