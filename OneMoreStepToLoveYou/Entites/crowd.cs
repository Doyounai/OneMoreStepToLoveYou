﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.Entites
{
    public class crowd : character, I_gameInterface
    {
        public int DrawOrder { get; set; }
        public int m_moveStep;
        private int crowdMoveStep = 4;
        public crowd(Texture2D texture, gridPosition gridPos)
        {
            this.type = gridType.Crowd;
            sprite = new Sprite(texture, Vector2.Zero, Color.White);
            m_gridPosition = gridPos;
            gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].type = gridType.Crowd;
            sprite.position = gameManager.GRID_DATA[m_gridPosition.row, m_gridPosition.column].getCenterGridPosition;
            sprite.position -= kaninKitRail.getCenterPoint(sprite.gameSprite.Width, sprite.gameSprite.Height);

            gameManager.crowds.Add(m_gridPosition, this);
        }

        public void Update()
        {
            updatePosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public override void changePosition(gridPosition pos)
        {

            gameManager.crowds.Remove(m_gridPosition);
            base.changePosition(pos);
            gameManager.crowds.Add(pos, this);
        }

        public gridType getNextGridType(gridPosition nextPos)
        {
            if (nextPos.column < 0 || nextPos.column > gameManager.GRID_COLUMN - 1 || nextPos.row < 0 || nextPos.row > gameManager.GRID_ROW - 1)
                return gridType.Unwalkable;

            if (gameManager.GRID_DATA[nextPos.row, nextPos.column].type == gridType.Player)
                return gridType.Unwalkable;

            return gameManager.GRID_DATA[nextPos.row, nextPos.column].type;
        }
    }
}