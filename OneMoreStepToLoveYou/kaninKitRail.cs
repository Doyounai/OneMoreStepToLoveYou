using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.Entites;

namespace OneMoreStepToLoveYou
{
    public class kaninKitRail
    {
        public static Vector2 getCenterPoint(Vector2 position, float width, float height)
        {
            return new Vector2(width / 2, height / 2) + position;
        }

        public static Vector2 getCenterPoint(int width, int height)
        {
            return new Vector2(width / 2, height / 2);
        }

        public static Vector2 convertGridPosToVectorPos(gridPosition pos)
        {
            return new Vector2(pos.column * gameManager.GRID_WIDTH + gameManager.GRID_STARTPOSITION.X, pos.row * gameManager.GRID_HEIGHT + gameManager.GRID_STARTPOSITION.Y);
        }

        public static Texture2D getBoxTexture(GraphicsDeviceManager graphics, int widht, int height, Color colorA, int strokSize, Color strokColor)
        {
            Texture2D[] originGridItem = new Texture2D[2];

            Texture2D rect = new Texture2D(graphics.GraphicsDevice, widht, height);
            Color[] colorData = new Color[widht * height];

            //color A
            //row
            for (int i = 0; i < height; i++)
            {
                //column
                for (int j = 0; j < widht; j++)
                {
                    //boarder
                    if (i <= strokSize || i >= height - strokSize || j <= strokSize || j >= widht - strokSize)
                    {
                        colorData[(widht * i) + j] = strokColor;
                    }//fill
                    else
                    {
                        colorData[(widht * i) + j] = colorA;
                    }

                }
            }
            rect.SetData(colorData);

            return rect;
        }

        public static Texture2D getBoxTexture(GraphicsDeviceManager graphics, int widht, int height,Color colorA, int strokSize)
        {
            Texture2D[] originGridItem = new Texture2D[2];

            Texture2D rect = new Texture2D(graphics.GraphicsDevice, widht, height);
            Color[] colorData = new Color[widht * height];

            //color A
            //row
            for (int i = 0; i < height; i++)
            {
                //column
                for (int j = 0; j < widht; j++)
                {
                    //boarder
                    if (i <= strokSize || i >= height - strokSize || j <= strokSize || j >= widht - strokSize)
                    {
                        colorData[(widht * i) + j] = new Color(255, 255, 255, 1);
                    }//fill
                    else
                    {
                        colorData[(widht * i) + j] = colorA;
                    }

                }
            }
            rect.SetData(colorData);

            return rect;
        }
    }
}
