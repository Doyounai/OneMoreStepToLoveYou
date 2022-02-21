using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using OneMoreStepToLoveYou.Entites;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class Logo : I_gameInterface
    {
        public int DrawOrder { get; set; }

        Sprite logo;
        float currentAlpha = 0f;
        float transitionSpeed = 0.01f;

        bool is_scaleDown = false;

        public Logo(ContentManager Content)
        {
            Texture2D logoTexture = Content.Load<Texture2D>("Logo");
            Vector2 position = new Vector2(1920 / 2 - logoTexture.Width / 2, 1080 / 2 - logoTexture.Height / 2);
            logo = new Sprite(Content.Load<Texture2D>("Logo"), position, Color.White);
        }

        public void Update(float animator_elapsed)
        {
            if (currentAlpha < 1 && !is_scaleDown)
            {
                currentAlpha += transitionSpeed;
                if (currentAlpha >= 1)
                    is_scaleDown = true;
            }
            else if(is_scaleDown)
            {
                currentAlpha -= transitionSpeed;
            }

            logo.tintColor = new Color(255, 255, 255, currentAlpha);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            logo.Draw(spriteBatch);
        }   
    }
}
