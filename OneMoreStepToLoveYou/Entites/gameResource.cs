using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace OneMoreStepToLoveYou.Entites
{
    public class gameResource
    {
        //animation sprite
        public Texture2D playerSprite;
        public Texture2D pEarthSprite;
        public Texture2D crowd1;
        public Texture2D crowd2;
        public Texture2D crowd3;
        public Texture2D ota;

        //item
        public Texture2D ssrCard;
        public Texture2D yaDov;

        //ui
        public Texture2D song;
        public Texture2D soundEffect;
        public Texture2D check;

        public SpriteFont dummyFont;

        public gameResource(ContentManager Content)
        {
            playerSprite = Content.Load<Texture2D>("Silver");
            pEarthSprite = Content.Load<Texture2D>("eath");
            crowd1 = Content.Load<Texture2D>("crowd1");
            crowd2 = Content.Load<Texture2D>("crowd2");
            crowd3 = Content.Load<Texture2D>("crowd3");
            ota = Content.Load<Texture2D>("Ota");

            ssrCard = Content.Load<Texture2D>("SSRCard");
            yaDov = Content.Load<Texture2D>("yaDov");
            song = Content.Load<Texture2D>("song");
            soundEffect = Content.Load<Texture2D>("soundEffect");
            check = Content.Load<Texture2D>("check");

            dummyFont = Content.Load<SpriteFont>("debugFont");
        }
    }
}
