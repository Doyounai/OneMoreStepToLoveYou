using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_Credit : I_gameInterface
    {
        public int DrawOrder { get; set; }

        ContentManager content;

        //config
        private float currentSpeed = 2f;

        //Random
        public Random rand = new Random();

        //background
        private int topBackgroundIndex;
        private Sprite[] backgroundSprites = new Sprite[2];
        List<Texture2D> backgroundTextures = new List<Texture2D>();

        //text
        List<text> creditText = new List<text>();
        float textSpeed = 1.2f;
        float textSkipSpeed = 10;
        float startText_Y = 1100;

        float desitination = 155000;
        float currentDistance = 0;

        Texture2D blackPlant;

        public Texture2D newBackgroundTexture
        {
            set
            {
                backgroundTextures.Add(value);
            }
        }

        public I_Credit(ContentManager content, GraphicsDeviceManager graphic)
        {
            this.content = content;
            blackPlant = kaninKitRail.getBoxTexture(graphic, 1920, 1080, Color.Black * 0.6f, 0);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(Game1.gameplaySong);
        }

        public void loadCredit(string credit)
        {
            Vector2 position = new Vector2(
                (1920 / 2) - content.Load<SpriteFont>("creaditFont").MeasureString(credit).X / 2 - 50,
                startText_Y + (content.Load<SpriteFont>("creaditFont").MeasureString(credit).Y + 10) * creditText.Count
                );
            creditText.Add(new text(content.Load<SpriteFont>("creaditFont"), Color.White, position, credit));
        }

        public void Update(float animator_elapsed)
        {
            if(currentDistance >= desitination)
            {
                MediaPlayer.Volume = 0.08f;
                Game1.changeSceneTo(101);
                return;
            }

            keyboard.GetState();
            foreach (text item in creditText)
            {
                if (keyboard.IsPressed(Keys.Space))
                {
                    item.position.Y -= textSkipSpeed;
                    currentDistance += textSkipSpeed;
                }
                else
                {
                    item.position.Y -= textSpeed;
                    currentDistance += textSpeed;
                }
            }

            //background
            for (int i = 0; i < 2; i++)
            {
                backgroundSprites[i].position.Y += currentSpeed;
                if (backgroundSprites[i].position.Y > 1080)
                {
                    backgroundSprites[i].position.Y = backgroundSprites[topBackgroundIndex].position.Y - 1080;
                    backgroundSprites[i].gameSprite = backgroundTextures[rand.Next(0, backgroundTextures.Count - 1)];
                    topBackgroundIndex = i;
                    backgroundSprites[i].position.Y += currentSpeed;
                }
            }
        }

        public void updateBackgrounds()
        {
            topBackgroundIndex = 1;
            backgroundSprites[0] = new Sprite(backgroundTextures[rand.Next(0, backgroundTextures.Count - 1)], Vector2.Zero, Color.White);
            backgroundSprites[1] = new Sprite(backgroundTextures[rand.Next(0, backgroundTextures.Count - 1)], new Vector2(0, -1080), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 2; i++)
            {
                backgroundSprites[i].Draw(spriteBatch);
            }
            spriteBatch.Draw(blackPlant, Vector2.Zero, Color.White);
            foreach (text item in creditText)
            {
                item.drawFont(spriteBatch);
            }
        }
    }
}
