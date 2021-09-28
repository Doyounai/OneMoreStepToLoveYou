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

        public crowdRacing(Texture2D texture, int column, float speed)
        {
            Vector2 position = gameManager.GRID_DATA[0, column].getCenterGridPosition;
            position -= kaninKitRail.getCenterPoint(texture.Width, texture.Height);
            position.Y = -texture.Height;
            this.sprite = new Sprite(texture, position, Color.White);
            this.speed = speed;
        }

        public void Update(float animator_elapsed)
        {
            if (sprite.position.Y <= 1080)
                sprite.position.Y += speed;
            else
                Game1.scene.Remove(this);

            if(!gameManager.racingGameManager.isBreak && sprite.rec.Intersects(gameManager.racingPlayer.sprite.rec))
            {
                gameManager.racingGameManager.เบรครถ();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
