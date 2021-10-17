using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMoreStepToLoveYou.Entites
{
    public class character
    {
        public Sprite sprite;
        public gridPosition m_gridPosition;
        public Vector2 targetPosition;
        public int moveSpeed = 8;
        public bool is_move = false;
        public gridType type;


        public AnimatedTexture animator;
        public int currentAnimation = 1;
        public bool isCorwd = false;

        public void moveLeft()
        {
            if (m_gridPosition.column <= 0 || gameManager.GRID_DATA[m_gridPosition.left.row, m_gridPosition.left.column].type == gridType.Unwalkable)
                return;

            changePosition(m_gridPosition.left);
        }

        public void moveRight()
        {
            if (m_gridPosition.column >= gameManager.GRID_COLUMN - 1 || gameManager.GRID_DATA[m_gridPosition.right.row, m_gridPosition.right.column].type == gridType.Unwalkable)
                return;

            changePosition(m_gridPosition.right);
        }

        public void moveUp()
        {
            if (m_gridPosition.row <= 0 || gameManager.GRID_DATA[m_gridPosition.up.row, m_gridPosition.up.column].type == gridType.Unwalkable)
                return;

            changePosition(m_gridPosition.up);
        }

        public void moveDown()
        {
            if (m_gridPosition.row >= gameManager.GRID_ROW - 1 || gameManager.GRID_DATA[m_gridPosition.down.row, m_gridPosition.down.column].type == gridType.Unwalkable)
                return;

            changePosition(m_gridPosition.down);
        }

        public virtual void changePosition(gridPosition pos)
        {
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Walkable;
            m_gridPosition = pos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = this.type;
            is_move = true;
            targetPosition = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            targetPosition -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);
        }

        public void updatePosition()
        {
            //sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            //sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            if (is_move)
            {
                if(Vector2.Distance(sprite.position, targetPosition) <= 5f)
                {
                    is_move = false;
                    sprite.position = targetPosition;
                    if (!isCorwd)
                        currentAnimation = 1;
                    return;
                }
                Vector2 diration = targetPosition - sprite.position;
                diration.Normalize();
                diration *= moveSpeed;
                sprite.position += diration;
            }
        }

        public int getRandomAnimation()
        {
            return Game1.rand.Next(1, 5);
        }
    }
}
