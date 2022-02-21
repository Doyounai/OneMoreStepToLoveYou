using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OneMoreStepToLoveYou.GameInterface;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace OneMoreStepToLoveYou.Entites
{
    public class audioControlPanel : I_gameInterface
    {
        public int DrawOrder { get; set; }

        Sprite musicIcon;
        Sprite soundEffectIcon;
        Sprite musicCheck;
        Sprite soundCheck;

        button music;
        button soundEffect;

        public static bool is_musicMute = false;
        public static bool is_soundEffectMute = false;

        float maxVolumn = 0.08f;

        public audioControlPanel(GraphicsDeviceManager graphic)
        {
            Vector2 startPosition = new Vector2(1650, 20);
            Vector2 offmargin = new Vector2(60, 0);
            musicIcon = new Sprite(Game1.resource.song, startPosition, Color.White);
            music = new button(graphic, Game1.resource.dummyFont, 50, 50, 5, startPosition + offmargin * 1, "", Color.White, Color.White, Color.Gray, Color.DarkGray);
            music.Click += musicClick;
            soundEffectIcon = new Sprite(Game1.resource.soundEffect, startPosition + (offmargin * 2), Color.White);
            soundEffect = new button(graphic, Game1.resource.dummyFont, 50, 50, 5, startPosition + offmargin * 3, "", Color.White, Color.White, Color.Gray, Color.DarkGray);
            soundEffect.Click += soundEffectClick;

            offmargin.Y = -10;
            musicCheck = new Sprite(Game1.resource.check, startPosition + offmargin * 1, Color.White);
            offmargin.Y = -3.3f;
            soundCheck = new Sprite(Game1.resource.check, startPosition + offmargin * 3, Color.White);

            MediaPlayer.Volume = maxVolumn;
        }

        public void Update(float animator_elapsed)
        {
            music.Update();
            soundEffect.Update();
        }

        public void musicClick(object sender, System.EventArgs e)
        {
            is_musicMute = !is_musicMute;
            if (is_musicMute)
                MediaPlayer.Volume = 0;
            else
                MediaPlayer.Volume = maxVolumn;
        }

        public void soundEffectClick(object sender, System.EventArgs e)
        {
            is_soundEffectMute = !is_soundEffectMute;
            if (is_soundEffectMute)
                SoundEffect.MasterVolume = 0;
            else
                SoundEffect.MasterVolume = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            musicIcon.Draw(spriteBatch, 0.1f);
            music.Draw(spriteBatch);
            soundEffectIcon.Draw(spriteBatch, 0.1f);
            soundEffect.Draw(spriteBatch);

            if (!is_musicMute)
                musicCheck.Draw(spriteBatch);
            if(!is_soundEffectMute)
                soundCheck.Draw(spriteBatch);
        }
    }
}
