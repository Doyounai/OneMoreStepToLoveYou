using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMoreStepToLoveYou.Entites
{
    public class playerCamera
    {
        public Matrix Transform { get; set; }

        public void Follow(Sprite target)
        {
            var position = new Matrix();
            int sceneOverHeight = 1080 - ((gameManager.GRID_HEIGHT * gameManager.GRID_ROW) % 1080) - 12;

            int gridLimit = (1080 / 2) / gameManager.GRID_WIDTH;

            //move
            if (gameManager.M_PLAYER.m_gridPosition.row > gridLimit && gameManager.M_PLAYER.m_gridPosition.row < gameManager.GRID_ROW - gridLimit &&
                target.position.Y <= ((gameManager.GRID_STARTPOSITION.Y + gameManager.GRID_HEIGHT * gameManager.GRID_ROW) - gameManager.GRID_HEIGHT * gridLimit) - sceneOverHeight
                )
            {
                position = Matrix.CreateTranslation(
                  (1920 / 2) * -1,
                  -target.position.Y - (target.gameSprite.Height / 2),
                  0);
            }
            else//dont move
            {
                if(gameManager.M_PLAYER.m_gridPosition.row <= gridLimit)
                {
                    if(target.position.Y > gameManager.GRID_STARTPOSITION.Y + gameManager.GRID_HEIGHT * gridLimit)
                        position = Matrix.CreateTranslation(
                          (1920 / 2) * -1,
                          -target.position.Y - (target.gameSprite.Height / 2),
                          0);
                    else
                        position = Matrix.CreateTranslation(
                          (1920 / 2) * -1,
                          -(gameManager.GRID_STARTPOSITION.Y + gameManager.GRID_HEIGHT * gridLimit) - (target.gameSprite.Height / 2) - 15,
                          0);
                }
                else if (gameManager.M_PLAYER.m_gridPosition.row >= gameManager.GRID_ROW - gridLimit)
                {
                    if (target.position.Y < ((gameManager.GRID_STARTPOSITION.Y + gameManager.GRID_HEIGHT * gameManager.GRID_ROW) - gameManager.GRID_HEIGHT * gridLimit) - sceneOverHeight)
                        position = Matrix.CreateTranslation(
                          (1920 / 2) * -1,
                          -target.position.Y - (target.gameSprite.Height / 2),
                          0);
                    else
                        position = Matrix.CreateTranslation(
                          (1920 / 2) * -1,
                          -((gameManager.GRID_STARTPOSITION.Y + gameManager.GRID_HEIGHT * gameManager.GRID_ROW) - gameManager.GRID_HEIGHT * gridLimit) + sceneOverHeight - (target.gameSprite.Height / 2),
                          0);
                }
                else
                {
                    position = Matrix.CreateTranslation(
                          (1920 / 2) * -1,
                          -((gameManager.GRID_STARTPOSITION.Y + gameManager.GRID_HEIGHT * gameManager.GRID_ROW) - gameManager.GRID_HEIGHT * gridLimit) + sceneOverHeight - (target.gameSprite.Height / 2),
                          0);
                }
            }
            var offset = Matrix.CreateTranslation(
                1920 / 3,
                1080 / 2,
                0);

            Transform = position * offset;
        }
    }
}
