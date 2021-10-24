using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;
using Microsoft.Xna.Framework.Media;

namespace OneMoreStepToLoveYou.Entites
{
    public class player : character, I_gameInterface
    {
        public int DrawOrder { get; set; }
        private bool is_yaDovNow = false;
        private bool is_ssr = false;

        private int cutScene = 0;
        private float walkVolumn = 0.1f;
        private float impactVolumn = 0.1f;

        public player(Texture2D texture, gridPosition gridPos)
        {
            this.type = gridType.Player;
            sprite = new Sprite(texture, gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition, Color.White);
            m_gridPosition = gridPos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Player;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            gameManager.M_PLAYER = this;
            //MediaPlayer.Volume = 0;
            MediaPlayer.Play(Game1.gameplaySong);

            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animator.Load(Game1.resource.playerSprite, 4, 6, 12);
        }
        public player(Texture2D texture, gridPosition gridPos, int cutScneToGo)
        {
            this.type = gridType.Player;
            sprite = new Sprite(texture, gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition, Color.White);
            m_gridPosition = gridPos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Player;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            gameManager.M_PLAYER = this;
            is_ssr = true;
            this.cutScene = cutScneToGo;

            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animator.Load(Game1.resource.playerSprite, 4, 6, 12);
        }

        public void Update(float animator_elapsed)
        {
            if (gameManager.is_PAUSE)
                return;

            animator.UpdateFrame(animator_elapsed);

            keyboard.GetState();
            #region player move            
            updatePosition();
            if (is_move && Vector2.Distance(sprite.position, targetPosition) > 5f)
                return;

            //cheat
            /*if(keyboard.HasBeenPressed(Keys.Space))
            {
                gameManager.updateStart();
                Game1.dialouge.dialogeOn();
            }*/
            if (keyboard.HasBeenPressed(Keys.Escape))
                Game1.pausePanel.Activate();
            
            //re
            if(keyboard.HasBeenPressed(Keys.R))
            {
                Game1.changeSceneTo(Game1.currentLevel);
            }

            //move left
            if (keyboard.HasBeenPressed(Keys.A) || keyboard.HasBeenPressed(Keys.Left))
            {
                currentAnimation = 2;
                keyLeft();
                Game1.playSound(Game1.walkSound, walkVolumn);
            }
            //move right
            else if (keyboard.HasBeenPressed(Keys.D) || keyboard.HasBeenPressed(Keys.Right))
            {
                currentAnimation = 5;
                keyRight();
                
                Game1.playSound(Game1.walkSound, walkVolumn);
            }
            //move down
            else if (keyboard.HasBeenPressed(Keys.S) || keyboard.HasBeenPressed(Keys.Down))
            {
                keyDown();
                Game1.playSound(Game1.walkSound, walkVolumn);
            }
            //move up
            else if (keyboard.HasBeenPressed(Keys.W) || keyboard.HasBeenPressed(Keys.Up))
            {
                currentAnimation = 4;
                keyUp();
                Game1.playSound(Game1.walkSound, walkVolumn);
            }
            #endregion
            checkCollision();

        }

