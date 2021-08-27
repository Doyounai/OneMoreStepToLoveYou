using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.Entites
{
    public class player : character, I_gameInterface
    {
        public int DrawOrder { get; set; }
        private bool is_yaDovNow = false;
        private bool is_ssr = false;

        public player(Texture2D texture, gridPosition gridPos)
        {
            this.type = gridType.Player;
            sprite = new Sprite(texture, gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition, Color.White);
            m_gridPosition = gridPos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Player;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            gameManager.M_PLAYER = this;
        }

        public void Update(float animator_elapsed)
        {
            if (gameManager.is_PAUSE)
                return;

            keyboard.GetState();
            #region player move            
            updatePosition();
            if (is_move && Vector2.Distance(sprite.position, targetPosition) > 5f)
                return;
                
            //move left
            if (keyboard.HasBeenPressed(Keys.A) || keyboard.HasBeenPressed(Keys.Left))
            {
                keyLeft();
            }
            //move right
            else if (keyboard.HasBeenPressed(Keys.D) || keyboard.HasBeenPressed(Keys.Right))
            {
                keyRight();
            }
            //move down
            else if (keyboard.HasBeenPressed(Keys.S) || keyboard.HasBeenPressed(Keys.Down))
            {
                keyDown();
            }
            //move up
            else if (keyboard.HasBeenPressed(Keys.W) || keyboard.HasBeenPressed(Keys.Up))
            {
                keyUp();
            }
            #endregion
            checkCollision();

        }

        private void keyUp()
        {
            if (!gameManager.crowds.ContainsKey(m_gridPosition.up))
                moveUp();
            else if (
                gameManager.crowds.ContainsKey(m_gridPosition.up) &&
                gameManager.crowds[m_gridPosition.up].getNextGridType(gameManager.crowds[m_gridPosition.up].m_gridPosition.up) == gridType.Walkable
                )
            {
                //particle
                //Game1.scene.Add(new particle(gameManager.crowds[m_gridPosition.up].sprite.position, 1, "impactDust", 5, 1, 18), 5);
                Game1.scene.Add(new particle(kaninKitRail.convertGridPosToVectorPos(m_gridPosition.up), 1.5f, "impactDust", 5, 1, 13), 5);
                //crowd move
                gameManager.crowds[m_gridPosition.up].originPath.Add(m_gridPosition.up);
                gameManager.crowds[m_gridPosition.up].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.up].moveUp();
                moveUp();
                gameManager.crowds[m_gridPosition.up].m_moveStep = gameManager.playerStep;
            }
            //ya dov
            else if (
                is_yaDovNow &&
                gameManager.crowds.ContainsKey(m_gridPosition.up) &&
                gameManager.crowds[m_gridPosition.up].getNextGridType(gameManager.crowds[m_gridPosition.up].m_gridPosition.up) == gridType.Crowd &&
                gameManager.crowds[m_gridPosition.up.up].getNextGridType(m_gridPosition.up.up.up) == gridType.Walkable
                )
            {
                //crowd2
                gameManager.crowds[m_gridPosition.up.up].originPath.Add(m_gridPosition.up.up);
                gameManager.crowds[m_gridPosition.up.up].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.up.up].moveUp();
                //crowd1
                gameManager.crowds[m_gridPosition.up].originPath.Add(m_gridPosition.up);
                gameManager.crowds[m_gridPosition.up].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.up].moveUp();
                moveUp();
                gameManager.crowds[m_gridPosition.up].m_moveStep = gameManager.playerStep;
            }
        }

        private void keyDown()
        {
            if (!gameManager.crowds.ContainsKey(m_gridPosition.down))
                moveDown();
            else if (
                gameManager.crowds.ContainsKey(m_gridPosition.down) &&
                gameManager.crowds[m_gridPosition.down].getNextGridType(gameManager.crowds[m_gridPosition.down].m_gridPosition.down) == gridType.Walkable
                )
            {
                //crowd move
                gameManager.crowds[m_gridPosition.down].originPath.Add(m_gridPosition.down);
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down].moveDown();
                moveDown();
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
            }
            //ya dov
            else if (
                is_yaDovNow &&
                gameManager.crowds.ContainsKey(m_gridPosition.down) &&
                gameManager.crowds[m_gridPosition.down].getNextGridType(gameManager.crowds[m_gridPosition.down].m_gridPosition.down) == gridType.Crowd &&
                gameManager.crowds[m_gridPosition.down.down].getNextGridType(m_gridPosition.down.down.down) == gridType.Walkable
                )
            {
                //crowd2
                gameManager.crowds[m_gridPosition.down.down].originPath.Add(m_gridPosition.down.down);
                gameManager.crowds[m_gridPosition.down.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down.down].moveDown();
                //crowd1
                gameManager.crowds[m_gridPosition.down].originPath.Add(m_gridPosition.down);
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down].moveDown();
                moveDown();
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
            }
        }

        private void keyLeft()
        {
            if (!gameManager.crowds.ContainsKey(m_gridPosition.left))
                moveLeft();
            else if (
                gameManager.crowds.ContainsKey(m_gridPosition.left) &&
                gameManager.crowds[m_gridPosition.left].getNextGridType(gameManager.crowds[m_gridPosition.left].m_gridPosition.left) == gridType.Walkable
                )
            {
                //crowd move
                gameManager.crowds[m_gridPosition.left].originPath.Add(m_gridPosition.left);
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left].moveLeft();
                moveLeft();
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
            }
            //ya dov
            else if (
                is_yaDovNow &&
                gameManager.crowds.ContainsKey(m_gridPosition.left) &&
                gameManager.crowds[m_gridPosition.left].getNextGridType(gameManager.crowds[m_gridPosition.left].m_gridPosition.left) == gridType.Crowd &&
                gameManager.crowds[m_gridPosition.left.left].getNextGridType(m_gridPosition.left.left.left) == gridType.Walkable
                )
            {
                //crowd2
                gameManager.crowds[m_gridPosition.left.left].originPath.Add(m_gridPosition.left.left);
                gameManager.crowds[m_gridPosition.left.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left.left].moveLeft();
                //crowd1
                gameManager.crowds[m_gridPosition.left].originPath.Add(m_gridPosition.left);
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left].moveLeft();
                moveLeft();
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
            }
        }

        private void keyRight()
        {
            if (!gameManager.crowds.ContainsKey(m_gridPosition.right))
                moveRight();
            else if (
                gameManager.crowds.ContainsKey(m_gridPosition.right) &&
                gameManager.crowds[m_gridPosition.right].getNextGridType(gameManager.crowds[m_gridPosition.right].m_gridPosition.right) == gridType.Walkable
                )
            {
                //crowd move
                gameManager.crowds[m_gridPosition.right].originPath.Add(m_gridPosition.right);
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right].moveRight();
                moveRight();
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
            }
            //ya dov
            else if (
                is_yaDovNow &&
                gameManager.crowds.ContainsKey(m_gridPosition.right) &&
                gameManager.crowds[m_gridPosition.right].getNextGridType(gameManager.crowds[m_gridPosition.right].m_gridPosition.right) == gridType.Crowd &&
                gameManager.crowds[m_gridPosition.right.right].getNextGridType(m_gridPosition.right.right.right) == gridType.Walkable
                )
            {
                //crowd2
                gameManager.crowds[m_gridPosition.right.right].originPath.Add(m_gridPosition.right.right);
                gameManager.crowds[m_gridPosition.right.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right.right].moveRight();
                //crowd1
                gameManager.crowds[m_gridPosition.right].originPath.Add(m_gridPosition.right);
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right].moveRight();
                moveRight();
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
            }
        }

        public void checkCollision()
        {
            //yaDov
            if (gameManager.ya != null && this.sprite.rec.Intersects(gameManager.ya.sprite.rec))
            {
                Game1.scene.Remove(gameManager.ya);
                gameManager.ya = null;
                is_yaDovNow = true;
            }
            //ssr
            if(gameManager.ssr != null && this.sprite.rec.Intersects(gameManager.ssr.sprite.rec))
            {
                Game1.scene.Remove(gameManager.ssr);
                gameManager.ssr = null;
                is_ssr = true;
            }
        }

        public override void changePosition(gridPosition pos)
        {
            if(pos.row == gameManager.pEarthPosition.row && pos.column == gameManager.pEarthPosition.column)
            {
                if (!is_ssr)
                    return;
                //transition
                Game1.dialouge.dialogeOn();
                //Game1.changeSceneTo(2);
                return;
            }

            base.changePosition(pos);
            gameManager.playerMove();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
