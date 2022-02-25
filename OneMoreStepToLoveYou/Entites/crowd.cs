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
    public enum crowdType
    {
        normal,
        slow,
        fast
    }

    public class crowd : character, I_gameInterface
    {
        public int DrawOrder { get; set; }
        public int m_moveStep;

        private int crowdMoveStep = 3;
        public List<gridPosition> originPath = new List<gridPosition>();

        public crowd(Texture2D texture, gridPosition gridPos)
        {
            this.type = gridType.Crowd;
            sprite = new Sprite(texture, Vector2.Zero, Color.White);
            m_gridPosition = gridPos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Crowd;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            gameManager.crowds.Add(m_gridPosition, this);

            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animator.Load(Game1.resource.crowd1, 4, 4, 4);
            isCorwd = true;
            currentAnimation = getRandomAnimation();
        }

        public crowd(Texture2D texture, gridPosition gridPos, crowdType type)
        {
            Color color = Color.White;

            if(type == crowdType.fast)
            {
                color = Color.Red;
                this.crowdMoveStep = 1;
                animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
                animator.Load(Game1.resource.crowd3, 4, 4, 4);
                isCorwd = true;
                currentAnimation = getRandomAnimation();
            }
            else if(type == crowdType.slow)
            {
                color = Color.Green;
                this.crowdMoveStep = 5;
                animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
                animator.Load(Game1.resource.crowd2, 4, 4, 4);
                isCorwd = true;
                currentAnimation = getRandomAnimation();
            }

            this.type = gridType.Crowd;
            sprite = new Sprite(texture, Vector2.Zero, color);
            m_gridPosition = gridPos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Crowd;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            gameManager.crowds.Add(m_gridPosition, this);
        }
        public void Update(float animator_elapsed)
        {
            if (gameManager.is_PAUSE)
                return;

            animator.UpdateFrame(animator_elapsed);
            updatePosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sprite.Draw(spriteBatch);
            animator.DrawFrame(spriteBatch, kaninKitRail.convertGridPosToVectorPos(m_gridPosition) + new Vector2(0, gameManager.characterHeight), currentAnimation);
        }

        public void goBack()
        {
            if (gameManager.playerStep - m_moveStep >= crowdMoveStep && originPath.Count > 0 && 
                getNextGridType(originPath[originPath.Count - 1]) != gridType.Unwalkable &&
                getNextGridType(originPath[originPath.Count - 1]) != gridType.Crowd
                )
            {
                //move up
                if (m_gridPosition.row > originPath[originPath.Count - 1].row)
                    currentAnimation = 4;
                //down
                else if (m_gridPosition.row < originPath[originPath.Count - 1].row)
                    currentAnimation = 1;
                //left
                else if (m_gridPosition.column > originPath[originPath.Count - 1].column)
                    currentAnimation = 2;
                //right
                else if (m_gridPosition.column < originPath[originPath.Count - 1].column)
                    currentAnimation = 3;

                changePosition(originPath[originPath.Count - 1]);
                originPath.RemoveAt(originPath.Count - 1);
            }
        }

        public override void changePosition(gridPosition pos)
        {
            gameManager.crowds.Remove(m_gridPosition);
            base.changePosition(pos);
            gameManager.crowds.Add(pos, this);
        }

        public gridType getNextGridType(gridPosition nextPos)
        {
            if (nextPos.column < 0 || nextPos.column > gameManager.GRID_COLUMN - 1 || nextPos.row < 0 || nextPos.row > gameManager.GRID_ROW - 1)
                return gridType.Unwalkable;

            if (gameManager.GRID_DATA[nextPos.row, nextPos.column].type == gridType.Player)
                return gridType.Unwalkable;

            if (gameManager.GRID_DATA[nextPos.row, nextPos.column].type == gridType.Crowd)
                return gridType.Crowd;

            return gameManager.GRID_DATA[nextPos.row, nextPos.column].type;
        }
    }
}
