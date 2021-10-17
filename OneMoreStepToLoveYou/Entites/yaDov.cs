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
    public class yaDov : I_gameInterface
    {
        public int DrawOrder { get; set; }
        public Sprite sprite;

        public AnimatedTexture animator;

        public yaDov(gridPosition pos, Texture2D image, Texture2D sprite)
        {
            Vector2 position = gameManager.GRID_DATA[pos.row, pos.column].getCenterGridPosition;
            position -= kaninKitRail.getCenterPoint(image.Width, image.Height);
            this.sprite = new Sprite(image, position, Color.White);

            animator = new AnimatedTexture(Vector2.Zero, 0, 0.7f, 1);
            animator.Load(sprite, 2, 1, 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sprite.Draw(spriteBatch);
            animator.DrawFrame(spriteBatch, sprite.position + new Vector2(10, 5), 1);
        }

        public void Update(float animator_elapsed)
        {
            animator.UpdateFrame(animator_elapsed);
        }
    }
}
