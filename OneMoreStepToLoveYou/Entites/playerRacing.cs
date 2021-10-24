using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMoreStepToLoveYou.GameInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace OneMoreStepToLoveYou.Entites
{

    public class playerRacing : character, I_gameInterface
    {
        public int DrawOrder { get; set; }
        AnimatedTexture animator;


        public playerRacing(Texture2D texture, gridPosition gridPos, Texture2D animationSprite)
        {
            this.moveSpeed = 15;
            this.type = gridType.Player;
            sprite = new Sprite(texture, gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition, Color.White);
            m_gridPosition = gridPos;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);
            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animator.Load(animationSprite, 4, 4, 15);

            gameManager.racingPlayer = this;

            if(!audioControlPanel.is_musicMute)
                MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(Game1.racingSong);
        }

        public void Update(float animator_elapsed)
        {
            if (gameManager.is_PAUSE)
                return;

            keyboard.GetState();
            animator.UpdateFrame(animator_elapsed);

            updatePosition();
            if (is_move && Vector2.Distance(sprite.position, targetPosition) > 5f)
                return;

            if (keyboard.HasBeenPressed(Keys.Escape))
                Game1.pausePanel.Activate();


            //move left
            if (keyboard.HasBeenPressed(Keys.A) || keyboard.HasBeenPressed(Keys.Left))
                moveLeft();
            //move right
            else if (keyboard.HasBeenPressed(Keys.D) || keyboard.HasBeenPressed(Keys.Right))
                moveRight();
        }

        public void hitParticle(gridPosition pos)
        {
            Game1.scene.Add(new particle(kaninKitRail.convertGridPosToVectorPos(pos), 1.5f, "impactDust", 5, 1, 19), 5);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sprite.Draw(spriteBatch);
            animator.DrawFrame(spriteBatch, sprite.position, 2);
        }
    }
}