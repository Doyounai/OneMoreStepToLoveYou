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

        //Random
        public Random rand = new Random();

        //background
        private List<Texture2D> backgroundTextures = new List<Texture2D>();
        private List<Sprite> backgrounSprites = new List<Sprite>();

        public racingManager(Texture2D texture)
        {
            this.texture = texture;
            spawnDistance = texture.Height + (spawnOffset * currentSpeed);
        }

        public void Update(float animator_elapsed)
        {
            //increase speed;
            totalTime += animator_elapsed;
            if(totalTime >=  disChangeSpeed)
            {
                currentSpeed += speedIncreaseRate;
                totalTime = 0;
                spawnDistance = texture.Height + (spawnOffset * currentSpeed);
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
            foreach (Sprite item in backgrounSprites)
            {
                item.position.Y += currentSpeed;
            }
        }

        private void spawnCrowd()
        {
            Game1.scene.Add(new crowdRacing(texture, rand.Next(0, 3), currentSpeed), 3);
        }

        public void loadBackgroundTexture(Texture2D texture)
        {
            backgroundTextures.Add(texture);
        }

        public void spawnNewBackground()
        {
            if(backgrounSprites.Count > 0)
            {
                backgrounSprites.Add(new Sprite(backgroundTextures[rand.Next(0, backgroundTextures.Count)], Vector2.Zero, Color.White));
                return;
            }

            backgrounSprites.Add(new Sprite(backgroundTextures[rand.Next(0, backgroundTextures.Count)], new Vector2(0, backgrounSprites[backgrounSprites.Count - 1].position.Y - 1080), Color.White));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite item in backgrounSprites)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}
