using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.Entites
{
    #region grid
    public struct gridPosition
    {
        public int column, row;

        public gridPosition(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public gridPosition up
        {
            get{ return new gridPosition(this.column, this.row - 1); }
        }

        public gridPosition down
        {
            get
            {
                return new gridPosition(this.column, this.row + 1);
            }
        }

        public gridPosition left
        {
            get
            {
                return new gridPosition(this.column - 1, this.row);
            }
        }

        public gridPosition right
        {
            get
            {
                return new gridPosition(this.column + 1, this.row);
            }
        }
    }
    public enum gridType
    {
        Player,
        Card,
        Crowd,
        Walkable,
        Unwalkable
    }
    #endregion
    public static class gameManager
    {
        public static bool is_PAUSE = false;
        public static ContentManager content;

        //grid
        public static Dictionary<gridPosition, crowd> crowds = new Dictionary<gridPosition, crowd>();
        public static Vector2 GRID_STARTPOSITION;
        public static gridItem[,] GRID_DATA;
        public static int GRID_WIDTH;
        public static int GRID_HEIGHT;
        public static int GRID_COLUMN;
        public static int GRID_ROW;

        //player
        public static int playerStep = 0;//crowd use for check to walk back to origin position
        public static yaDov ya;
        public static player M_PLAYER;
        public static yaDov ssr;

        //pEarth
        public static gridPosition pEarthPosition;

        //call when player move
        public static void playerMove()
        {
            var temp = crowds.Values.ToArray();
            foreach (var item in temp)
            {
                item.goBack();
            }
            playerStep += 1;
        }

        public static void addShadowArea(int j, int i)
        {
            GRID_DATA[i, j].type = gridType.Unwalkable;
        }
    }
}
