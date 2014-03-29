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
        public bool movingUp, movingDown, movingRight, movingLeft, movingUpCheck, movingDownCheck, movingRightCheck, movingLeftCheck;
        bool facingUp, facingDown, facingRight, facingLeft;

        // Animations
        Texture2D texture;
        Rectangle sourceRect;
        Vector2 animationPosition;
        int currentFrame, spriteHeight, spriteWidth, animationStart, totalFrames;

        // Constructor
        public Player(int newPosX, int newPosY)
        {
            posX = newPosX;
            posY = newPosY;

            facingDown = true;
            totalFrames = 16;

            moveDown = moveReset - 1;
            moveLeft = moveReset - 1;
            moveRight = moveReset - 1;
            moveUp = moveReset - 1;

            spriteHeight = 31;
            spriteWidth = 32;
            
            moveReset = 16; // 33 ticks

            animationPosition = new Vector2((posX * 32), ((posY) * 32) + (currentFrame * 2));
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        { 
            // Animation textures
            texture = Content.Load<Texture2D>("map/player/playerTexture");
        }

        // Update
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Move player
            #region
            // Deal with move timers and begin moving booleans
            #region
            if (keyState.IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
            {
                moveUp++;
                if(moveUp >= moveReset)
                {
                    ResetToZero();
                    movingUpCheck = true;
                    facingUp = true;
                    facingRight = false;
                    facingLeft = false;
                    facingDown = false;
                    moveInstantReset = 1;
                }
            }

            else if (keyState.IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
            {
                moveDown++;
                if (moveDown >= moveReset)
                {
                    ResetToZero();
                    movingDownCheck = true;
                    facingUp = false;
                    facingRight = false;
                    facingLeft = false;
                    facingDown = true;
                    moveInstantReset = 1;
                }
            }

            else if (keyState.IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
            {
                moveLeft++;
                if (moveLeft >= moveReset)
                {
                    ResetToZero();
                    movingLeftCheck = true;
                    facingUp = false;
                    facingRight = false;
                    facingLeft = true;
                    facingDown = false;
                    moveInstantReset = 1;
                }
            }

            else if (keyState.IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
            {
                moveRight++;
                if (moveRight >= moveReset)
                {
                    ResetToZero();
                    movingRightCheck = true;
                    facingUp = false;
                    facingRight = true;
                    facingLeft = false;
                    facingDown = false;
                    moveInstantReset = 1;
                }
            }
            #endregion

            // Allow for 'Run' mode (Halves reset timer)
            #region
            //if (keyState.IsKeyDown(Keys.X))
            //{
            //    moveReset = 15;
            //}

            //else if (keyState.IsKeyUp(Keys.X))
            //{
            //    moveReset = 30;
            //}
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

            PlayerMoveAnimations(gameTime);
            #endregion
        }

        // Reset move timers to 0
        public void ResetToZero()
        {
            #region
            moveDown = 0;
            moveLeft = 0;
            moveRight = 0;
            moveUp = 0;
            #endregion
        }

        // Animation Logic
        public void PlayerMoveAnimations(GameTime gameTime)
        {
            #region 
            // Downward facing
            #region 
            if (facingDown)
            {
                if (!movingDown && animationStart != 1)
                {
                    sourceRect = new Rectangle(32, 0, spriteWidth, spriteHeight);
                }
                
                if (movingDown)
                {
                    animationStart = 1;
                }

                if (animationStart == 1)
                {
                    currentFrame++;
                    if (currentFrame <= (int)totalFrames/3)
                    {
                        sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 5 && currentFrame <= (int)totalFrames/1.5)
                    {
                        sourceRect = new Rectangle(32, 0, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 10 && currentFrame <= totalFrames)
                    {
                        sourceRect = new Rectangle(64, 0, spriteWidth, spriteHeight);
                    }

                    if (currentFrame == 16)
                    {
                        animationStart = 0;
                        currentFrame = 0;
                    }
                }

                if (animationStart == 1)
                {
                    animationPosition = new Vector2((posX * 32), ((posY - 1) * 32) + (currentFrame * 2));
                }

            }
            #endregion
            // Upward facing
            #region
            if (facingUp)
            {
                if (!movingUp && animationStart != 1)
                {
                    sourceRect = new Rectangle(32, 96, spriteWidth, spriteHeight);
                }

                if (movingUp)
                {
                    animationStart = 1;
                }

                if (animationStart == 1)
                {
                    currentFrame++;
                    if (currentFrame <= 5)
                    {
                        sourceRect = new Rectangle(0, 96, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 5 && currentFrame <= 10)
                    {
                        sourceRect = new Rectangle(32, 96, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 10 && currentFrame <= 16)
                    {
                        sourceRect = new Rectangle(64, 96, spriteWidth, spriteHeight);
                    }

                    if (currentFrame == 16)
                    {
                        animationStart = 0;
                        currentFrame = 0;
                    }
                }

                if (animationStart == 1)
                {
                    animationPosition = new Vector2((posX * 32), ((posY + 1) * 32) - (currentFrame * 2));
                }

            }
            #endregion
            // Left facing
            #region
            if (facingLeft)
            {
                if (!movingLeft && animationStart != 1)
                {
                    sourceRect = new Rectangle(32, 32, spriteWidth, spriteHeight);
                }

                if (movingLeft)
                {
                    animationStart = 1;
                }

                if (animationStart == 1)
                {
                    currentFrame++;
                    if (currentFrame <= 5)
                    {
                        sourceRect = new Rectangle(0, 32, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 5 && currentFrame <= 10)
                    {
                        sourceRect = new Rectangle(32, 32, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 10 && currentFrame <= 16)
                    {
                        sourceRect = new Rectangle(64, 32, spriteWidth, spriteHeight);
                    }

                    if (currentFrame == 16)
                    {
                        animationStart = 0;
                        currentFrame = 0;
                    }
                }

                if (animationStart == 1)
                {
                    animationPosition = new Vector2(((posX + 1) * 32 - (currentFrame * 2)), ((posY) * 32));
                }

            }
            #endregion
            // Right facing
            #region
            if (facingRight)
            {
                if (!movingRight && animationStart != 1)
                {
                    sourceRect = new Rectangle(32, 64, spriteWidth, spriteHeight);
                }

                if (movingRight)
                {
                    animationStart = 1;
                }

                if (animationStart == 1)
                {
                    currentFrame++;
                    if (currentFrame <= 5)
                    {
                        sourceRect = new Rectangle(0, 64, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 5 && currentFrame <= 10)
                    {
                        sourceRect = new Rectangle(32, 64, spriteWidth, spriteHeight);
                    }

                    else if (currentFrame > 10 && currentFrame <= 16)
                    {
                        sourceRect = new Rectangle(64, 64, spriteWidth, spriteHeight);
                    }

                    if (currentFrame == 16)
                    {
                        animationStart = 0;
                        currentFrame = 0;
                    }
                }

                if (animationStart == 1)
                {
                    animationPosition = new Vector2(((posX - 1) * 32) + (currentFrame * 2), ((posY) * 32));
                }

            }
            #endregion
            #endregion
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, animationPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
