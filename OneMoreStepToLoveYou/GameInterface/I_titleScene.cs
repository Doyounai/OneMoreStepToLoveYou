using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_titleScene : I_gameInterface
    {
        public int DrawOrder { get; set; }

        private button startButton;
        private button exit;

        private static int start_lv = 1;

        public Sprite bg;

        public I_titleScene(GraphicsDeviceManager graphics, SpriteFont font, Texture2D bg)
        {
            startButton = new button(graphics, font, 220, 100, 10, new Vector2(360, 640), "Start", Color.Blue, Color.DarkBlue, Color.Gray, Color.DarkGray);
            startButton.Click += startOnClick;
            exit = new button(graphics, font, 220, 100, 10, new Vector2(360, 760), "Exit", Color.Blue, Color.DarkBlue, Color.Gray, Color.DarkGray);
            this.bg = new Sprite(bg, Vector2.Zero, Color.White);
        }

        private static void startOnClick(object sender, System.EventArgs e)
        {
            Game1.changeSceneTo(start_lv);
        }

        public void Update(float animator_elapsed)
        {
            startButton.Update();
            exit.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bg.Draw(spriteBatch);
            startButton.Draw(spriteBatch);
            exit.Draw(spriteBatch);
        }
    }
}
