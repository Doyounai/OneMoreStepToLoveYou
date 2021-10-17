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

        //star
        public static int star_1_step = 0;
        public static int star_2_step = 0;
        public static int star_3_step = 0;
        public static int currentLevel;
        public static int[] levelStar = new int[5];
        public static int sceneNumbrtToGO;

        public static int getCurrentStar { get { return levelStar[currentLevel - 1]; } }

        //racing game
        public static racingManager racingGameManager;
        public static playerRacing racingPlayer;

        //star system
        public static void updateStart()
        {
            int star = 3;

            if (playerStep >= star_2_step)
                star = 2;
            if (playerStep >= star_1_step)
                star = 1;

            if (levelStar[currentLevel - 1] < star)
                levelStar[currentLevel - 1] = star;

            //Console.WriteLine(playerStep);
            //Console.WriteLine(getCurrentStar);
        }

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
