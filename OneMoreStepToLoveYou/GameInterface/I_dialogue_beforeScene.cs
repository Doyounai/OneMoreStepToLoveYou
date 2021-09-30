using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_dialogue_beforeScene : I_gameInterface
    {
        public int DrawOrder { get; set; }
        Texture2D bg;

        public I_dialogue_beforeScene(Texture2D bg)
        {
            this.bg = bg;
            Game1.dialouge.dialogeOn();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, Vector2.Zero, Color.White);
        }

        public void Update(float animator_elapsed)
        {

        }
    }
}
