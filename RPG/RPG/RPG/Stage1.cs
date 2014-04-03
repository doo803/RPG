using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RPG
{
    public class Stage1
    {
        string mapSelect = "1_1";
        public int[,] currentMap;
        public Texture2D currentTextureMap;
        public int playerX, playerY, changePlayerX, changePlayerY, i, changeMapX, changeMapY, changeMapTo;
        Texture2D texture1_1, texture1_1_tent1, texture1_1_tent2,
            texture1_2; 

        // Maps
        #region
        // Region 1_1
        #region
        int[,] map1_1 = ReadIn(@"maps/stage1/map_1_1");

        int[,] map_tent1_1_1 = ReadIn(@"maps/stage1/map_tent1_1_1");

        int[,] map_tent2_1_1 = ReadIn(@"maps/stage1/map_tent2_1_1");
        
        #endregion

        // Region 1_2
        #region
        int[,] map1_2 = ReadIn(@"maps/stage1/map_1_2");

        #endregion

        #endregion

        // Constructor
        public Stage1()
        {
            changePlayerX = -1;
            changePlayerY = -1;
            changeMapX = -1;
            changeMapY = -1;
            changeMapTo = 0;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            texture1_1 = Content.Load<Texture2D>("map/stage1/Stage1_1");
            texture1_1_tent1 = Content.Load<Texture2D>("map/stage1/1_1_tent1");
            texture1_1_tent2 = Content.Load<Texture2D>("map/stage1/1_1_tent2");
            texture1_2 = Content.Load<Texture2D>("map/stage1/Stage1_2");
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            // Allow map to be edited
            //ChangeMap();

            switch (mapSelect)
            {
                // Region 1_1
                #region
                case "1_1":
                    #region
                    {
                        currentTextureMap = texture1_1;
                        currentMap = map1_1;
                        if (currentMap[playerY, playerX] == 2)
                        {
                            mapSelect = "1_1_tent1";
                            changePlayerX = 15;
                            changePlayerY = 11;
                        }

                        else if (currentMap[playerY, playerX] == 3)
                        {
                            mapSelect = "1_1_tent2";
                            changePlayerX = 15;
                            changePlayerY = 11;
                        }

                        else if (currentMap[playerY, playerX] == 4)
                        {
                            mapSelect = "1_2";
                            changePlayerX = 15;
                            changePlayerY = 3;
                        }
                        break;
                    }
                    #endregion

                case "1_1_tent1":
                    #region
                    {
                        currentTextureMap = texture1_1_tent1;
                        currentMap = map_tent1_1_1;
                        if (currentMap[playerY, playerX] == 2)
                        {
                            mapSelect = "1_1";
                            changePlayerX = 10;
                            changePlayerY = 5;
                        }
                        break;
                    }
                    #endregion

                case "1_1_tent2":
                    #region
                    {
                        currentTextureMap = texture1_1_tent2;
                        currentMap = map_tent2_1_1;
                        if (currentMap[playerY, playerX] == 2)
                        {
                            mapSelect = "1_1";
                            changePlayerX = 21;
                            changePlayerY = 5;
                        }
                        break;    
                    }
                    #endregion
                #endregion

                // Region 1_2
                #region
                case "1_2":
                    {
                        currentMap = map1_2;
                        currentTextureMap = texture1_2;
                        if (currentMap[playerY, playerX] == 2)
                        {
                            mapSelect = "1_1";
                            changePlayerX = 16;
                            changePlayerY = 16;
                        }
                        break;
                    }
                #endregion

            }
        }

        static int[,] ReadIn(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            int width;
            int height;
            int.TryParse(reader.ReadLine(), out width);
            int.TryParse(reader.ReadLine(), out height);
            int[,] readValues = new int[width, height];
            for (int i = 0; i < readValues.GetLength(0); i++)
            {
                for (int j = 0; j < readValues.GetLength(1); j++)
                {
                    int.TryParse(reader.ReadLine(), out readValues[i, j]);
                }
            }
            reader.Close();
            return readValues;
        }

        public void WriteOut(int[,] testValues, string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.WriteLine(testValues.GetLength(0));
            writer.WriteLine(testValues.GetLength(1));
            for (int i = 0; i < testValues.GetLength(0); i++)
            {
                for (int j = 0; j < testValues.GetLength(1); j++)
                {
                    writer.WriteLine(testValues[i, j]);
                }
            }
            writer.Close();
        }

        // Change map array from edit
        public void ChangeMap()
        {
            if (changeMapX != -1 || changeMapY != -1 && changeMapTo != 0)
            {
                if ((changeMapTo == -1 && currentMap[changeMapY, changeMapX] != 0) ||
                    changeMapTo == 1)
                {
                    int[,] changedMap = currentMap;
                    changedMap[changeMapY, changeMapX] += changeMapTo;
                    string saveLocation = String.Format("maps/stage1/map_{0}", mapSelect);
                    WriteOut(changedMap, saveLocation);
                    changeMapTo = 0;
                    changeMapX = -1;
                    changeMapY = -1;                 
                    currentMap = ReadIn(saveLocation);
                }
                
            }
        }
    }
}
