using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using System.IO;

public class save : Game
{
    static public void f_fill(ref List<List<int>> list, int size_x, int size_y)
    {
        if (list.Capacity < size_y)
            list.Capacity = size_y;
        for (int i = list.Count; i < list.Capacity; i++)
        {
            list.Add(new List<int>());
        }
        for (int y = 0; y < size_y; y++)
        {
            if (list[y].Capacity < size_x)
                list[y].Capacity = size_x;
            for (int i = list[y].Count; i < list[y].Capacity; i++)
            {
                list[y].Add(0);
            }
        }
    }
    #region int array
    public void f_mapsave(
        int[,] map,
        string file)
    {

        string filecontent = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                filecontent = filecontent + map[y, x];
                if (x != map.GetLength(1) - 1)
                    filecontent = filecontent + " ";
            }
            if (y != map.GetLength(0) - 1)
                filecontent = filecontent + ",\n";
        }
        File.WriteAllText(file, filecontent);
    }
    public void f_mapget(
        string file,
        ref int[,] map)
    {
        string filecontent = File.ReadAllText(file);
        string[] split = filecontent.Split(',');
        string[] yAxisValue = split[0].Split(' ');
        map = new int[split.GetLength(0), yAxisValue.GetLength(0)];
        for (int y = 0; y < split.GetLength(0); y++)
        {
            yAxisValue = split[y].Split(' ');
            for (int x = 0; x < yAxisValue.GetLength(0); x++)
            {
                map[y, x] = int.Parse(yAxisValue[x]);
            }
        }
    }
    #endregion
    #region list array
    public void f_listsave(
        List<List<int>> map,
        string file)
    {

        string filecontent = "";
        for (int y = 0; y < map.Capacity; y++)
        {
            for (int x = 0; x < map[0].Capacity; x++)
            {
                filecontent = filecontent + map[y][x];
                if (x != map[0].Capacity - 1)
                    filecontent = filecontent + " ";
            }
            if (y != map.Capacity - 1)
                filecontent = filecontent + ",\n";
        }
        File.WriteAllText(file, filecontent);
    }
    public void f_listget(
        string file,
        ref List<List<int>> map,
        ref int sizeX,
        ref int sizeY)
    {
        string filecontent = File.ReadAllText(file);
        string[] split = filecontent.Split(',');
        string[] yAxisValue = split[0].Split(' ');
        sizeX = yAxisValue.GetLength(0);
        sizeY = split.GetLength(0);
        f_fill(ref map, sizeX, sizeY);
        for (int y = 0; y < split.GetLength(0); y++)
        {
            yAxisValue = split[y].Split(' ');
            for (int x = 0; x < yAxisValue.GetLength(0); x++)
            {
                map[y][x] = int.Parse(yAxisValue[x]);
            }
        }
    }
    #endregion
    #region rectangle
    public void f_recsave(
        List<Rectangle> recs,
        string file)
    {

        string filecontent = "";
        for (int t = 0; t < recs.Count; t++)
        {
            filecontent = filecontent + recs[t].X + " " + recs[t].Y + " " + recs[t].Width + " " + recs[t].Height;
            if (t != recs.Count - 1)
                filecontent = filecontent + ",\n";
        }
        File.WriteAllText(file, filecontent);
    }
    public void f_recget(
        string file,
        ref List<Rectangle> recs)
    {
        string filecontent = File.ReadAllText(file);
        string[] split = filecontent.Split(',');
        for (int t = 0; t < split.GetLength(0); t++)
        {
            string[] temp = split[t].Split(' ');
            recs.Add(new Rectangle(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]), int.Parse(temp[3])));
        }
    }
    #endregion
}