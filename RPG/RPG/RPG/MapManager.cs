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
        Texture2D canPass, notPass, playerTile;
        bool showCollideMap, showPlayerMap;

        // Constructor
        public MapManager()
        {
            showCollideMap = false;
            playerArray = new int[18, 32];
            player = new Player(5, 5);
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            canPass = Content.Load<Texture2D>("map/tile");
            notPass = Content.Load<Texture2D>("map/!tile");
            playerTile = Content.Load<Texture2D>("map/playerTile");
        }

        // Update
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            stage1.Update(gameTime);
            mapArray = stage1.currentMap;

            // Show / hide collision grid
            ToggleGrids();
            
            // Update player's position on current mapArray
            UpdatePlayerPos(playerArray);
            
        }

        // DRAW METHOD
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCollisionGrid(spriteBatch);
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
                    showPlayerMap = true;
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

        // Update player position
        public void UpdatePlayerPos(int[,] map)
        {
            #region
            map[player.posY, player.posX] = 5;
            #endregion
        }
    }
}
