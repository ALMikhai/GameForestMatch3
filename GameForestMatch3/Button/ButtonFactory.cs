using System.Drawing.Printing;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameForestMatch3.Button
{
    public static class ButtonFactory
    {
        public static ContentManager ContentManager { get; set; }
        private static Texture2D _buttonTexture2D = null;
        private static SoundEffect _clickSound = null;

        public static TextureButton CreateButton(SpriteFont font, string text)
        {
            if (_buttonTexture2D == null)
                _buttonTexture2D = ContentManager.Load<Texture2D>("Button");
            
            if (_clickSound == null)
                _clickSound = ContentManager.Load<SoundEffect>("ButtonClick");

            return new TextureButton(_buttonTexture2D, font, text) {ClickSound = _clickSound};
        }
    }
}