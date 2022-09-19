using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJom
{
    class TileMap
    {
        static MouseState mouseState = Game1.mouseState;
        static Texture2D SelectTexture;
        static Point TileSize = new Point(96, 96);
        static float Zoom = 1;
        int LayerSerialNum;
        AutomatedDraw Graphics;
        int ChangeTo;
        List<List<int>> BaseTileMap;
        public TileMap(bool Drawn)
        {

            Graphics = new AutomatedDraw(Game1.ScreenBounds, Color.White, Drawn, Zoom);
        }
        public void Update()
        {
            Rectangle mouselocation = Graphics.DisplayToCalculation(new Rectangle(mouseState.X, mouseState.Y, 0, 0));
            Point selectedTile = new Point((int)(mouselocation.X / TileSize.X), (int)(mouselocation.Y / TileSize.Y));
            Rectangle currentTile = new Rectangle(selectedTile.X * TileSize.X, selectedTile.Y * TileSize.Y, TileSize.X, TileSize.Y);

            Button Tile = new Button(Graphics, Graphics.Drawn);
            Tile.ButtonUpdate(currentTile, SelectTexture);
            if (Tile.PressedLeft)
            {
                BaseTileMap[selectedTile.X][selectedTile.Y] = ChangeTo;
            }
        }
    }
    class LevelEditor
    {
        static AutomatedDraw LevelGraphics;
        Point TileMapSize;

        public LevelEditor(Point tileMap)
        { TileMapSize = tileMap; }
        public LevelEditor() : this(new Point(10, 10)) { }

        static public void Save()
        {

        }

        static public void Load()
        {

        }


    }
}
