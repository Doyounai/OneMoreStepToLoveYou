using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.Entites
{
    public class pEarth : I_gameInterface
    {
        public int DrawOrder { get; set; }
        public AnimatedTexture animator;
        private Vector2 position = Vector2.Zero;

        bool isBeforeLv4 = false;

        public pEarth(gridPosition position, ContentManager content, string asset, int frameCount, int frameRow, int framesPerSec)
        {
            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animator.Load(content, asset, frameCount, frameRow, framesPerSec);
            this.position = gameManager.GRID_DATA[position.row, position.column].getCenterGridPosition;
            this.position -=    kaninKitRail.getCenterPoint((int)animator.frameWidht, (int)animator.frameHeight);
            gameManager.pEarthPosition = position;
        }

        public pEarth(gridPosition position)
        {
            animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            this.position = gameManager.GRID_DATA[position.row, position.column].getCenterGridPosition;
            this.position -= kaninKitRail.getCenterPoint((int)animator.frameWidht, (int)animator.frameHeight);
            gameManager.pEarthPosition = position;

            isBeforeLv4 = true;
        }

        public void Update(float animator_elapsed)
        {
            if (isBeforeLv4)
                return;
            animator.UpdateFrame(animator_elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isBeforeLv4)
                return;
            animator.DrawFrame(spriteBatch, position);
        }
    }
}
