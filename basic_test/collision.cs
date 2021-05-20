using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

class colision
{
    
    static public void f_slope_check(
        int till_rec_dif,
        int tilesize,
        double div_slope,
        int player_dem_neg,
        int player_dem_pos,
        int tilemap,
        ref int resistance,
        ref bool wallcheck,
        bool rescheck,
        int change)
    {
        if (till_rec_dif + tilesize >= div_slope - player_dem_neg
            && till_rec_dif <= div_slope + player_dem_pos)
        {
            if (tilemap != 1)
            {
                if (rescheck)
                {
                    resistance = change;
                    wallcheck = true;
                }
            }
        }
    }
    static public void f_slope_checkII(
        bool side_one,
        bool side_two,
        bool tilemapCheck,
        ref int resistance,
        ref bool wallcheck,
        bool rescheck,
        int change)
    {
        if (side_one
            && side_two)
        {
            if (tilemapCheck)
            {
                if (rescheck)
                {
                    resistance = change;
                    wallcheck = true;
                }
            }
        }
    }

    public void _collide(
                ref Rectangle player,
                int tile_size,
                int[,] mapOfTiles,
                ref int affected_varx,
                ref int affected_vary,
                bool[] side_touching_wall,
                int checkedNum)
    {
        Rectangle[] collision_detector =
        {
                new Rectangle(player.X - affected_varx, player.Y + affected_vary,  player.Width + affected_varx, player.Height - affected_vary),
                new Rectangle(player.X - affected_varx, player.Y, player.Width + affected_varx, player.Height + affected_vary),
                new Rectangle(player.X, player.Y + affected_vary, player.Width - affected_varx, player.Height - affected_vary),
                new Rectangle(player.X, player.Y,  player.Width - affected_varx, player.Height + affected_vary)
            };
        Rectangle relivant_rectangle = new Rectangle();
        int x_restricter = 0;
        int y_restricter = 0;
        bool isrelivant = false;
        bool isgoingleft = true;
        bool isgoingup = true;
        isrelivant = false;
        isgoingleft = true;
        isgoingup = true;
        #region direction
        if (affected_varx < 0)
        {
            isgoingleft = false;
        }
        if (affected_vary > 0)
        {
            isgoingup = false;
        }
        if (affected_vary != 0
            | affected_varx != 0)
        {
            if (isgoingleft == true)
            {
                if (isgoingup == true)
                {
                    isrelivant = true;
                    relivant_rectangle = collision_detector[0];
                    y_restricter = relivant_rectangle.Y;
                }
                else
                {
                    isrelivant = true;
                    relivant_rectangle = collision_detector[1];
                    y_restricter = relivant_rectangle.Y + relivant_rectangle.Height;
                }
                x_restricter = relivant_rectangle.X;
            }
            else
            {
                if (isgoingup == true)
                {
                    isrelivant = true;
                    relivant_rectangle = collision_detector[2];
                    y_restricter = relivant_rectangle.Y;
                }
                else
                {
                    isrelivant = true;
                    relivant_rectangle = collision_detector[3];
                    y_restricter = relivant_rectangle.Y + relivant_rectangle.Height;
                }
                x_restricter = relivant_rectangle.X + relivant_rectangle.Width;
            }
        }
        #endregion
        int left_tile = relivant_rectangle.Left / tile_size;
        int right_tile = relivant_rectangle.Right / tile_size;
        int top_tile = relivant_rectangle.Top / tile_size;
        int bottom_tile = relivant_rectangle.Bottom / tile_size;
        if (left_tile < 0 || right_tile > mapOfTiles.GetLength(1) ||
            top_tile < 0 || bottom_tile > mapOfTiles.GetLength(0))
        {
            isrelivant = false;
            affected_varx = 0;
            affected_vary = 0;
            player.X = 0;
            player.Y = 0;
        }
        if (isrelivant == true)
        {
            for (int b = 1; b <= 2; b++)
            {
                #region relivant tiles
                left_tile = relivant_rectangle.Left / tile_size;
                right_tile = (relivant_rectangle.Right - 1) / tile_size;
                top_tile = relivant_rectangle.Top / tile_size;
                bottom_tile = relivant_rectangle.Bottom / tile_size;

                if (left_tile < 0)
                {
                    left_tile = 0;
                }
                if (right_tile > mapOfTiles.GetLength(1))
                {
                    right_tile = mapOfTiles.GetLength(1);
                }
                if (top_tile < 0)
                {
                    top_tile = 0;
                }
                if (bottom_tile > mapOfTiles.GetLength(0))
                {
                    bottom_tile = mapOfTiles.GetLength(0);
                }


                #endregion
                double slope = 0;

                if (affected_varx != 0
                    && affected_vary != 0)
                {
                    slope = (0.0 - (double)affected_vary) / (double)affected_varx;
                }
                for (int y = top_tile; y <= bottom_tile; y++)
                {
                    for (int x = left_tile; x <= right_tile; x++)
                    {
                        if (mapOfTiles[y, x] == checkedNum)
                        {
                            Rectangle tile_check = new Rectangle(x * tile_size, y * tile_size, tile_size, tile_size);
                            if (isgoingleft == true)
                            {
                                #region upper left
                                if (isgoingup == true)
                                {
                                    if (tile_check.X + tile_size - relivant_rectangle.X <= affected_varx)
                                    {
                                        f_slope_checkII(
                                            tile_check.Y - relivant_rectangle.Y + tile_size >= (tile_check.X + tile_size - relivant_rectangle.X) * slope,
                                            tile_check.Y - relivant_rectangle.Y <= (tile_check.X + tile_size - relivant_rectangle.X) * slope + player.Height,
                                            mapOfTiles[y, x + 1] != checkedNum,
                                            ref x_restricter, ref side_touching_wall[0],
                                            x_restricter < tile_check.X + tile_size,
                                            tile_check.X + tile_size);
                                    }
                                    if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                    {
                                        if (affected_varx != 0
                                            && slope != 0)
                                        {
                                            f_slope_checkII(
                                                tile_check.X - relivant_rectangle.X + tile_size >= (tile_check.Y + tile_size - relivant_rectangle.Y) / slope,
                                                tile_check.X - relivant_rectangle.X <= (tile_check.Y + tile_size - relivant_rectangle.Y) / slope + player.Width,
                                                mapOfTiles[y + 1, x] != checkedNum,
                                                ref y_restricter, ref side_touching_wall[2],
                                                y_restricter < tile_check.Y + tile_size,
                                                tile_check.Y + tile_size);
                                        }
                                    }
                                    if (affected_varx == 0)
                                    {
                                        if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                        {
                                            if (mapOfTiles[y + 1, x] != checkedNum)
                                            {
                                                if (y_restricter < tile_check.Y + tile_size)
                                                {
                                                    y_restricter = tile_check.Y + tile_size;
                                                    side_touching_wall[2] = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region lower left
                                else
                                {
                                    if (tile_check.X + tile_size - relivant_rectangle.X <= affected_varx)
                                    {
                                        f_slope_checkII(
                                            tile_check.Y - relivant_rectangle.Y  + tile_size >= relivant_rectangle.Height + (tile_check.X + tile_size - relivant_rectangle.X) * slope - player.Height,
                                            tile_check.Y - relivant_rectangle.Y <= relivant_rectangle.Height + (tile_check.X + tile_size - relivant_rectangle.X) * slope,
                                            mapOfTiles[y, x + 1] != checkedNum,
                                            ref x_restricter, ref side_touching_wall[0],
                                            x_restricter < tile_check.X + tile_size,
                                            tile_check.X + tile_size);
                                    }

                                    if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                    {
                                        if (affected_varx != 0
                                            && slope != 0)
                                        {
                                            f_slope_checkII(
                                                tile_check.X - relivant_rectangle.X  + tile_size >= (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / -slope,
                                                tile_check.X - relivant_rectangle.X <= (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / -slope + player.Width,
                                                mapOfTiles[y - 1, x] != checkedNum,
                                                ref y_restricter, ref side_touching_wall[3],
                                                y_restricter > tile_check.Y,
                                                tile_check.Y);
                                        }
                                    }
                                    if (affected_varx == 0)
                                    {
                                        if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                        {
                                            if (mapOfTiles[y - 1, x] != checkedNum)
                                            {
                                                if (y_restricter > tile_check.Y)
                                                {
                                                    y_restricter = tile_check.Y;
                                                    side_touching_wall[3] = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region upper right
                                if (isgoingup == true)
                                {
                                    if (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X <= -affected_varx)
                                    {
                                        f_slope_checkII(
                                            tile_check.Y - relivant_rectangle.Y  + tile_size >= (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope,
                                            tile_check.Y - relivant_rectangle.Y <= (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope + player.Height,
                                            mapOfTiles[y, x - 1] != checkedNum,
                                            ref x_restricter, ref side_touching_wall[1],
                                            x_restricter > tile_check.X,
                                            tile_check.X);
                                    }
                                    if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                    {
                                        if (slope != 0)
                                        {
                                            f_slope_checkII(
                                                tile_check.X - relivant_rectangle.X  + tile_size >= relivant_rectangle.Width + (tile_check.Y + tile_size - relivant_rectangle.Y) / slope - player.Width,
                                                tile_check.X - relivant_rectangle.X <= relivant_rectangle.Width + (tile_check.Y + tile_size - relivant_rectangle.Y) / slope,
                                                mapOfTiles[y + 1, x] != checkedNum,
                                                ref y_restricter, ref side_touching_wall[2],
                                                y_restricter < tile_check.Y + tile_size, tile_check.Y + tile_size);
                                        }
                                    }
                                }
                                #endregion
                                #region lower right
                                else
                                {
                                    if (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X <= -affected_varx)
                                    {
                                        f_slope_checkII(
                                            tile_check.Y - relivant_rectangle.Y  + tile_size >= (relivant_rectangle.Height - (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope) - player.Height,
                                            tile_check.Y - relivant_rectangle.Y <= (relivant_rectangle.Height - (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope) ,
                                            mapOfTiles[y, x - 1] != checkedNum,
                                            ref x_restricter, ref side_touching_wall[1],
                                            x_restricter > tile_check.X,
                                            tile_check.X);
                                    }
                                    if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                    {
                                        if (slope != 0)
                                        {
                                            f_slope_checkII(
                                                tile_check.X - relivant_rectangle.X >= relivant_rectangle.Width - (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / slope - player.Width,
                                                tile_check.X - relivant_rectangle.X <= relivant_rectangle.Width - (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / slope,
                                                mapOfTiles[y - 1, x] != checkedNum,
                                                ref y_restricter, ref side_touching_wall[3],
                                                y_restricter > tile_check.Y,
                                                tile_check.Y);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                #region rectangle change
                if (true)
                {
                    if (isgoingleft == true)
                    {
                        relivant_rectangle.Width = relivant_rectangle.X + relivant_rectangle.Width - x_restricter;
                        relivant_rectangle.X = x_restricter;
                    }
                    else
                    {
                        relivant_rectangle.Width = x_restricter - relivant_rectangle.X;
                    }
                    if (isgoingup == true)
                    {
                        relivant_rectangle.Height = relivant_rectangle.Y + relivant_rectangle.Height - y_restricter;
                        relivant_rectangle.Y = y_restricter;
                    }
                    else
                    {
                        relivant_rectangle.Height = y_restricter - relivant_rectangle.Y;
                    }
                }
                #endregion
            }
            #region var change
            if (isgoingleft == true)
            {
                if (x_restricter != 0)
                {
                    affected_varx = relivant_rectangle.Width - player.Width;
                }
                if (isgoingup == true
                    && y_restricter != 0)
                {
                    affected_vary = -relivant_rectangle.Height + player.Height;
                }
                else if (y_restricter != 0)
                {
                    affected_vary = relivant_rectangle.Height - player.Height;
                }
            }
            else
            {
                if (x_restricter != 0)
                {
                    affected_varx = -relivant_rectangle.Width + player.Width;
                }
                if (isgoingup == true
                    && y_restricter != 0)
                {
                    affected_vary = -relivant_rectangle.Height + player.Height;
                }
                else if (y_restricter != 0)
                {
                    affected_vary = relivant_rectangle.Height - player.Height;
                }
            }
            #endregion
        }
    }
}