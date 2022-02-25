using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OneMoreStepToLoveYou.GameInterface;
using OneMoreStepToLoveYou.Entites;
using System;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace OneMoreStepToLoveYou
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Random rand = new Random();
        public static Game1 self;

        //scene
        public static I_sceneManager scene = new I_sceneManager();
        static int sceneToGo;

        //transitional
        public static int currentLevel = 0;
        public static float transitionSpeed = 0.03f;
        float transitionAlpha = 1f;
        Sprite transitionPanel;
        static bool is_fadeOut = true;
        static bool is_fadeIn = false;

        //dialogue
        public static I_dialouge dialouge;  

        //camera
        playerCamera camera;
        public static bool is_CameraOn = false;

        //sound
        public static SoundEffect walkSound;
        public static SoundEffect impactSound;
        public static SoundEffect endScene;
        public static SoundEffect powerUP;
        public static SoundEffect SSR;
        public static SoundEffect Click;
        public static SoundEffect star;

        //song
        public static Song racingSong;
        public static Song titleSong;
        public static Song gameplaySong;

        public static gameResource resource;

        public static I_pausePanel pausePanel;

        public void exit()
        {
            this.Exit();
        }

        public static void playSound(SoundEffect sound)
        {
            var instance = sound.CreateInstance();
            instance.Play();
        }

        public static void playSound(SoundEffect sound, float volumn)
        {
            var instance = sound.CreateInstance();
            instance.Volume = volumn;
            instance.Play();
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            self = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = false;
            this.IsMouseVisible = true;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameManager.content = Content;

            //camera
            camera = new playerCamera();

            //transition
            transitionPanel = new Sprite(kaninKitRail.getBoxTexture(graphics, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, Color.White, 0), Vector2.Zero, Color.White);

            //dialouge
            dialouge = new I_dialouge(graphics, Content.Load<SpriteFont>("dialogueName_Font"), Content.Load<SpriteFont>("dialogueMessege_font"), Content);
            dialouge.buttonSetup(graphics, Content.Load<SpriteFont>("dialogueNextFont"), 120, 80, 10, new Vector2(1690, 980), "Next", Color.Gray, Color.Black, Color.White, Color.White);

            //sound song
            walkSound = Content.Load<SoundEffect>("playerWalk");
            impactSound = Content.Load<SoundEffect>("playerImpact");
            powerUP = Content.Load<SoundEffect>("powerUP");
            endScene = Content.Load<SoundEffect>("endScene");
            SSR = Content.Load<SoundEffect>("SSR");
            Click = Content.Load<SoundEffect>("Click");
            star = Content.Load<SoundEffect>("Star");
            racingSong = Content.Load<Song>("racingGame");
            titleSong = Content.Load<Song>("titleSong");
            gameplaySong = Content.Load<Song>("gameplayMusic");

            pausePanel = new I_pausePanel(graphics, Content);
            resource = new gameResource(Content);

            //in game entites
            scene_LV1();

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here 
            #region transition
            if (is_fadeIn)
            {
                if (transitionAlpha < 1)
                {
                    transitionAlpha += transitionSpeed;
                    transitionPanel.tintColor = Color.Black * transitionAlpha;
                }
                else
                {
                    is_fadeIn = false;
                    //transitionSpeed = 0.04f;
                    dialouge = new I_dialouge(graphics, Content.Load<SpriteFont>("dialogueName_Font"), Content.Load<SpriteFont>("dialogueMessege_font"), Content);
                    dialouge.buttonSetup(graphics, Content.Load<SpriteFont>("dialogueNextFont"), 120, 80, 10, new Vector2(1690, 980), "Next", Color.Gray, Color.Black, Color.White, Color.White);
                    resetConfingulation();
                    sceneChange();
                    is_fadeOut = true;
                }
            }
            else if (is_fadeOut)
            {
                if (transitionAlpha > 0)
                {
                    transitionAlpha -= transitionSpeed;
                    transitionPanel.tintColor = Color.Black * transitionAlpha;
                }
                else if (transitionAlpha <= 0)
                    is_fadeOut = false;
            }
            else if(pausePanel.is_acitve)
            {
                pausePanel.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                scene.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            #endregion
            if (is_CameraOn)
                camera.Follow(gameManager.M_PLAYER.sprite);

            dialouge.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            //draw component
            if (is_CameraOn)
                spriteBatch.Begin(transformMatrix: camera.Transform);
            else
                spriteBatch.Begin();

            scene.Draw(spriteBatch);
            spriteBatch.End();

            //draw transition
            spriteBatch.Begin();
            pausePanel.Draw(spriteBatch);
            dialouge.Draw(spriteBatch);
            transitionPanel.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void sceneChange()
        {
            MediaPlayer.Stop();
            switch (sceneToGo)
            {
                case 0:
                    titleLoad();
                    break;
                case 1://Level 1
                    scene_LV1();
                    break;
                case 2://Level 2
                    scene_LV2();
                    break;
                case 3://Level 3
                    scene_LV3();
                    break;
                case 4://Level 4
                    scene_LV4();
                    break;
                case 5://Level 5
                    scene_LV5();
                    break;
                case 6://Level 6
                    scene_LV6();
                    break;
                case 15://star
                    star_display();
                    break;
                case 11://lv1 dialogue
                    dialogue_Lv1();
                    break;
                case 22://lv2 dialogue
                    dialogue_Lv2();
                    break;
                case 33://lv3 dialogue
                    dialogue_Lv3();
                    break;
                case 44://lv4 dialogue
                    dialogue_Lv4();
                    break;
                case 55://lv5 dialogue
                    dialogue_Lv5();
                    break;
                case 66://lv4 dialogue
                    dialogue_Lv6();
                    break;
                case 100://credit scene
                    creadit();
                    break;
                case 101://titleEnd
                    titleLoadEnd();
                    break;
                case 404:
                    beforeLevel4();
                    break;
                case 505:
                    beforeLevel5();
                    break;
                case 111:
                    finaScene();
                    break;
                case 7:
                    secretLevel();
                    break;
                default:
                    break;
            }
        }

        public static void changeSceneTo(int sceneIndex)
        {
            //transitionSpeed = 0.05f;
            MediaPlayer.Stop();
            sceneToGo = sceneIndex;
            //fade transition panel
            is_fadeIn = true;
            scene.entites = new List<I_gameInterface>();
            if (currentLevel == 3)
                racingManager.sound.Stop();
        }

        private static void resetConfingulation()
        {
            scene.entites.Clear();
            is_CameraOn = false;

            gameManager.is_PAUSE = false;
            gameManager.crowds.Clear();
            gameManager.GRID_DATA = null;
            gameManager.playerStep = 0;
            gameManager.ya = null;
        }

        private void titleLoad()
        {
            scene.entites.Add(new I_titleScene(graphics, Content.Load<SpriteFont>("debugFont"), Content.Load<Texture2D>("title_test2")));
            scene.entites.Add(new audioControlPanel(graphics));
        }
        private void titleLoadEnd()
        {
            scene.entites.Add(new titleEnd(graphics, Content.Load<SpriteFont>("debugFont"), Content.Load<Texture2D>("title_test2")));
        }
        private void scene_LV1()
        {
            currentLevel = 1;
            //set star
            gameManager.currentLevel = 1;
            gameManager.star_1_step = 25;
            gameManager.star_2_step = 22;
            gameManager.star_3_step = 21;
            gameManager.sceneNumbrtToGO = 22;

            //grid
            scene.entites.Add(new I_gridBox(6, 6, 1, 7, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            Xml_Data.levelUnwalkableArea shadowArea = Content.Load<Xml_Data.levelUnwalkableArea>("shadowArea_s1");
            for (int i = 0; i < shadowArea.unwalkableAreas.Length; i++)
            {
                gameManager.addShadowArea(shadowArea.unwalkableAreas[i].x, shadowArea.unwalkableAreas[i].y);
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(1, 4)));
            scene.entites[1].DrawOrder = 2;

            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 1), crowdType.fast));
            scene.entites[2].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 5)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 1)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 2), crowdType.slow));
            scene.entites[5].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 3)));
            scene.entites[6].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 5)));
            scene.entites[7].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 1)));
            scene.entites[8].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 4)));
            scene.entites[9].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 2)));
            scene.entites[10].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 4)));
            scene.entites[11].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 2)));
            scene.entites[12].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 3)));
            scene.entites[13].DrawOrder = 3;

            //p earth
            scene.entites.Add(new pEarth(new gridPosition(0, 0), Content, "eath", 4, 1, 2));
            scene.entites[14].DrawOrder = 2;

            //ssr
            scene.entites.Add(new yaDov(new gridPosition(5, 5), Content.Load<Texture2D>("qq"), resource.ssrCard));
            gameManager.ssr = (yaDov)scene.entites[15];
            scene.entites[15].DrawOrder = 3;

            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv1")));
            scene.entites[16].DrawOrder = 0;

            //animation
            scene.entites.Add(new AnimationObject(new Vector2(0, 1280), Content.Load<Texture2D>("motorBike"), 4, 4, new Vector2(0, -500), 6));
            (scene.entites[17] as AnimationObject).lockAnimationRow(3);
            scene.entites[17].DrawOrder = 2;
            scene.entites.Add(new AnimationObject(new Vector2(260, -1500), Content.Load<Texture2D>("motorBike"), 4, 4, new Vector2(260, 1500), 8));
            (scene.entites[18] as AnimationObject).lockAnimationRow(1);
            scene.entites[18].DrawOrder = 2;

            scene.entites.Add(new audioControlPanel(graphics));

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue("A_S1", Content);
        }
        private void scene_LV2()
        {
            currentLevel = 2;
            //set star
            gameManager.currentLevel = 2;
            gameManager.star_1_step = 31;
            gameManager.star_2_step = 28;
            gameManager.star_3_step = 27;
            gameManager.sceneNumbrtToGO = 33;

            //grid
            scene.entites.Add(new I_gridBox(7, 8, 1, 4, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            Xml_Data.levelUnwalkableArea shadowArea = Content.Load<Xml_Data.levelUnwalkableArea>("shadowArea_s2");
            for (int i = 0; i < shadowArea.unwalkableAreas.Length; i++)
            {
                gameManager.addShadowArea(shadowArea.unwalkableAreas[i].x, shadowArea.unwalkableAreas[i].y);
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(0, 0)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(0, 1), Content.Load<Texture2D>("ya"), resource.yaDov));
            gameManager.ya = (yaDov)scene.entites[2];
            scene.entites[2].DrawOrder = 2;
            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 5)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 3)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 2)));
            scene.entites[5].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(0, 3)));
            scene.entites[6].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 1)));
            scene.entites[7].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 2)));
            scene.entites[8].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 6)));
            scene.entites[9].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 1)));
            scene.entites[10].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 3)));
            scene.entites[11].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 4)));
            scene.entites[12].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 5)));
            scene.entites[13].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 2)));
            scene.entites[14].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 1)));
            scene.entites[15].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 3)));
            scene.entites[16].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 4)));
            scene.entites[17].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 5)));
            scene.entites[18].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 6)));
            scene.entites[19].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(6, 3)));
            scene.entites[20].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 1)));
            scene.entites[21].DrawOrder = 3;
            //p earth
            scene.entites.Add(new pEarth(new gridPosition(7, 0), Content, "eath", 4, 1, 2));
            scene.entites[22].DrawOrder = 2;

            //ssr
            scene.entites.Add(new yaDov(new gridPosition(0, 5), Content.Load<Texture2D>("qq"), resource.ssrCard));
            gameManager.ssr = (yaDov)scene.entites[23];
            scene.entites[23].DrawOrder = 3;

            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv2")));
            scene.entites[24].DrawOrder = 0;

            //animation
            scene.entites.Add(new AnimationObject(new Vector2(1680, 1580), Content.Load<Texture2D>("redcar"), 1, 1, new Vector2(1680, -1000), 9));
            (scene.entites[25] as AnimationObject).lockAnimationRow(1);
            (scene.entites[25] as AnimationObject).setSize(3.55f);
            scene.entites[25].DrawOrder = 2;

            scene.entites.Add(new audioControlPanel(graphics));

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue("A_S2", Content);
        }
        private void scene_LV3()
        {
            currentLevel = 3;
            //grid
            scene.entites.Add(new I_gridBox(8, 3, 1, 6, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 0;

            //player
            scene.entites.Add(new playerRacing(Content.Load<Texture2D>("qq"), new gridPosition(1, 5), Content.Load<Texture2D>("motorBike")));
            scene.entites[1].DrawOrder = 2;

            //crowd manager
            scene.entites.Add(new racingManager(Content.Load<Texture2D>("qq"), Content.Load<Texture2D>("motorBike"), Content));
            scene.entites[2].DrawOrder = 1;
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type1");
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type2");
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type3");
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type4");
            (scene.entites[2] as racingManager).updateBackgrounds();

            scene.entites.Add(new audioControlPanel(graphics));
            scene.entites[3].DrawOrder = 5;

            //dialoge
            dialouge.sceneToGo = 404;
            dialouge.addDialogue("A_S3", Content);
        }
        private void scene_LV4()
        {
            currentLevel = 4;
            //set star
            gameManager.currentLevel = 3;
            gameManager.star_1_step = 37;
            gameManager.star_2_step = 35;
            gameManager.star_3_step = 33;
            gameManager.sceneNumbrtToGO = 505;

            //grid
            scene.entites.Add(new I_gridBox(7, 8, 2, 2, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            Xml_Data.levelUnwalkableArea shadowArea = Content.Load<Xml_Data.levelUnwalkableArea>("shadowArea_s4");
            for (int i = 0; i < shadowArea.unwalkableAreas.Length; i++)
            {
                gameManager.addShadowArea(shadowArea.unwalkableAreas[i].x, shadowArea.unwalkableAreas[i].y);
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(0, 5)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(6, 4), Content.Load<Texture2D>("ya"), resource.yaDov));
            gameManager.ya = (yaDov)scene.entites[2];
            scene.entites[2].DrawOrder = 2;
            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 5)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 6)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 5)));
            scene.entites[5].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 5)));
            scene.entites[6].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 5)));
            scene.entites[7].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 6)));
            scene.entites[8].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(6, 2)));
            scene.entites[9].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(6, 3)));
            scene.entites[10].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 0)));
            scene.entites[11].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 1)));
            scene.entites[12].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 1)));
            scene.entites[13].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 2)));
            scene.entites[14].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 1)));
            scene.entites[15].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 1)));
            scene.entites[16].DrawOrder = 3;
            //p earth
            scene.entites.Add(new pEarth(new gridPosition(0, 2), Content, "eath", 4, 1, 2));
            scene.entites[17].DrawOrder = 2;
            //ssr
            scene.entites.Add(new yaDov(new gridPosition(5, 3), Content.Load<Texture2D>("qq"), resource.ssrCard));
            gameManager.ssr = (yaDov)scene.entites[18];
            scene.entites[18].DrawOrder = 3;
            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv4")));
            scene.entites[19].DrawOrder = 0;

            scene.entites.Add(new audioControlPanel(graphics));

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue("A_S4", Content);
        } 
        private void scene_LV5()
        {
            currentLevel = 5;
            //set star
            gameManager.currentLevel = 4;
            gameManager.star_1_step = 80;
            gameManager.star_2_step = 78;
            gameManager.star_3_step = 76;
            gameManager.sceneNumbrtToGO = 66;

            //grid
            scene.entites.Add(new I_gridBox(8, 11, 0, 3, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            Xml_Data.levelUnwalkableArea shadowArea = Content.Load<Xml_Data.levelUnwalkableArea>("shadowArea_s5");
            for (int i = 0; i < shadowArea.unwalkableAreas.Length; i++)
            {
                gameManager.addShadowArea(shadowArea.unwalkableAreas[i].x, shadowArea.unwalkableAreas[i].y);
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(4, 7)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(10, 1), Content.Load<Texture2D>("ya"), resource.yaDov));
            gameManager.ya = (yaDov)scene.entites[2];
            scene.entites[2].DrawOrder = 2;

            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(0, 5)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 5)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 5)));
            scene.entites[5].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(0, 0)));
            scene.entites[6].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 2)));
            scene.entites[7].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 1)));
            scene.entites[8].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 2)));
            scene.entites[9].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 1)));
            scene.entites[10].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 0)));
            scene.entites[11].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 0)));
            scene.entites[12].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(6, 1)));
            scene.entites[13].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(8, 1)));
            scene.entites[14].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 0)));
            scene.entites[15].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(9, 0)));
            scene.entites[16].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 2)));
            scene.entites[17].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(9, 2)));
            scene.entites[18].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(10, 4)));
            scene.entites[19].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(10, 0)));
            scene.entites[20].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(10, 2)));
            scene.entites[21].DrawOrder = 3;
            //p earth
            scene.entites.Add(new pEarth(new gridPosition(4, 5), Content, "eath", 4, 1, 2));
            scene.entites[22].DrawOrder = 3;
            //ssr
            scene.entites.Add(new yaDov(new gridPosition(8, 5), Content.Load<Texture2D>("qq"), resource.ssrCard));
            gameManager.ssr = (yaDov)scene.entites[23];
            scene.entites[23].DrawOrder = 3;
            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv5")));
            scene.entites[24].DrawOrder = 0;

            scene.entites.Add(new audioControlPanel(graphics));

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue("A_S5", Content);
        }
        private void scene_LV6()
        {
            currentLevel = 6;
            //set star
            gameManager.currentLevel = 5;
            gameManager.star_1_step = 50;
            gameManager.star_2_step = 48;
            gameManager.star_3_step = 46;
            gameManager.sceneNumbrtToGO = 111;

            //grid
            scene.entites.Add(new I_gridBox(17, 7, 0, 5, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            Xml_Data.levelUnwalkableArea shadowArea = Content.Load<Xml_Data.levelUnwalkableArea>("shadowArea_s6");
            for (int i = 0; i < shadowArea.unwalkableAreas.Length; i++)
            {
                gameManager.addShadowArea(shadowArea.unwalkableAreas[i].x, shadowArea.unwalkableAreas[i].y);
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(3, 16)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(3, 11), Content.Load<Texture2D>("ya"), resource.yaDov));
            gameManager.ya = (yaDov)scene.entites[2];
            scene.entites[2].DrawOrder = 2;
            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 15)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 15)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 15)));
            scene.entites[5].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 16)));
            scene.entites[6].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 16)));
            scene.entites[7].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 13)));
            scene.entites[8].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 13)));
            scene.entites[9].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 13)));
            scene.entites[10].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 12)));
            scene.entites[11].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 12)));
            scene.entites[12].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 12)));
            scene.entites[13].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 12)));
            scene.entites[14].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 10)));
            scene.entites[15].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 9)));
            scene.entites[16].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 8)));
            scene.entites[17].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 8)));
            scene.entites[18].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 6)));
            scene.entites[19].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 6)));
            scene.entites[20].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 4)));
            scene.entites[21].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 4)));
            scene.entites[22].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 0)));
            scene.entites[23].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 0)));
            scene.entites[24].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 0)));
            scene.entites[25].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 0)));
            scene.entites[26].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 1)));
            scene.entites[27].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 2)));
            scene.entites[28].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(3, 2)));
            scene.entites[29].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 2)));
            scene.entites[30].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 1)));
            scene.entites[31].DrawOrder = 3;

            //p earth
            scene.entites.Add(new pEarth(new gridPosition(3, 0), Content, "eath", 4, 1, 2));
            scene.entites[32].DrawOrder = 3;
            //ssr
            scene.entites.Add(new yaDov(new gridPosition(1, 3), Content.Load<Texture2D>("qq"), resource.ssrCard));
            gameManager.ssr = (yaDov)scene.entites[33];
            scene.entites[33].DrawOrder = 3;
            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv6")));
            scene.entites[34].DrawOrder = 0;

            //scene.entites.Add(new audioControlPanel(graphics));

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue("A_S6", Content);
        }
        public void secretLevel()
        {
            currentLevel = 7;
            //set star
            gameManager.currentLevel = 6;
            gameManager.star_1_step = 50;
            gameManager.star_2_step = 48;
            gameManager.star_3_step = 46;
            gameManager.sceneNumbrtToGO = 111;

            scene.entites.Add(new I_gridBox(7, 13, 1, 2, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 1;
            Xml_Data.levelUnwalkableArea shadowArea = Content.Load<Xml_Data.levelUnwalkableArea>("shadowArea_s7");
            for (int i = 0; i < shadowArea.unwalkableAreas.Length; i++)
            {
                gameManager.addShadowArea(shadowArea.unwalkableAreas[i].x, shadowArea.unwalkableAreas[i].y);
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(6, 0)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(0, 0), Content.Load<Texture2D>("ya"), resource.yaDov));
            gameManager.ya = (yaDov)scene.entites[2];
            scene.entites[2].DrawOrder = 2;
            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(4, 2)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 4)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 2)));
            scene.entites[5].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 3)));
            scene.entites[6].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 2)));
            scene.entites[7].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 3)));
            scene.entites[8].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(8, 2)));
            scene.entites[9].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(8, 4)));
            scene.entites[10].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(10, 2)));
            scene.entites[11].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(10, 4)));
            scene.entites[12].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 2)));
            scene.entites[13].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 4)));
            scene.entites[14].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 0)));
            scene.entites[15].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(5, 0)));
            scene.entites[16].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 3)));
            scene.entites[17].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(7, 4)));
            scene.entites[18].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(9, 3)));
            scene.entites[19].DrawOrder = 3;
            //p earth
            scene.entites.Add(new pEarth(new gridPosition(0, 6), Content, "fuckingHolyDog", 2, 1, 5));
            scene.entites[20].DrawOrder = 2;

            //ssr
            scene.entites.Add(new yaDov(new gridPosition(12, 6), Content.Load<Texture2D>("qq"), resource.ssrCard));
            gameManager.ssr = (yaDov)scene.entites[21];
            scene.entites[21].DrawOrder = 3;
            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("secretLevel")));
            scene.entites[22].DrawOrder = 0;
            //bg
            scene.entites.Add(new I_bgGame(kaninKitRail.getBoxTexture(graphics, 1920, 1080, Color.Black * 0.7f, 0)));
            scene.entites[23].DrawOrder = 4;

            scene.entites.Add(new audioControlPanel(graphics));

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue("A_S6", Content);
        }
        private void star_display()
        {
            scene.entites.Add(new starDisplay(Content.Load<Texture2D>("Hearth")));
        }
        private void dialogue_Lv1()
        {
            //dialoge
            dialouge.sceneToGo = 1;
            dialouge.addDialogue("B_S1", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("dan1"), "Chapter 1", Content));
        }
        private void dialogue_Lv2()
        {
            //dialoge
            dialouge.sceneToGo = 2;
            dialouge.addDialogue("B_S2", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("dan2"), "Chapter 2", Content));
        }
        private void dialogue_Lv3()
        {
            //dialoge
            dialouge.sceneToGo = 3;
            dialouge.addDialogue("B_S3", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("dan2"), "Chapter 2.5", Content));
        }
        private void dialogue_Lv4()
        {
            //dialoge
            dialouge.sceneToGo = 4;
            dialouge.addDialogue("B_S4", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("dan3"), "Chapter 3", Content));
        }
        private void dialogue_Lv5()
        {
            //dialoge
            dialouge.sceneToGo = 5;
            dialouge.addDialogue("B_S5", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("dan4"), "Chapter 4", Content));
        }
        private void dialogue_Lv6()
        {
            //dialoge
            dialouge.sceneToGo = 6;
            dialouge.addDialogue("B_S6", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("dan5"), "Chapter 5", Content));
        }
        private void creadit()
        {
            scene.entites.Add(new I_Credit(Content, graphics));
            (scene.entites[0] as I_Credit).newBackgroundTexture = Content.Load<Texture2D>("type1");
            (scene.entites[0] as I_Credit).newBackgroundTexture = Content.Load<Texture2D>("type2");
            (scene.entites[0] as I_Credit).newBackgroundTexture = Content.Load<Texture2D>("type3");
            (scene.entites[0] as I_Credit).newBackgroundTexture = Content.Load<Texture2D>("type4");
            (scene.entites[0] as I_Credit).updateBackgrounds();

            string filepath = Path.Combine(@"Content\credit.txt");
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            while(!sr.EndOfStream)
            {
                (scene.entites[0] as I_Credit).loadCredit(sr.ReadLine());
            }
            sr.Close();
        }
        private void beforeLevel4()
        {
            //grid
            scene.entites.Add(new I_gridBox(4, 10, 3, 4, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            gameManager.addShadowArea(0, 0);
            gameManager.addShadowArea(1, 0);
            gameManager.addShadowArea(2, 0);
            gameManager.addShadowArea(3, 0);
            gameManager.addShadowArea(4, 0);
            gameManager.addShadowArea(0, 1);
            gameManager.addShadowArea(1, 1);
            gameManager.addShadowArea(2, 1);
            gameManager.addShadowArea(3, 1);
            gameManager.addShadowArea(4, 1);
            gameManager.addShadowArea(6, 0);
            gameManager.addShadowArea(8, 0);

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(0, 2), 44));
            scene.entites[1].DrawOrder = 2;

            //p earth
            scene.entites.Add(new pEarth(new gridPosition(7, 0)));
            scene.entites[2].DrawOrder = 3;

            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("BeforeLevel4")));
            scene.entites[3].DrawOrder = 0;
        }
        private void beforeLevel5()
        {
            //grid
            scene.entites.Add(new I_gridBox(4, 10, 2, 3, Content.Load<SpriteFont>("debugFont"), graphics, false));
            scene.entites[0].DrawOrder = 1;
            for (int i = 0; i <= 2; i++)
            {
                for (int k = 0; k <= 8; k++)
                {
                    gameManager.addShadowArea(k, i);
                }
            }

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(0, 3), 55));
            scene.entites[1].DrawOrder = 2;

            //p earth
            scene.entites.Add(new pEarth(new gridPosition(9, 0)));
            scene.entites[2].DrawOrder = 3;

            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("BeforeLv5")));
            scene.entites[3].DrawOrder = 0;
        }
        private void finaScene()
        {
            //dialoge
            dialouge.sceneToGo = 100;
            dialouge.addDialogue("B_S6", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("B_S1_image"), "น้องสีเงิน will return", Content, true));
        }
    }
}
