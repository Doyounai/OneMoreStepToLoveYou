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
    public class starDisplay : I_gameInterface
    {
        public int DrawOrder { get; set; }
        Texture2D star;

        float popupDelay = 0.6f;
        float totalTime = 0;

        int currentStar = 0;
        float starMargin = 50;

        float endDelay = 1f;
        bool is_endDisplay = false;

        public starDisplay(Texture2D star)
        {
            this.star = star;
        }

        public void Update(float animator_elapsed)
        {
            totalTime += animator_elapsed;

            if(totalTime >= popupDelay && currentStar < gameManager.getCurrentStar)
            {
                Game1.playSound(Game1.star, 0.6f);
                currentStar += 1;
                totalTime = 0;
                Vector2 position = new Vector2(1920 / 2 - (star.Width * 3 + starMargin * 2) / 2, (1080 / 2) - (star.Height / 2));
                position.X += ((currentStar - 1) * star.Width + starMargin * (currentStar - 1)) - ((120 / 2) * 5) + star.Width / 2;
                position.Y -= ((120 / 20) * 5) + star.Height / 2 - 20;
                //position.Y += star.Height / 2;
                Game1.scene.Add(new particle(position, 5, "impactDust", 5, 1, 16, false), 0);
                is_endDisplay = true;
            }

            if(is_endDisplay && totalTime >= endDelay)
            {
                /*if (gameManager.currentLevel < 3)
                    Game1.changeSceneTo(gameManager.currentLevel + 1);
                else if (gameManager.currentLevel >= 3 && gameManager.currentLevel <= 4)
                    Game1.changeSceneTo(gameManager.currentLevel + 2);
                else
                    Game1.changeSceneTo(0);*/
                Game1.changeSceneTo(gameManager.sceneNumbrtToGO);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < currentStar; i++)
            {
                //Vector2 startPosition = new Vector2((1920 / 2) - (star.Width / 2) - (starMargin * (currentStar - 1)) - (star.Width * currentStar - 1), 0);
                Vector2 startPosition = new Vector2(1920 / 2 - (star.Width * 3 + starMargin * 2) / 2, 1080 / 2 - star.Height / 2);
                spriteBatch.Draw(star, startPosition + new Vector2(star.Width * i + starMargin * i, 0), Color.White);
            }
        }

    }
}
