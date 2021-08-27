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
    public class particle : I_gameInterface
    {
        public int DrawOrder { get; set; }

        private AnimatedTexture m_animator;
        private Vector2 position;

        public particle(Vector2 origin, float scale, string asset, int frameCount, int frameRow, int framesPerSec)
        {
            m_animator = new AnimatedTexture(Vector2.Zero, 0, scale, 1);
            m_animator.Load(gameManager.content, asset, frameCount, frameRow, framesPerSec);
            if (scale != 1)
                position = origin - new Vector2((m_animator.myTexture.Width / frameCount) * scale - gameManager.GRID_WIDTH, (m_animator.myTexture.Height / frameRow) * scale - gameManager.GRID_HEIGHT) / 2;
            else
                position = origin;
        }

        public void Update(float animator_elapsed)
        {
            m_animator.UpdateFrame(animator_elapsed);

            if (m_animator.IsEnd)
            {
                Game1.scene.Remove(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_animator.DrawFrame(spriteBatch, position);
        }
    }
}
