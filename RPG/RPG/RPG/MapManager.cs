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

        int[,] mapArray, playerArray;
        int collideMapToggle = 0;
        Texture2D canPass, notPass, playerTile, currentTextureMap;
        bool showCollideMap, showPlayerMap;

        // Constructor
        public MapManager()
        {
            showCollideMap = false;
            playerArray = new int[18, 32];
            player = new Player(10, 10);
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            canPass = Content.Load<Texture2D>("map/tile");
            notPass = Content.Load<Texture2D>("map/!tile");
            playerTile = Content.Load<Texture2D>("map/playerTile");
            stage1.LoadContent(Content);
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
            
            // Update player's position on current mapArray
            UpdatePlayerPosToArray(playerArray);

            // Check for player movement and if the move is valid
            PlayerMovement();
            
        }

        // DRAW METHOD
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawMapTexture(spriteBatch);
            DrawCollisionGrid(spriteBatch);
            DrawPlayerOnArray(spriteBatch);
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
                            spriteBatch.Draw(canPass, new Rectangle(j * 32, i * 32, 32, 32), Color.White * 0.5f);
                        }

                        if (mapArray[i, j] == 1)
                        {
                            spriteBatch.Draw(notPass, new Rectangle(j * 32, i * 32, 32, 32), Color.White * 0.5f);
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
            if(player.movingUp)
            {
                if(mapArray[player.posY - 1,player.posX] == 0)
                {
                    playerArray[player.posY, player.posX] = 0;
                    player.posY--;
                }
            }

            else if(player.movingDown)
            {
                if (mapArray[player.posY + 1, player.posX] == 0)
                {
                    playerArray[player.posY, player.posX] = 0;
                    player.posY++;
                }
            }

            else if(player.movingLeft)
            {
                if (mapArray[player.posY, player.posX - 1] == 0)
                {
                    playerArray[player.posY, player.posX] = 0;
                    player.posX--;
                }
            }

            else if (player.movingRight)
            {
                if (mapArray[player.posY, player.posX + 1] == 0)
                {
                    playerArray[player.posY, player.posX] = 0;
                    player.posX++;
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
    }
}
