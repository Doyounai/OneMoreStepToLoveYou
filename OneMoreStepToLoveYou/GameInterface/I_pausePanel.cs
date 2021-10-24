using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;
using Microsoft.Xna.Framework.Input;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_pausePanel : I_gameInterface
    {
        public int DrawOrder { get; set; }
        Sprite background;

        button reusumeButtom;
        button restartButtom;
        button exitButton;
        button menuButton;

        public bool is_acitve = false;

        public I_pausePanel(GraphicsDeviceManager graphic, ContentManager content)
        {
            Texture2D backgroundTexture = kaninKitRail.getBoxTexture(graphic, 1920, 1080, Color.Black * 0.8f, 0);
            background = new Sprite(backgroundTexture, Vector2.Zero, Color.White);

            SpriteFont font = content.Load<SpriteFont>("debugFont");
            Vector2 positionFitst = new Vector2(1920 / 2 - 220 / 2, 300);
            reusumeButtom = new button(graphic, font, 220, 100, 10, positionFitst, "resume", Color.White, Color.GhostWhite, Color.Salmon, Color.Orange);
            reusumeButtom.Click += resume;
            restartButtom = new button(graphic, font, 220, 100, 10, positionFitst + new Vector2(0, 150), "restart", Color.White, Color.GhostWhite, Color.Salmon, Color.Orange);
            restartButtom.Click += restart;
            menuButton = new button(graphic, font, 220, 100, 10, positionFitst + new Vector2(0, 300), "menu", Color.White, Color.GhostWhite, Color.Salmon, Color.Orange);
            menuButton.Click += menu;
            exitButton = new button(graphic, font, 220, 100, 10, positionFitst + new Vector2(0, 450), "exit", Color.White, Color.GhostWhite, Color.Salmon, Color.Orange);
            exitButton.Click += exit;
        }

        public void Update(float animator_elapsed)
        {
            if (!is_acitve)
                return;
            keyboard.GetState();
            if (keyboard.HasBeenPressed(Keys.Escape))
            {
                is_acitve = false;
                gameManager.is_PAUSE = false;
            }
            reusumeButtom.Update();
            restartButtom.Update();
            menuButton.Update();
            exitButton.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!is_acitve)
                return;
            background.Draw(spriteBatch);
            reusumeButtom.Draw(spriteBatch);
            restartButtom.Draw(spriteBatch);
            menuButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }

        private void resume(object sender, System.EventArgs e)
        {
            is_acitve = false;
            gameManager.is_PAUSE = false;
        }

        private void restart(object sender, System.EventArgs e)
        {
            Game1.changeSceneTo(Game1.currentLevel);
            is_acitve = false;
            gameManager.is_PAUSE = false;

        }

        private void exit(object sender, System.EventArgs e)
        {
            Game1.self.Exit();
        }

        private void menu(object sender, System.EventArgs e)
        {
            Game1.changeSceneTo(101);
            is_acitve = false;
            gameManager.is_PAUSE = false;
        }

        public void Activate()
        {
            is_acitve = true;
            gameManager.is_PAUSE = true;
        }
    }
}
