using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.Entites
{
    public class crowdRacing : I_gameInterface
    {
        public int DrawOrder { get; set; }
        private float speed;
        private Sprite sprite;

        AnimatedTexture animator;

        public crowdRacing(Texture2D texture, int column, float speed, Texture2D animationSprite)
        {
            Vector2 position = gameManager.GRID_DATA[0, column].getCenterGridPosition;
            position -= kaninKitRail.getCenterPoint(texture.Width, texture.Height);
            position.Y = -texture.Height;
            //position.Y = 1080;
            this.sprite = new Sprite(texture, position, Color.White);
            this.speed = speed;

            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animator.Load(animationSprite, 4, 4, 15);
        }

        public void Update(float animator_elapsed)
        {
            //breakcase
            if((sprite.position.Y <= -120 && gameManager.racingGameManager.isBreak))
                Game1.scene.Remove(this);

            if (sprite.position.Y <= 1080)
            {
                if (!gameManager.racingGameManager.isBreak)
                    sprite.position.Y += speed;
                if (gameManager.racingGameManager.isBreak)
                    sprite.position.Y -= speed;
            }
            else if(sprite.position.Y > 1080)
                Game1.scene.Remove(this);

            animator.UpdateFrame(animator_elapsed);

            float scaleDown = 0.7f;
            Rectangle playerRec = gameManager.racingPlayer.sprite.rec;
            playerRec.Width = (int)(playerRec.Width * scaleDown);
            playerRec.Height = (int)(playerRec.Width * scaleDown);

            if(!gameManager.racingGameManager.isBreak && sprite.rec.Intersects(playerRec))
            {
                gameManager.racingGameManager.เบรครถ();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sprite.Draw(spriteBatch);
            animator.DrawFrame(spriteBatch, sprite.position, 3);
        }
    }
}
