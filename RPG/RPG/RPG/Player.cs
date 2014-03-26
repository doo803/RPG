using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RPG
{
    public class Player
    {
        public int posX, posY;
        int moveUp, moveDown, moveRight, moveLeft, moveReset, moveInstantReset, boolResetTimer;
        public bool movingUp, movingDown, movingRight, movingLeft;

        // Constructor
        public Player(int newPosX, int newPosY)
        {
            posX = newPosX;
            posY = newPosY;
            moveReset = 30; // 30 ticks, 0.5 seconds
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        { 
        
        }

        // Update
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Move player
            #region
            // Deal with move timers and begin moving booleans
            #region
            if (keyState.IsKeyDown(Keys.Up))
            {
                moveUp++;
                if(moveUp >= moveReset)
                {
                    moveUp = 0;
                    movingUp = true;
                    moveInstantReset = 1;
                }
            }

            else if (keyState.IsKeyDown(Keys.Down))
            {
                moveDown++;
                if (moveDown >= moveReset)
                {
                    moveDown = 0;
                    movingDown = true;
                    moveInstantReset = 1;
                }
            }

            else if (keyState.IsKeyDown(Keys.Left))
            {
                moveLeft++;
                if (moveLeft >= moveReset)
                {
                    moveLeft = 0;
                    movingLeft = true;
                    moveInstantReset = 1;
                }
            }

            else if (keyState.IsKeyDown(Keys.Right))
            {
                moveRight++;
                if (moveRight >= moveReset)
                {
                    moveRight = 0;
                    movingRight = true;
                    moveInstantReset = 1;
                }
            }
            #endregion

            // Reset moving booleans
            #region
            if (movingUp || movingRight || movingDown || movingLeft)
            {
                boolResetTimer++;
                if(boolResetTimer == 2)
                {
                    movingLeft = false;
                    movingDown = false;
                    movingRight = false;
                    movingUp = false;
                    boolResetTimer = 0;
                }
            }
            #endregion

            // Allow for instant inital moving
            #region
            if (moveInstantReset >= 1)
            {
                moveInstantReset++;
            }

            if(keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down) && keyState.IsKeyUp(Keys.Left) && keyState.IsKeyUp(Keys.Right) && moveInstantReset >= 30)
            {
                moveUp = moveReset - 1;
                moveRight = moveReset - 1;
                moveDown = moveReset - 1;
                moveLeft = moveReset - 1;
                moveInstantReset = 0;
            }
            #endregion
            #endregion
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        { 
        
        }
    }
}
