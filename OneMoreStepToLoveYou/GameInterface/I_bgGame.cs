using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_bgGame : I_gameInterface
    {
        public int DrawOrder { get; set; }
        public Sprite sprite;

        public I_bgGame(Texture2D bg)
        {
            if (1080 > gameManager.GRID_HEIGHT * gameManager.GRID_ROW)
                sprite = new Sprite(bg, Vector2.Zero, Color.White);
            else
                sprite = new Sprite(bg, new Vector2(0, -960), Color.White);
        }

        public void Update(float animator_elapsed)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
