using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameForestMatch3.Button
{
    public class TextureButton
    {
        public delegate void ClickEventHandler();
        public event ClickEventHandler Click;

        public Vector2 Position { get; set; }
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        public string Text { get; }
        public SoundEffect ClickSound { get; set; } = null;
        
        private SpriteFont _font;
        private Texture2D _texture;

        public TextureButton(Texture2D texture, SpriteFont font, string text)
        {
            _texture = texture;
            _font = font;
            Text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);

            if(!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), Color.White);
            }
        }

        public void MouseClick(Vector2 position)
        {
            if (Rectangle.Contains(position))
            {
                ClickSound?.CreateInstance().Play();
                Click?.Invoke();
            }
        }
    }
}