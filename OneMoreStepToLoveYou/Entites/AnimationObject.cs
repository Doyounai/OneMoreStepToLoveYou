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
    public class AnimationObject : I_gameInterface
    {
        public int DrawOrder { get; set; }

        private AnimatedTexture animator;

        private bool is_move = false;
        private float speed;

        private Vector2 startPosition;
        private Vector2 m_position;
        private Vector2 m_destination;
        private Vector2 diration;

        private int animationRow = 0;

        public AnimationObject(Vector2 m_position, Texture2D sprite, int frameCount, int frameRow)
        {
            this.is_move = false;
            this.m_position = m_position;
            this.animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            this.animator.Load(sprite, frameCount, frameRow, 15);
        }

        public AnimationObject(Vector2 m_position, Texture2D sprite, int frameCount, int frameRow, Vector2 destination, float speed)
        {
            this.is_move = true;
            this.startPosition = m_position;
            this.m_position = startPosition;
            this.animator = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            this.animator.Load(sprite, frameCount, frameRow, 15);

            //move diration set up
            this.speed = speed;
            this.m_destination = destination;
            this.diration = destination - m_position;
            this.diration.Normalize();
        }

        public void lockAnimationRow(int row)
        {
            this.animationRow = row;
        }

        public void setSize(float size)
        {
            this.animator.Scale = size;
        }

        public void Update(float animator_elapsed)
        {
            animator.UpdateFrame(animator_elapsed);
            
            //move
            if (is_move)
            {
                if(Vector2.Distance(m_position, m_destination) <= 4)
                {
                    m_position = startPosition;
                    return;
                }

                m_position += diration * speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (animationRow == 0)
                animator.DrawFrame(spriteBatch, m_position);
            else
                animator.DrawFrame(spriteBatch, m_position, animationRow);
        }
    }
}