        private void hitParticle(gridPosition pos)
        {
            Game1.scene.Add(new particle(kaninKitRail.convertGridPosToVectorPos(pos), 1.5f, "impactDust", 5, 1, 19), 5);
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
                //Game1.scene.Add(new particle(kaninKitRail.convertGridPosToVectorPos(m_gridPosition.up), 1.5f, "impactDust", 5, 1, 13), 5);
                hitParticle(m_gridPosition.up);
                Game1.playSound(Game1.impactSound, impactVolumn);
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
                //particle
                hitParticle(m_gridPosition.up);
                Game1.playSound(Game1.impactSound, impactVolumn);
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
                //particle
                hitParticle(m_gridPosition.down);
                Game1.playSound(Game1.impactSound, impactVolumn);
                //crowd move
                gameManager.crowds[m_gridPosition.down].originPath.Add(m_gridPosition.down);
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down].moveDown();
                moveDown();
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down].currentAnimation = 4;
            }
            //ya dov
            else if (
                is_yaDovNow &&
                gameManager.crowds.ContainsKey(m_gridPosition.down) &&
                gameManager.crowds[m_gridPosition.down].getNextGridType(gameManager.crowds[m_gridPosition.down].m_gridPosition.down) == gridType.Crowd &&
                gameManager.crowds[m_gridPosition.down.down].getNextGridType(m_gridPosition.down.down.down) == gridType.Walkable
                )
            {
                //particle
                hitParticle(m_gridPosition.down);
                Game1.playSound(Game1.impactSound, impactVolumn);
                //crowd2
                gameManager.crowds[m_gridPosition.down.down].originPath.Add(m_gridPosition.down.down);
                gameManager.crowds[m_gridPosition.down.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down.down].currentAnimation = 4;
                gameManager.crowds[m_gridPosition.down.down].moveDown();
                //crowd1
                gameManager.crowds[m_gridPosition.down].originPath.Add(m_gridPosition.down);
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down].moveDown();
                moveDown();
                gameManager.crowds[m_gridPosition.down].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.down].currentAnimation = 4;
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
                //particle
                currentAnimation = 3;
                hitParticle(m_gridPosition.left);
                Game1.playSound(Game1.impactSound, impactVolumn);
                //crowd move
                gameManager.crowds[m_gridPosition.left].originPath.Add(m_gridPosition.left);
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left].currentAnimation = 3;
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
                //particle
                currentAnimation = 3;
                hitParticle(m_gridPosition.left);
                Game1.playSound(Game1.impactSound, impactVolumn);
                //crowd2
                gameManager.crowds[m_gridPosition.left.left].originPath.Add(m_gridPosition.left.left);
                gameManager.crowds[m_gridPosition.left.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left.left].currentAnimation = 3;
                gameManager.crowds[m_gridPosition.left.left].moveLeft();
                //crowd1
                gameManager.crowds[m_gridPosition.left].originPath.Add(m_gridPosition.left);
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left].moveLeft();
                moveLeft();
                gameManager.crowds[m_gridPosition.left].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.left].currentAnimation = 3;
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
                //particle
                currentAnimation = 6;
                hitParticle(m_gridPosition.right);
                Game1.playSound(Game1.impactSound, impactVolumn);
                //crowd move
                gameManager.crowds[m_gridPosition.right].originPath.Add(m_gridPosition.right);
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right].moveRight();
                moveRight();
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right].currentAnimation = 2;
            }
            //ya dov
            else if (
                is_yaDovNow &&
                gameManager.crowds.ContainsKey(m_gridPosition.right) &&
                gameManager.crowds[m_gridPosition.right].getNextGridType(gameManager.crowds[m_gridPosition.right].m_gridPosition.right) == gridType.Crowd &&
                gameManager.crowds[m_gridPosition.right.right].getNextGridType(m_gridPosition.right.right.right) == gridType.Walkable
                )
            {
                //particle.
                currentAnimation = 6;
                hitParticle(m_gridPosition.right);
                Game1.playSound(Game1.impactSound, impactVolumn);
                //crowd2
                gameManager.crowds[m_gridPosition.right.right].originPath.Add(m_gridPosition.right.right);
                gameManager.crowds[m_gridPosition.right.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right.right].currentAnimation = 3;
                gameManager.crowds[m_gridPosition.right.right].moveRight();
                //crowd1
                gameManager.crowds[m_gridPosition.right].originPath.Add(m_gridPosition.right);
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right].moveRight();
                moveRight();
                gameManager.crowds[m_gridPosition.right].m_moveStep = gameManager.playerStep;
                gameManager.crowds[m_gridPosition.right].currentAnimation = 3;
            }
        }

        public void checkCollision()
        {
            //yaDov
            if (gameManager.ya != null && this.sprite.rec.Intersects(gameManager.ya.sprite.rec))
            {
                Game1.playSound(Game1.endScene);
                Game1.scene.Remove(gameManager.ya);
                gameManager.ya = null;
                is_yaDovNow = true;
            }
            //ssr
            if(gameManager.ssr != null && this.sprite.rec.Intersects(gameManager.ssr.sprite.rec))
            {
                Game1.playSound(Game1.SSR);
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
                if(cutScene != 0)
                {
                    Game1.changeSceneTo(cutScene);
                    return;
                }
                //transition           
                Game1.playSound(Game1.endScene);
                gameManager.updateStart();
                Game1.dialouge.dialogeOn();
                //Game1.changeSceneTo(2);

                return;
            }

            base.changePosition(pos);
            gameManager.playerMove();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sprite.Draw(spriteBatch);
            animator.DrawFrame(spriteBatch, kaninKitRail.convertGridPosToVectorPos(m_gridPosition), currentAnimation);
        }
    }
}
