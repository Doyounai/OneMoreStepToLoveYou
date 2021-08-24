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

        public yaDov(gridPosition pos, Texture2D image)
        {
            Vector2 position = gameManager.GRID_DATA[pos.row, pos.column].getCenterGridPosition;
            position -= kaninKitRail.getCenterPoint(image.Width, image.Height);
            this.sprite = new Sprite(image, position, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public void Update(float animator_elapsed)
        {

        }
    }
}
