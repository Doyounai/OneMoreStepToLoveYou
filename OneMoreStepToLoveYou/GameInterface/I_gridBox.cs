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
    public class I_gridBox : I_gameInterface
    {
        public int DrawOrder { get; set; }
        private text[,] debugText;
        private bool is_Debug = true;

        public I_gridBox(int row, int column, int startRow, int startColumn, SpriteFont font, GraphicsDeviceManager graphics)
        {
            //confingulation
            gameManager.GRID_ROW = row;
            gameManager.GRID_COLUMN = column;
            gameManager.GRID_WIDTH = 120;
            gameManager.GRID_HEIGHT = 120;
            if (graphics.PreferredBackBufferHeight > gameManager.GRID_HEIGHT * row)
            {
                //gameManager.GRID_STARTPOSITION = kaninKitRail.getCenterPoint(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                //gameManager.GRID_STARTPOSITION -= kaninKitRail.getCenterPoint(column * gameManager.GRID_WIDTH, row * gameManager.GRID_HEIGHT);
                gameManager.GRID_STARTPOSITION = new Vector2(startColumn * gameManager.GRID_WIDTH, startRow * gameManager.GRID_HEIGHT);
                Game1.is_CameraOn = false;
            }
            else
            {
                //gameManager.GRID_STARTPOSITION = kaninKitRail.getCenterPoint(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                //gameManager.GRID_STARTPOSITION -= kaninKitRail.getCenterPoint(column * gameManager.GRID_WIDTH, row * gameManager.GRID_HEIGHT);
                //gameManager.GRID_STARTPOSITION.Y -= (gameManager.GRID_HEIGHT * row) - graphics.PreferredBackBufferHeight;
                int sceneUperOver = (row * gameManager.GRID_HEIGHT) - ((row * gameManager.GRID_HEIGHT) % 1080);
                gameManager.GRID_STARTPOSITION = new Vector2(startColumn * gameManager.GRID_WIDTH, -sceneUperOver);
                Game1.is_CameraOn = true;
            }
            gameManager.GRID_DATA = new gridItem[row, column];
            debugText = new text[row, column];

            #region genarate test box texture
            //test box
            Color colorA = new Color(51, 122, 184) * 0;
            Color colorB = new Color(42, 183, 155) * 0;
            int strokSize = 5;
            Texture2D[] originGridItem = new Texture2D[2];
            /*
            Texture2D rect1 = new Texture2D(graphics.GraphicsDevice, gameManager.GRID_WIDTH, gameManager.GRID_HEIGHT);
            Color[] colorData = new Color[gameManager.GRID_WIDTH * gameManager.GRID_HEIGHT];

            //color A
            //row
            for (int i = 0; i < gameManager.GRID_HEIGHT; i++)
            {
                //column
                for (int j = 0; j < gameManager.GRID_WIDTH; j++)
                {
                    //boarder
                    if (i <= strokSize || i >= gameManager.GRID_HEIGHT - strokSize || j <= strokSize || j >= gameManager.GRID_WIDTH - strokSize)
                    {
                        colorData[(gameManager.GRID_WIDTH * i) + j] = new Color(255, 255, 255, 1);
                    }//fill
                    else
                    {
                        colorData[(gameManager.GRID_WIDTH * i) + j] = colorA;
                    }

                }
            }
            rect1.SetData(colorData);

            originGridItem[0] = rect1;

            //Color B
            Texture2D rect2 = new Texture2D(graphics.GraphicsDevice, gameManager.GRID_WIDTH, gameManager.GRID_HEIGHT);
            for (int i = 0; i < gameManager.GRID_HEIGHT; i++)
            {
                //column
                for (int j = 0; j < gameManager.GRID_WIDTH; j++)
                {
                    //boarder
                    if (i <= strokSize || i >= gameManager.GRID_HEIGHT - strokSize || j <= strokSize || j >= gameManager.GRID_WIDTH - strokSize)
                    {
                        colorData[(gameManager.GRID_WIDTH * i) + j] = new Color(255, 255, 255, 1);
                    }//fill
                    else
                    {
                        colorData[(gameManager.GRID_WIDTH * i) + j] = colorB;
                    }

                }
            }
            rect2.SetData(colorData);

            originGridItem[1] = rect2;
            */
            originGridItem[0] = kaninKitRail.getBoxTexture(graphics, gameManager.GRID_WIDTH, gameManager.GRID_HEIGHT, colorA, strokSize);
            originGridItem[1] = kaninKitRail.getBoxTexture(graphics, gameManager.GRID_WIDTH, gameManager.GRID_HEIGHT, colorB, strokSize);
            #endregion

            //add new grid
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    //grid item
                    Vector2 gridPosition = new Vector2(j * gameManager.GRID_WIDTH, i * gameManager.GRID_HEIGHT) + gameManager.GRID_STARTPOSITION;
                    int selector = (i + j) % 2;
                    gameManager.GRID_DATA[i, j] = new gridItem(originGridItem[selector], gridPosition, Color.White, gridType.Walkable);
                    //debug text
                    if (is_Debug)
                        debugText[i, j] = new text(font, Color.Black, gameManager.GRID_DATA[i, j].getCenterGridPosition);
                }
            }
        }

        public void Update(float animator_elapsed)
        {
            //nothing
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < gameManager.GRID_ROW; i++)
            {
                for (int j = 0; j < gameManager.GRID_COLUMN; j++)
                {
                    if (gameManager.GRID_DATA[i, j].type != gridType.Unwalkable)
                    {
                        gameManager.GRID_DATA[i, j].sprite.Draw(spriteBatch);
                        if (is_Debug)
                            debugText[i, j].drawFont(spriteBatch, j + ", " + i.ToString());
                    }
                }
            }
        }
    }
}
