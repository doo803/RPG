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
    public class MapManager
    {
        Stage1 stage1 = new Stage1();
        Player player;
        SpriteFont tileFont;

        int[,] mapArray, playerArray;
        int collideMapToggle = 0;
        int x = 0;
        int currentStage;
        Texture2D canPass, notPass, changeTile, playerTile, currentTextureMap;
        public bool showCollideMap, showPlayerMap, editCollideMap, mouseInWindow;
        int b = 0;
        int e = 0;

        // Constructor
        public MapManager()
        {
            showCollideMap = false;
            playerArray = new int[18, 32];
            player = new Player(10, 10);
            currentStage = 1;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            canPass = Content.Load<Texture2D>("map/tile");
            notPass = Content.Load<Texture2D>("map/!tile");
            changeTile = Content.Load<Texture2D>("map/transitionTile");
            playerTile = Content.Load<Texture2D>("map/playerTile");
            stage1.LoadContent(Content);
            player.LoadContent(Content);
            tileFont = Content.Load<SpriteFont>("TileFont");
        }

        // Update
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            stage1.Update(gameTime);
            player.Update(gameTime);

            mapArray = stage1.currentMap;
            currentTextureMap = stage1.currentTextureMap;

            // Show / hide collision grid
            ToggleGrids();

            // Allow editing of grids
            CollisionEditor();
            stage1.ChangeMap();
            
            // Update player's position on current mapArray
            UpdatePlayerPosToArray(playerArray);

            // Check for player movement and if the move is valid
            PlayerMovement();

            // Update player coords on Stage1
            stage1.playerX = player.posX;
            stage1.playerY = player.posY;

            if (stage1.changePlayerX != -1 && stage1.changePlayerY != -1)
            {
                playerArray[player.posY, player.posX] = 0;
                player.posX = stage1.changePlayerX;
                player.posY = stage1.changePlayerY;
                stage1.changePlayerY = -1;
                stage1.changePlayerX = -1;
            }
        }

        // DRAW METHOD
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawMapTexture(spriteBatch);
            DrawCollisionGrid(spriteBatch);
            DrawPlayerOnArray(spriteBatch);
            player.Draw(spriteBatch);

            // Show edit mode is active
            #region
            if (editCollideMap)
            {
                spriteBatch.DrawString(tileFont, "*Editing mode active*", new Vector2(10, 10), Color.White * 0.8f);
            }
            #endregion
        }

        // Toggle collision and player grid
        public void ToggleGrids()
        {
            #region
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Tab))
            {
                collideMapToggle++;
                if (collideMapToggle == 1 && showCollideMap == false)
                {
                    showCollideMap = true;
                    showPlayerMap = true;
                }

                else if (collideMapToggle == 1 && showCollideMap == true)
                {
                    showCollideMap = false;
                    showPlayerMap = false;
                }
            }

            else if (keyState.IsKeyUp(Keys.Tab))
            {
                collideMapToggle = 0;
            }
            #endregion
        }

        // Draw collision grid
        public void DrawCollisionGrid(SpriteBatch spriteBatch)
        {            
            #region
            // Draw map 
            if (showCollideMap == true)
            {
                for (int i = 0; i < 18; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (mapArray[i, j] == 0)
                        {
                            spriteBatch.Draw(canPass, new Rectangle(j * 32, i * 32, 32, 32), Color.White * 0.2f);
                        }

                        if (mapArray[i, j] == 1)
                        {
                            spriteBatch.Draw(notPass, new Rectangle(j * 32, i * 32, 32, 32), Color.White * 0.5f);
                        }

                        if (mapArray[i, j] >= 2)
                        {
                            spriteBatch.Draw(changeTile, new Rectangle(j * 32, i * 32, 32, 32), Color.White * 0.5f);
                            spriteBatch.DrawString(tileFont, mapArray[i, j].ToString(), 
                                new Vector2((j * 32) + 10, (i * 32) + 5), Color.White);
                        }
                    }
                }
            }
            #endregion
        }

        // Draw player on array
        public void DrawPlayerOnArray(SpriteBatch spriteBatch)
        {
            #region
            // Draw map 
            if (showPlayerMap == true)
            {
                for (int i = 0; i < 18; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (playerArray[i, j] == 1)
                        {
                            spriteBatch.Draw(playerTile, new Rectangle(j * 32, i * 32, 32, 32), Color.White * 0.5f);
                        }
                    }
                }
            }
            #endregion
        }

        // Update player position
        public void UpdatePlayerPosToArray(int[,] map)
        {
            #region
            map[player.posY, player.posX] = 1;
            #endregion
        }

        // Check for player movement
        public void PlayerMovement()
        {
            #region
            if(player.movingUpCheck)
            {
                if (mapArray[player.posY - 1, player.posX] != 1)
                {
                    player.movingUpCheck = false;
                    player.movingUp = true;
                    playerArray[player.posY, player.posX] = 0;
                    player.posY--;
                }

                else
                {
                    player.movingUpCheck = false;
                }
            }

            else if(player.movingDownCheck)
            {
                if (mapArray[player.posY + 1, player.posX] != 1)
                {
                    player.movingDownCheck = false;
                    player.movingDown = true;
                    playerArray[player.posY, player.posX] = 0;
                    player.posY++;
                }

                else
                {
                    player.movingDownCheck = false;
                }
            }

            else if(player.movingLeftCheck)
            {
                if (mapArray[player.posY, player.posX - 1] != 1)
                {
                    player.movingLeftCheck = false;
                    player.movingLeft = true;
                    playerArray[player.posY, player.posX] = 0;
                    player.posX--;
                }

                else 
                {
                    player.movingLeftCheck = false;
                }
            }

            else if (player.movingRightCheck)
            {
                if (mapArray[player.posY, player.posX + 1] != 1)
                {
                    player.movingRightCheck = false;
                    player.movingRight = true;
                    playerArray[player.posY, player.posX] = 0;
                    player.posX++;
                }

                else
                {
                    player.movingRightCheck = false;
                }
            }
            #endregion
        }

        // Draw the actual texture
        public void DrawMapTexture(SpriteBatch spriteBatch)
        {
            #region
            spriteBatch.Draw(currentTextureMap, Vector2.Zero, Color.White);
            #endregion          
        }

        // Change collision with mouse
        public void CollisionEditor()
        {
            #region
            KeyboardState keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            int z = 0;
            int y = 0;

            if (keyState.IsKeyDown(Keys.OemTilde))
            {
                z = 1;
            }

            if (keyState.IsKeyUp(Keys.OemTilde))
            {
                x = 0;
            }

            if (z != y)
            {
                x++;
            }

            if (x == 1 && editCollideMap == false)
            {
                editCollideMap = true;
            }

            else if (x == 1 && editCollideMap == true)
            {
                editCollideMap = false;
            }

            if (editCollideMap == true)
            {
                int a = 0;               
                int c = 0;

                if (mouseState.LeftButton == ButtonState.Pressed && mouseInWindow)
                {
                    c = 1;
                }

                if (mouseState.LeftButton == ButtonState.Released)
                {
                    b = 0;
                }

                if (c != a)
                {
                    b++;
                }

                if (b == 1)
                {
                    if (currentStage == 1)
                    {
                        stage1.changeMapX = (int)(mousePosition.X / 32);
                        stage1.changeMapY = (int)(mousePosition.Y / 32);
                        stage1.changeMapTo = 1;
                    }
                }

                int d = 0;
                int f = 0;

                if (mouseState.RightButton == ButtonState.Pressed && mouseInWindow)
                {
                    d = 1;
                }

                if (mouseState.RightButton == ButtonState.Released)
                {
                    e = 0;
                }

                if (d != f)
                {
                    e++;
                }

                if (e == 1)
                {
                    if (currentStage == 1)
                    {
                        stage1.changeMapX = (int)(mousePosition.X / 32);
                        stage1.changeMapY = (int)(mousePosition.Y / 32);
                        stage1.changeMapTo = -1;
                    }
                }
            }
            #endregion
        }
    }
}
