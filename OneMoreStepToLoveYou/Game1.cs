using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OneMoreStepToLoveYou.GameInterface;
using OneMoreStepToLoveYou.Entites;
using System;

namespace OneMoreStepToLoveYou
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //scene
        public static I_sceneManager scene = new I_sceneManager();
        text debugText;
        static int sceneToGo;

        //transitional
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen = true;
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
            dialouge = new I_dialouge(graphics, Content.Load<SpriteFont>("dialogueName_Font"), Content.Load<SpriteFont>("dialogueMessege_font"));
            dialouge.buttonSetup(graphics, Content.Load<SpriteFont>("dialogueNextFont"), 120, 80, 10, new Vector2(1690, 980), "Next", Color.Gray, Color.Black, Color.White, Color.White);

            //debug texts
            debugText = new text(Content.Load<SpriteFont>("debugFont"), Color.Black, Vector2.Zero);

            //in game entites
            titleLoad();
            //scene_LV1();

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
                    dialouge = new I_dialouge(graphics, Content.Load<SpriteFont>("dialogueName_Font"), Content.Load<SpriteFont>("dialogueMessege_font"));
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

                //load some content
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

            //bg_sprite.Draw(spriteBatch);
            scene.Draw(spriteBatch);
            /*string debugMessege = "";
            for (int i = 0; i < gameManager.GRID_ROW; i++)
            {
                for (int j = 0; j < gameManager.GRID_COLUMN; j++)
                {
                    debugMessege += gameManager.GRID_DATA[i, j].type.ToString() + "   | ";
                }
                debugMessege += "\n";
            }
            debugMessege += "\n\n\n\n" + gameManager.playerStep;
            debugText.drawFont(spriteBatch, debugMessege);*/
            spriteBatch.End();

            //draw transition
            spriteBatch.Begin();
            dialouge.Draw(spriteBatch);
            transitionPanel.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void sceneChange()
        {
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
                default:
                    break;
            }
        }

        public static void changeSceneTo(int sceneIndex)
        {
            //transitionSpeed = 0.05f;
            sceneToGo = sceneIndex;
            //fade transition panel
            is_fadeIn = true;
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
        }
        private void scene_LV1()
        {
            //set star
            gameManager.currentLevel = 1;
            gameManager.star_1_step = 25;
            gameManager.star_2_step = 22;
            gameManager.star_3_step = 21;

            //grid
            scene.entites.Add(new I_gridBox(6, 6, 1, 7, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 1;
            gameManager.addShadowArea(0, 1);
            gameManager.addShadowArea(0, 2);
            gameManager.addShadowArea(0, 3);
            gameManager.addShadowArea(0, 4);
            gameManager.addShadowArea(0, 5);

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(1, 4)));
            scene.entites[1].DrawOrder = 2;

            //crowd
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 1)));
            scene.entites[2].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(1, 5)));
            scene.entites[3].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 1)));
            scene.entites[4].DrawOrder = 3;
            scene.entites.Add(new crowd(Content.Load<Texture2D>("Player"), new gridPosition(2, 2)));
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
            scene.entites.Add(new pEarth(new gridPosition(0, 0), Content, "CoketumpBreathe", 3, 1, 10));
            scene.entites[14].DrawOrder = 3;

            //ssr
            scene.entites.Add(new yaDov(new gridPosition(5, 5), Content.Load<Texture2D>("qq")));
            gameManager.ssr = (yaDov)scene.entites[15];
            scene.entites[15].DrawOrder = 3;

            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv1")));
            scene.entites[16].DrawOrder = 0;

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue(new dialouge("p'Earth", "omae wa mou shindeiru", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("pEarth", "rasengan!!!!!", Content.Load<Texture2D>("pEarthRasengunSaiNaKung"), 0.57f));
            dialouge.addDialogue(new dialouge("Nong Bao", "NANIIII!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Ahaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Content.Load<Texture2D>("nongBao"), 0.5f));
        }
        private void scene_LV2()
        {
            //set star
            gameManager.currentLevel = 2;
            gameManager.star_1_step = 31;
            gameManager.star_2_step = 28;
            gameManager.star_3_step = 27;

            //grid
            scene.entites.Add(new I_gridBox(7, 8, 2, 4, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 1;
            gameManager.addShadowArea(0, 4);
            gameManager.addShadowArea(0, 6);
            gameManager.addShadowArea(1, 0);
            gameManager.addShadowArea(2, 0);
            gameManager.addShadowArea(5, 0);
            gameManager.addShadowArea(3, 0);
            gameManager.addShadowArea(4, 0);
            gameManager.addShadowArea(6, 0);
            gameManager.addShadowArea(7, 2);
            gameManager.addShadowArea(7, 3);
            gameManager.addShadowArea(7, 4);
            gameManager.addShadowArea(7, 5);
            gameManager.addShadowArea(7, 6);

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(0, 0)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(0, 1), Content.Load<Texture2D>("ya")));
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
            scene.entites.Add(new pEarth(new gridPosition(7, 0), Content, "CoketumpBreathe", 3, 1, 10));
            scene.entites[22].DrawOrder = 2;

            //ssr
            scene.entites.Add(new yaDov(new gridPosition(0, 5), Content.Load<Texture2D>("qq")));
            gameManager.ssr = (yaDov)scene.entites[23];
            scene.entites[23].DrawOrder = 3;

            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv2")));
            scene.entites[24].DrawOrder = 0;

            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue(new dialouge("pEarth", "see leuuang", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Yellow!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "ma muuang", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Mango!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "rot gra ba", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Vego!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "fai chaek", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "zippo!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "meet", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "e to!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "chut chan nai", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Wago!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
        }
        private void scene_LV3()
        {
            //grid
            scene.entites.Add(new I_gridBox(8, 3, 1, 6, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 0;

            //player
            scene.entites.Add(new playerRacing(Content.Load<Texture2D>("qq"), new gridPosition(1, 5)));
            scene.entites[1].DrawOrder = 3;

            //crowd manager
            scene.entites.Add(new racingManager(Content.Load<Texture2D>("qq")));
            scene.entites[2].DrawOrder = 1;
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type1");
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type2");
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type3");
            (scene.entites[2] as racingManager).newBackgroundTexture = Content.Load<Texture2D>("type4");
            (scene.entites[2] as racingManager).updateBackgrounds();
        }
        private void scene_LV4()
        {
            //set star
            gameManager.currentLevel = 3;
            gameManager.star_1_step = 37;
            gameManager.star_2_step = 35;
            gameManager.star_3_step = 33;

            //grid
            scene.entites.Add(new I_gridBox(7, 8, 2, 4, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 1;
            gameManager.addShadowArea(0, 3);
            gameManager.addShadowArea(1, 2);
            gameManager.addShadowArea(1, 3);
            gameManager.addShadowArea(0, 4);
            gameManager.addShadowArea(1, 4);
            gameManager.addShadowArea(2, 2);
            gameManager.addShadowArea(3, 0);
            gameManager.addShadowArea(3, 2);
            gameManager.addShadowArea(4, 2);
            gameManager.addShadowArea(4, 3);
            gameManager.addShadowArea(4, 4);
            gameManager.addShadowArea(5, 4);
            gameManager.addShadowArea(7, 4);
            gameManager.addShadowArea(7, 5);

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(0, 5)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(6, 4), Content.Load<Texture2D>("ya")));
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
            scene.entites.Add(new pEarth(new gridPosition(0, 2), Content, "CoketumpBreathe", 3, 1, 10));
            scene.entites[17].DrawOrder = 2;
            //ssr
            scene.entites.Add(new yaDov(new gridPosition(5, 3), Content.Load<Texture2D>("qq")));
            gameManager.ssr = (yaDov)scene.entites[18];
            scene.entites[18].DrawOrder = 3;
            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv4")));
            scene.entites[19].DrawOrder = 0;
            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue(new dialouge("pEarth", "see leuuang", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Yellow!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "ma muuang", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Mango!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "rot gra ba", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Vego!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "fai chaek", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "zippo!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "meet", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "e to!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "chut chan nai", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Wago!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
        } 
        private void scene_LV5()
        {
            //set star
            gameManager.currentLevel = 4;
            gameManager.star_1_step = 80;
            gameManager.star_2_step = 78;
            gameManager.star_3_step = 76;

            //grid
            scene.entites.Add(new I_gridBox(8, 11, 1, 2, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 1;
            gameManager.addShadowArea(1, 6);
            gameManager.addShadowArea(2, 6);
            gameManager.addShadowArea(3, 6);
            gameManager.addShadowArea(4, 6);
            gameManager.addShadowArea(5, 6);
            gameManager.addShadowArea(6, 6);
            gameManager.addShadowArea(7, 6);
            gameManager.addShadowArea(8, 6);
            gameManager.addShadowArea(1, 3);
            gameManager.addShadowArea(2, 3);
            gameManager.addShadowArea(3, 3);
            gameManager.addShadowArea(4, 3);
            gameManager.addShadowArea(5, 3);
            gameManager.addShadowArea(6, 3);
            gameManager.addShadowArea(7, 3);
            gameManager.addShadowArea(8, 3);
            gameManager.addShadowArea(9, 3);
            gameManager.addShadowArea(9, 4);
            gameManager.addShadowArea(9, 6);
            gameManager.addShadowArea(1, 4);
            gameManager.addShadowArea(2, 4);

            gameManager.addShadowArea(7, 4);
            gameManager.addShadowArea(8, 4);

            gameManager.addShadowArea(5, 0);
            gameManager.addShadowArea(5, 2);
            gameManager.addShadowArea(6, 4);
            gameManager.addShadowArea(6, 5);
            gameManager.addShadowArea(7, 5);
            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(4, 7)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(10, 1), Content.Load<Texture2D>("ya")));
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
            scene.entites.Add(new pEarth(new gridPosition(4, 5), Content, "CoketumpBreathe", 3, 1, 10));
            scene.entites[22].DrawOrder = 3;
            //ssr
            scene.entites.Add(new yaDov(new gridPosition(8, 5), Content.Load<Texture2D>("qq")));
            gameManager.ssr = (yaDov)scene.entites[23];
            scene.entites[23].DrawOrder = 3;
            //bg
            scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv5")));
            scene.entites[24].DrawOrder = 0;
            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue(new dialouge("pEarth", "see leuuang", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Yellow!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "ma muuang", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Mango!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "rot gra ba", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Vego!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "fai chaek", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "zippo!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "meet", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "e to!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("pEarth", "chut chan nai", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Wago!!!", Content.Load<Texture2D>("nongBao"), 0.5f));
        }
        private void scene_LV6()
        {
            //set star
            gameManager.currentLevel = 5;
            gameManager.star_1_step = 50;
            gameManager.star_2_step = 48;
            gameManager.star_3_step = 46;

            //grid
            scene.entites.Add(new I_gridBox(17, 7, 0, 7, Content.Load<SpriteFont>("debugFont"), graphics));
            scene.entites[0].DrawOrder = 1;
            gameManager.addShadowArea(0, 0);
            gameManager.addShadowArea(0, 1);
            gameManager.addShadowArea(0, 2);
            gameManager.addShadowArea(0, 3);
            gameManager.addShadowArea(0, 4);
            gameManager.addShadowArea(0, 5);
            gameManager.addShadowArea(0, 6);

            gameManager.addShadowArea(0, 8);
            gameManager.addShadowArea(0, 9);
            gameManager.addShadowArea(0, 10);
            gameManager.addShadowArea(0, 11);
            gameManager.addShadowArea(0, 12);
            gameManager.addShadowArea(0, 13);
            gameManager.addShadowArea(0, 14);
            gameManager.addShadowArea(0, 15);
            gameManager.addShadowArea(0, 16);
            gameManager.addShadowArea(6, 0);
            gameManager.addShadowArea(6, 1);
            gameManager.addShadowArea(6, 2);
            gameManager.addShadowArea(6, 3);
            gameManager.addShadowArea(6, 4);
            gameManager.addShadowArea(6, 5);
            gameManager.addShadowArea(6, 6);

            gameManager.addShadowArea(6, 8);
            gameManager.addShadowArea(6, 9);
            gameManager.addShadowArea(6, 10);
            gameManager.addShadowArea(6, 11);
            gameManager.addShadowArea(6, 12);
            gameManager.addShadowArea(6, 13);
            gameManager.addShadowArea(6, 14);
            gameManager.addShadowArea(6, 15);
            gameManager.addShadowArea(6, 16);
            gameManager.addShadowArea(1, 11);
            gameManager.addShadowArea(2, 11);
            gameManager.addShadowArea(4, 11);
            gameManager.addShadowArea(5, 11);
            gameManager.addShadowArea(3, 7);
            gameManager.addShadowArea(1, 8);
            gameManager.addShadowArea(1, 9);
            gameManager.addShadowArea(1, 10);
            gameManager.addShadowArea(5, 8);
            gameManager.addShadowArea(5, 9);
            gameManager.addShadowArea(5, 10);
            gameManager.addShadowArea(1, 6);
            gameManager.addShadowArea(5, 6);
            gameManager.addShadowArea(2, 3);
            gameManager.addShadowArea(4, 3);
            gameManager.addShadowArea(1, 2);
            gameManager.addShadowArea(5, 2);

            //player
            scene.entites.Add(new player(Content.Load<Texture2D>("qq"), new gridPosition(3, 16)));
            scene.entites[1].DrawOrder = 2;
            //ya dob
            scene.entites.Add(new yaDov(new gridPosition(3, 11), Content.Load<Texture2D>("ya")));
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
            scene.entites.Add(new pEarth(new gridPosition(3, 0), Content, "CoketumpBreathe", 3, 1, 10));
            scene.entites[32].DrawOrder = 3;
            //ssr
            scene.entites.Add(new yaDov(new gridPosition(1, 3), Content.Load<Texture2D>("qq")));
            gameManager.ssr = (yaDov)scene.entites[33];
            scene.entites[33].DrawOrder = 3;
            //bg
            //scene.entites.Add(new I_bgGame(Content.Load<Texture2D>("Lv6")));
            //scene.entites[34].DrawOrder = 0;
            //dialoge
            dialouge.sceneToGo = 15;
            dialouge.addDialogue(new dialouge("pEarth", "omae wa mou shindeiru", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("pEarth", "rasengan!!!!!", Content.Load<Texture2D>("pEarthRasengunSaiNaKung"), 0.57f));
            dialouge.addDialogue(new dialouge("Nong Bao", "NANIIII!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Ahaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Content.Load<Texture2D>("nongBao"), 0.5f));
        }
        private void star_display()
        {
            scene.entites.Add(new starDisplay(Content.Load<Texture2D>("Hearth")));
        }
        private void dialogue_Lv1()
        {
            //dialoge
            dialouge.sceneToGo = 1;
            /*dialouge.addDialogue(new dialouge("พี่โลก", "คือลื้อ", Content.Load<Texture2D>("pEarthStand2"), 1.2f));
            dialouge.addDialogue(new dialouge("pEarth", "rasengan!!!!!", Content.Load<Texture2D>("pEarthRasengunSaiNaKung"), 0.57f));
            dialouge.addDialogue(new dialouge("Nong Bao", "NANIIII!!", Content.Load<Texture2D>("nongBao"), 0.5f));
            dialouge.addDialogue(new dialouge("Nong Bao", "Ahaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Content.Load<Texture2D>("nongBao"), 0.5f));*/
            dialouge.addDialogue("B_S1", Content);
            dialouge.MAX_BG_midderAlpha = 0.7f;

            //scene
            scene.entites.Add(new I_dialogue_beforeScene(Content.Load<Texture2D>("B_S1_image")));
        }
    }
}
