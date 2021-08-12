using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OneMoreStepToLoveYou.Entites
{
    public class button
    {
        private Sprite button_bg;
        private text button_text;

        public string messege;

        private Color bg_idleColor;
        private Color bg_hoverColor;
        private Color text_idleColor;
        private Color text_hoverColor;

        private bool is_hover;
        public event EventHandler Click;

        //mouse
        private MouseState _previousMouse;
        private MouseState _currentMouse;

        public Rectangle buttonRectangle
        {
            get
            {
                return new Rectangle((int)button_bg.position.X, (int)button_bg.position.Y, button_bg.gameSprite.Width, button_bg.gameSprite.Height);
            }
        }

        public button(GraphicsDeviceManager graphics, SpriteFont font, int b_widht, int b_height, int strok, Vector2 position, string messege, Color t_idleColor, Color t_hoverColor, Color b_idleColor, Color b_hoverColor)
        {
            this.messege = messege;
            this.text_idleColor = t_idleColor;
            this.text_hoverColor = t_hoverColor;
            this.bg_idleColor = b_idleColor;
            this.bg_hoverColor = b_hoverColor;

            Texture2D bg = kaninKitRail.getBoxTexture(graphics, b_widht, b_height, Color.White, strok);
            button_bg = new Sprite(bg, position, bg_idleColor);

            var x = (buttonRectangle.X + (buttonRectangle.Width / 2)) - (font.MeasureString(messege).X / 2);
            var y = (buttonRectangle.Y + (buttonRectangle.Height / 2)) - (font.MeasureString(messege).Y / 2);
            button_text = new text(font, text_idleColor, new Vector2(x, y));
        }

        public void Update()
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            is_hover = false;

            if (mouseRectangle.Intersects(this.buttonRectangle))
            {
                is_hover = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(is_hover)
            {
                button_bg.tintColor = bg_hoverColor;
                button_text.fontColor = text_hoverColor;
            }
            else
            {
                button_bg.tintColor = bg_idleColor;
                button_text.fontColor = text_idleColor;
            }

            button_bg.Draw(spriteBatch);
            button_text.drawFont(spriteBatch, messege);
        }
    }
}
