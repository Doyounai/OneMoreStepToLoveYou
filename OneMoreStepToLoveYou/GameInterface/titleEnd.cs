using Microsoft.Xna.Framework;
using OneMoreStepToLoveYou.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class titleEnd : I_gameInterface
    {
        public int DrawOrder { get; set; }

        private button startButton;
        private button exit;

        //level btn
        private button level1_button;
        private button level2_button;
        private button level3_button;
        private button level4_button;
        private button level5_button;

        private static int start_lv = 11;

        public Sprite bg;

        public titleEnd(GraphicsDeviceManager graphics, SpriteFont font, Texture2D bg)
        {
            startButton = new button(graphics, font, 220, 100, 10, new Vector2(100, 640), "Start", Color.White, Color.GhostWhite, Color.Salmon, Color.Orange);
            startButton.Click += startOnClick;
            exit = new button(graphics, font, 220, 100, 10, new Vector2(100, 760), "Exit", Color.White, Color.GhostWhite, Color.Salmon, Color.Orange);
            exit.Click += exitGame;

            level1_button = new button(graphics, font, 220, 100, 10, new Vector2(350, 640), "Level1", Color.White, Color.GhostWhite, Color.BurlyWood, Color.Brown);
            level1_button.Click += startOnClick;
            level2_button = new button(graphics, font, 220, 100, 10, new Vector2(600, 640), "Level2", Color.White, Color.GhostWhite, Color.BurlyWood, Color.Brown);
            level2_button.Click += level2;
            level3_button = new button(graphics, font, 220, 100, 10, new Vector2(850, 640), "Level3", Color.White, Color.GhostWhite, Color.BurlyWood, Color.Brown);
            level3_button.Click += level3;
            level4_button = new button(graphics, font, 220, 100, 10, new Vector2(350, 760), "Level4", Color.White, Color.GhostWhite, Color.BurlyWood, Color.Brown);
            level4_button.Click += level4;
            level5_button = new button(graphics, font, 220, 100, 10, new Vector2(600, 760), "Level5", Color.White, Color.GhostWhite, Color.BurlyWood, Color.Brown);
            level5_button.Click += level5;

            this.bg = new Sprite(bg, Vector2.Zero, Color.White);
            MediaPlayer.Play(Game1.titleSong);
        }

        private static void startOnClick(object sender, System.EventArgs e)
        {
            Game1.playSound(Game1.Click);
            Game1.changeSceneTo(start_lv);
        }

        private static void exitGame(object sender, System.EventArgs e)
        {
            Game1.self.Exit();
        }

        #region scene change event

        private static void level2(object sender, System.EventArgs e)
        {
            Game1.playSound(Game1.Click);
            Game1.changeSceneTo(22);
        }
        private static void level3(object sender, System.EventArgs e)
        {
            Game1.playSound(Game1.Click);
            Game1.changeSceneTo(44);
        }

        private static void level4(object sender, System.EventArgs e)
        {
            Game1.playSound(Game1.Click);
            Game1.changeSceneTo(55);
        }

        private static void level5(object sender, System.EventArgs e)
        {
            Game1.playSound(Game1.Click);
            Game1.changeSceneTo(66);
        }

        #endregion

        public void Update(float animator_elapsed)
        {
            startButton.Update();
            exit.Update();

            level1_button.Update();
            level2_button.Update();
            level3_button.Update();
            level4_button.Update();
            level5_button.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bg.Draw(spriteBatch);
            startButton.Draw(spriteBatch);
            level1_button.Draw(spriteBatch);
            level2_button.Draw(spriteBatch);
            level3_button.Draw(spriteBatch);
            level4_button.Draw(spriteBatch);
            level5_button.Draw(spriteBatch);
            exit.Draw(spriteBatch);
        }
    }
}
