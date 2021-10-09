using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;

namespace OneMoreStepToLoveYou.GameInterface
{
    public struct dialouge
    {
        public string name;
        public string messege;
        public Texture2D image;
        public float imageScale;

        public dialouge(string name, string messege, Texture2D image, float imageScale)
        {
            this.name = name;
            this.messege = messege;
            this.image = image;
            this.imageScale = imageScale;
        }

        public char getMessegeAt(int index)
        {
            return messege[index];
        }
    }

    public class I_dialouge : I_gameInterface
    {
        public int DrawOrder { get; set; }

        private bool is_play;

        public float transitionSpeed = 0.02f;
        public float MAX_BG_midderAlpha = 0.9f;
        private float upperDestinationY = -340f;
        private float lowerDestinationY = 750f;
        private float transitionMovingSpeed = 15;

        private float BG_Alpha = 0;
        private Sprite BG_upper;
        private Sprite BG_lower;
        private Sprite BG_midder;
        private Sprite characterSprite;

        private int currentDialouge = 0;
        private List<dialouge> dialogues = new List<dialouge>();

        private text nameText;
        private text messegeText;
        private string name = "";
        private string messege = "";
        private int currentMessege = -1;
        private float textSpeed = (float)1 / 20;
        private float TotalElapsed;

        private float characterAlpha = 0;
        private float characterTransitionSpeed = 0.02f;

        public button m_button;

        public int sceneToGo;

        public void addDialogue(dialouge newDialouge)
        {
            dialogues.Add(newDialouge);
        }

        public void addDialogue(string scene, ContentManager content)
        {
            Xml_Data.dialoguesData data = content.Load<Xml_Data.dialoguesData>(scene);

            foreach (Xml_Data.diaglogueInfo item in data.dialogueInfos)
            {
                addDialogue(new dialouge(item.name, item.messege, content.Load<Texture2D>(item.imageName), item.imageScale));
            }
        }

        public I_dialouge(GraphicsDeviceManager graphics, SpriteFont nameFont, SpriteFont messegeFont)
        {
            Vector2 sceneSize = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            Vector2 startPoint = Vector2.Zero;
            BG_upper = new Sprite(kaninKitRail.getBoxTexture(graphics, (int)sceneSize.X, (int)sceneSize.Y / 2, Color.White, 0), startPoint - new Vector2(0, 150), Color.White * 0);
            BG_lower = new Sprite(kaninKitRail.getBoxTexture(graphics, (int)sceneSize.X, (int)sceneSize.Y / 2, Color.White, 0), startPoint + new Vector2(0, 450), Color.White * 0);
            BG_midder = new Sprite(kaninKitRail.getBoxTexture(graphics, (int)sceneSize.X, (int)sceneSize.Y, Color.Black, 0), startPoint, Color.White * 0);
            characterSprite = new Sprite(null, Vector2.Zero, Color.White * 0);

            nameText = new text(nameFont, Color.Black, new Vector2(50, 780));
           // float nameX = (1920 / 2) - nameFont.MeasureString("1234567").X;
            //nameText = new text(nameFont, Color.Black, new Vector2());
            messegeText = new text(messegeFont, Color.Black, new Vector2(80, 840));
        }

        public void buttonSetup(GraphicsDeviceManager graphics, SpriteFont font, int b_widht, int b_height, int strok, Vector2 position, string messege, Color t_idleColor, Color t_hoverColor, Color b_idleColor, Color b_hoverColor)
        {
            m_button = new button(graphics, font, b_widht, b_height, strok, position, messege, t_idleColor, t_hoverColor, b_idleColor, b_hoverColor);
            m_button.Click += nextDialogue;
        }

        public void Update(float animator_elapsed)
        {
            keyboard.GetState();
            if(is_play)
            {
                //fade
                if(characterAlpha < 1)
                {
                    characterAlpha += characterTransitionSpeed;
                    characterSprite.tintColor = Color.White * characterAlpha;
                }
                if (BG_Alpha < 1)
                {
                    BG_Alpha += transitionSpeed;
                    BG_lower.tintColor = Color.White * BG_Alpha;
                    BG_upper.tintColor = Color.White * BG_Alpha;
                    if (BG_Alpha < MAX_BG_midderAlpha)
                        BG_midder.tintColor = Color.White * BG_Alpha;
                }
                //move
                if(BG_lower.position.Y < lowerDestinationY || BG_upper.position.Y > upperDestinationY)
                {
                    if (BG_lower.position.Y < lowerDestinationY)
                        BG_lower.position.Y += transitionMovingSpeed;
                    if (BG_upper.position.Y > upperDestinationY)
                        BG_upper.position.Y -= transitionMovingSpeed;
                }

                TotalElapsed += animator_elapsed;
                if (BG_Alpha > 0.9f && TotalElapsed > textSpeed && currentMessege < dialogues[currentDialouge].messege.Length - 1)
                {
                    currentMessege += 1;
                    messege += dialogues[currentDialouge].getMessegeAt(currentMessege);
                    //currentMessege = currentMessege % dialogues[currentDialouge].messege.Length;
                    TotalElapsed -= textSpeed;
                }

                m_button.Update();
                if (keyboard.HasBeenPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                    nextDialogue();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!is_play)
                return;

            BG_midder.Draw(spriteBatch);
            BG_upper.Draw(spriteBatch);
            characterSprite.Draw(spriteBatch, dialogues[currentDialouge].imageScale);
            BG_lower.Draw(spriteBatch);
            nameText.drawFont(spriteBatch, name);
            messegeText.drawFont(spriteBatch, messege);
            if(BG_Alpha >= 1)
                m_button.Draw(spriteBatch);
        }

        private void updateDialogue()
        {
            characterSprite.gameSprite = dialogues[currentDialouge].image;
            characterSprite.position = kaninKitRail.getCenterPoint(1920, 1080) -
                new Vector2(dialogues[currentDialouge].image.Width / 2, dialogues[currentDialouge].image.Height / 2) * dialogues[currentDialouge].imageScale;
            characterSprite.position.Y -= 150;

            name = dialogues[currentDialouge].name;
        }

        public void nextDialogue()
        {
            //end
            if (currentDialouge >= dialogues.Count - 1)
            {
                Game1.changeSceneTo(sceneToGo);
                return;
            }

            characterAlpha = 0;
            currentDialouge += 1;
            messege = "";
            currentMessege = -1;
            updateDialogue();
        }

        public void nextDialogue(object sender, System.EventArgs e)
        {
            nextDialogue();
        }

        public void dialogeOn()
        {
            gameManager.is_PAUSE = true;
            is_play = true;
            updateDialogue();
        }
    }
}
