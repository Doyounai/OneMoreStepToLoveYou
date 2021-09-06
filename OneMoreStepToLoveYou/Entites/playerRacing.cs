using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMoreStepToLoveYou.GameInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OneMoreStepToLoveYou.Entites
{

    public class playerRacing : character, I_gameInterface
    {
        public int DrawOrder { get; set; }

        public playerRacing(Texture2D texture, gridPosition gridPos)
        {
            this.moveSpeed = 15;
            this.type = gridType.Player;
            sprite = new Sprite(texture, gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition, Color.White);
            m_gridPosition = gridPos;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);
        }

        public void Update(float animator_elapsed)
        {
            if (gameManager.is_PAUSE)
                return;

            keyboard.GetState();
           
            updatePosition();
            if (is_move && Vector2.Distance(sprite.position, targetPosition) > 5f)
                return;

            //move left
            if (keyboard.HasBeenPressed(Keys.A) || keyboard.HasBeenPressed(Keys.Left))
                moveLeft();
            //move right
            else if (keyboard.HasBeenPressed(Keys.D) || keyboard.HasBeenPressed(Keys.Right))
                moveRight();
        }

        private void hitParticle(gridPosition pos)
        {
            Game1.scene.Add(new particle(kaninKitRail.convertGridPosToVectorPos(pos), 1.5f, "impactDust", 5, 1, 19), 5);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}