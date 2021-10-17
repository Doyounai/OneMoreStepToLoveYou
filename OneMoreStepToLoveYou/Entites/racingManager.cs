using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.Entites
{
    public class racingManager : I_gameInterface
    {
        public int DrawOrder { get; set; }

        //speed
        private const float speedIncreaseRate = 0.8f;
        private float currentSpeed = 10;
        private const float disChangeSpeed = 2f;
        private float totalTime = 0;

        //crowd spawn
        private Texture2D texture;
        private float currentDistance = 0;
        private float lastSpawn = 0;
        private float spawnDistance;
        private float spawnOffset = 20;

        //back ground
        private int topBackgroundIndex;
        private Sprite[] backgroundSprites = new Sprite[2];
        List<Texture2D> backgroundTextures = new List<Texture2D>();

        //event hit
        private float startSpeed;
        private float breakForce = 2f;
        private float breakTime = 4f;
        private float totalBreakTime = 0;
        public bool isBreak = false;

        float endDestination = 13000f;

        Texture2D animationSprite;


        public static SoundEffectInstance sound;

        public static SoundEffect soundfast;
        public static SoundEffect soundBreak;

        public Texture2D newBackgroundTexture
        {
            set
            {
                backgroundTextures.Add(value);
            }
        }

        public racingManager(Texture2D texture, Texture2D animationSprite, ContentManager content)
        {
            this.texture = texture;
            spawnDistance = texture.Height + (spawnOffset * currentSpeed);
            gameManager.racingGameManager = this;

            startSpeed = currentSpeed;

            this.animationSprite = animationSprite;

            soundfast = content.Load<SoundEffect>("racingFast");
            soundBreak = content.Load<SoundEffect>("racingBreak");

            sound = soundfast.CreateInstance();
            sound.Volume = 0.7f;
            sound.IsLooped = true;
            sound.Play();
        }

        public void Update(float animator_elapsed)
        {
            if(currentDistance >= endDestination)
            {
                //Game1.changeSceneTo(4);
                isBreak = true;
                Game1.dialouge.dialogeOn();
                sound.Stop();
                return;
            }

            //increase speed;
            totalTime += animator_elapsed;
            /*if(totalTime >=  disChangeSpeed)
            {
                currentSpeed += speedIncreaseRate;
                totalTime = 0;
                spawnDistance = texture.Height + (spawnOffset * currentSpeed);
            }*/
            if(isBreak)
            {
                totalBreakTime += animator_elapsed;
                if(totalBreakTime > breakTime)
                {
                    isBreak = false;
                    currentSpeed = startSpeed;

                    sound.Stop();
                    sound = soundfast.CreateInstance();
                    sound.Volume = 0.7f;
                    sound.IsLooped = true;
                    sound.Play();
                }
            }

            //current speed
            currentDistance += currentSpeed;

            //spawn corwd
            if (currentDistance - lastSpawn >= spawnDistance)
            {
                spawnCrowd();
                lastSpawn = currentDistance;
            }

            //background
            for (int i = 0; i < 2; i++)
            {
                backgroundSprites[i].position.Y += currentSpeed;
                if(backgroundSprites[i].position.Y > 1080)
                {
                    backgroundSprites[i].position.Y = backgroundSprites[topBackgroundIndex].position.Y - 1080;
                    backgroundSprites[i].gameSprite = backgroundTextures[Game1.rand.Next(0, backgroundTextures.Count - 1)];
                    topBackgroundIndex = i;
                    backgroundSprites[i].position.Y += currentSpeed;
                }
            }
        }

        private void spawnCrowd()
        {
            Game1.scene.Add(new crowdRacing(texture, Game1.rand.Next(0, 3), (isBreak) ? startSpeed : currentSpeed, animationSprite), 3);
        }

        public void updateBackgrounds()
        {
            topBackgroundIndex = 1;
            backgroundSprites[0] = new Sprite(backgroundTextures[Game1.rand.Next(0, backgroundTextures.Count)], Vector2.Zero, Color.White);
            backgroundSprites[1] = new Sprite(backgroundTextures[Game1.rand.Next(0, backgroundTextures.Count)], new Vector2(0, -1080), Color.White);
        }

        public void เบรครถ()
        {
            currentSpeed -= breakForce;
            isBreak = true;
            totalBreakTime = 0;
            gameManager.racingPlayer.hitParticle(gameManager.racingPlayer.m_gridPosition);

            sound.Stop();
            sound = soundBreak.CreateInstance();
            sound.Volume = 1f;
            sound.IsLooped = true;
            sound.Play();

            Game1.playSound(Game1.impactSound);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 2; i++)
            {
                backgroundSprites[i].Draw(spriteBatch);
            }
        }
    }
}
