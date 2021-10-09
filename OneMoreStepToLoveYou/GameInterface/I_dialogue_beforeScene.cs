using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;
using Microsoft.Xna.Framework.Content;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_dialogue_beforeScene : I_gameInterface
    {
        public int DrawOrder { get; set; }
        Texture2D bg;
        bool isDialogueOn = false;

        string chapter;
        text chapterText;
        float textAlpha = 0;
        float textFadeSpeed = 0.03f;
        float deltaTime = 0;
        bool fadeflag = false;

        float countDownTime = 1f;
        bool isstartCountDown = false;

        public I_dialogue_beforeScene(Texture2D bg, string chapter, ContentManager content)
        {
            this.bg = bg;

            this.chapter = chapter;
            Vector2 textPosition = new Vector2((1920 / 2) - content.Load<SpriteFont>("ChapterText").MeasureString(chapter).X / 2, 1080 / 2 - 100);
            chapterText = new text(content.Load<SpriteFont>("ChapterText"), Color.White * 0, textPosition);
            //Game1.dialouge.dialogeOn();
        }

        public void Update(float animator_elapsed)
        {
            if (isDialogueOn)
                return;

            deltaTime += animator_elapsed;
            if (!isstartCountDown)
            {
                if (deltaTime >= textFadeSpeed)
                {
                    if(!fadeflag)
                    {
                        textAlpha += textFadeSpeed;
                        chapterText.fontColor = Color.White * textAlpha;
                        deltaTime = 0;
                        if (textAlpha >= 1)
                            fadeflag = true;
                    }
                    else
                    {
                        textAlpha -= textFadeSpeed;
                        chapterText.fontColor = Color.White * textAlpha;
                        deltaTime = 0;
                        if (textAlpha <= 0)
                            isstartCountDown = true;
                    }
                }
            }
            else
            {
                if(deltaTime >= countDownTime)
                {
                    //Game1.dialouge.transitionSpeed = 0.03f;
                    Game1.dialouge.dialogeOn();
                    isDialogueOn = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!isDialogueOn)
            {
                chapterText.drawFont(spriteBatch, chapter);
                return;
            }

            spriteBatch.Draw(bg, Vector2.Zero, Color.White);
        }

    }
}
