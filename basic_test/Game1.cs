﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using IWshRuntimeLibrary;
using System.Runtime.Serialization;
using fiel = System.IO.File;

namespace basic_test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>       1172
    public class Game1 : Game
    {
        #region classes

        colision f_collision = new colision();
        fill f_fill = new fill();
        save f_save = new save();

        #endregion
        #region variables
        int[,] tileMap;
        double zoom = 1;
        double edit_zoom = 1;
        int tilesize = 96;
        bool dead = false;
        bool is_crouching = false;
        bool exiting = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseState;
        int mousepos_x;
        int mousepos_y;
        private Texture2D standurdised_box;
        private Texture2D level;
        private Texture2D pausescreen;
        private Texture2D pointer;
        private Texture2D bar;
        private Texture2D grid;
        private Texture2D transparent;
        private Texture2D spikes;

        private Texture2D b_play;
        private Texture2D b_exit;
        private Texture2D b_level;
        private Texture2D b_pause;
        private Texture2D b_menu;
        private Texture2D b_resume;   
        private Texture2D b_save;
        private Texture2D b_test;

        public Rectangle _player = new Rectangle(96, 288, 160, 80);
        Rectangle r_level = new Rectangle(0,0,0,0);
        private int y_velocityDown = -12;
        private int x_velocityLeft = -0;
        bool debug = false;
        string gamestate = "menu";
        int wiggleroom_x = 0;
        int wiggleroom_y = 0;
        int camera_moveTo_x = 0;
        int camera_moveTo_y = 0;
        int camera_move_x = 0;
        int camera_move_y = 0;
        //leveleditor

        List<List<int>> edit_tilemap = new List<List<int>>();
        List<List<int>> edit_roomTilemap = new List<List<int>>();
        List<Rectangle> edit_rooms = new List<Rectangle>();
        List<List<int>> edit_spikesUp = new List<List<int>>();
        List<List<int>> edit_spikesRight = new List<List<int>>();
        List<List<int>> edit_spikesDown = new List<List<int>>();
        List<List<int>> edit_spikesLeft = new List<List<int>>();
        int[,] spike_calculation = { { 0 } };
        int edit_currentRoom = 0;
        int test_currentRoom = 0;
        int first_pointX;
        int first_pointY;
        int[,] test_tilemap;
        int edit_sizeX = 10;
        int edit_sizeY = 10;
        int f = 1;
        int x_offset;
        int y_offset;
        int spikeSide = 0;
        enum spikeside
        {
            up = 0,
            right = 1,
            down = 2
        }
        //key press

        List<bool> pressed = new List<bool>();
        bool pressed_escape;
        bool pressed_x;
        bool pressed_y;
        bool pressed_testButton;
        bool pressed_leftMouseButton;

        //movement variables
        

        //horozontal

        private int speed = 8;
        private int friction = 8;
        private int speedcap = 16;

        //vertical
        
        private int fallcap1 = 24;
        private int fallcap2 = 27;
        private int drag = 2;
        private int gravspeed = 3;
        private int jumpspeed = 24;
        private bool autojustpreventer = false;

        int climb_speed = -6;
        int slip_speed = 6;

        //timer variables

        //horozontal

        private double t_accelerationDelay = 5;
        private double C_timeSinceLastAccelerationUpdate = 0;

        private double t_fricdelay = 4;
        private double C_timeSinceLastFrictionUpdate = 0;

        //vertical

        private double t_jumpslowdowndelay = 0;
        private double C_timesincelastjumpslowdown = 0;

        private double jumpvariation_upper = 6;
        private double jumpvariation_lower = 3;
        private double _timetilljumpslowdown = 0;

        private double falldelay = 0;
        private double _timesincelastfallacc = 0;

        private double airresdelay = 8;
        private double C_timesincelastairrescheck = 0;

        bool wall_climb = false;

        //main movement

        private double movedelay = 0;
        private double _timesincelastmove = 0;

        private bool[] iscoliding =
        {
            false,
            false,
            false,
            false,
        };
        private bool[] aircheck =
        {
            false,
            false,
            false,
            false
        };
        bool[] isSliping = 
        {
            false,
            false,
            false,
            false
        };
        Rectangle paused;
        Rectangle menu;
        Rectangle resume;
        Rectangle play;
        Rectangle level_editer;
        Rectangle exit;
        Rectangle edit_menu;
        Rectangle save;
        Rectangle test;
        #endregion
        public Game1()
        
        {
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 2160;
            graphics.PreferredBackBufferWidth = 3840;
            graphics.IsFullScreen = true;
            
            Content.RootDirectory = "Content";
        }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {





            // TODO: Add your initialization logic here
            paused = new Rectangle ((graphics.PreferredBackBufferWidth - 720) / 2, (graphics.PreferredBackBufferHeight / 2) - 280, 720, 192);
            menu = new Rectangle((graphics.PreferredBackBufferWidth - 228) / 2, (graphics.PreferredBackBufferHeight / 2), 228, 60);
            resume = new Rectangle((graphics.PreferredBackBufferWidth - 360) / 2, (graphics.PreferredBackBufferHeight / 2) + 80, 360, 84);
            play = new Rectangle(200, 500, 408, 168);
            exit = new Rectangle(200, 750, 384, 168);
            level_editer = new Rectangle(200, 1000, 1176, 168);
            edit_menu = new Rectangle(40, 40, 228 * 2, 60 * 2);
            save = new Rectangle(228 * 2 + 40 * 3, 40, 252 * 2, 60 * 2);
            test = new Rectangle(228 * 2 + 40 * 5 + 252 * 2, 16, 252 * 2, 72 * 2);
            base.Initialize();
            _player.Width = 84;
            _player.Height = 108;
            pressed_x = false;
            pressed_y = false;

            f_fill.f_fill(ref edit_roomTilemap, edit_sizeX, edit_sizeY);
            f_fill.f_fill(ref edit_tilemap, edit_sizeX, edit_sizeY);

            if (!fiel.Exists("position_x.txt"))
                fiel.WriteAllText("position_x.txt", _player.X.ToString());
            if (!fiel.Exists("position_y.txt"))
                fiel.WriteAllText("position_y.txt", _player.Y.ToString());
            if (!fiel.Exists("notrightspeed.txt"))
                fiel.WriteAllText("notrightspeed.txt", x_velocityLeft.ToString());
            if (!fiel.Exists("fallspeed.txt"))
                fiel.WriteAllText("fallspeed.txt", y_velocityDown.ToString());
            if (!fiel.Exists("list.txt"))
                f_save.f_listsave(edit_tilemap, "list.txt");
            if (!fiel.Exists("rooms.txt"))
                f_save.f_listsave(edit_roomTilemap, "rooms_map.txt");

            if (fiel.Exists("list.txt"))
                f_save.f_listget("list.txt", ref edit_tilemap, ref edit_sizeX, ref edit_sizeY);
            if (fiel.Exists("rooms_map.txt"))
                f_save.f_listget("rooms_map.txt", ref edit_roomTilemap, ref edit_sizeX, ref edit_sizeY);
            if (fiel.Exists("rooms.txt"))
                f_save.f_recget("rooms.txt", ref edit_rooms);


            _player.X = int.Parse(fiel.ReadAllText("position_x.txt"));
            _player.Y = int.Parse(fiel.ReadAllText("position_y.txt"));
            x_velocityLeft = int.Parse(fiel.ReadAllText("notrightspeed.txt"));
            y_velocityDown = int.Parse(fiel.ReadAllText("fallspeed.txt"));
            tileMap = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,1},
                {1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,1},
                {1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,1},
                {1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,1,1},
                {1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1},
                {1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1},
                {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1},                
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},

            };
        }
        #region functions
        #region button function
        static public void f_button(
            Rectangle dimentions,
            ref bool ispressed)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.X > dimentions.X &
                mouseState.X < dimentions.X + dimentions.Width &
                mouseState.Y > dimentions.Y &
                mouseState.Y < dimentions.Y + dimentions.Height &
                (mouseState.LeftButton == ButtonState.Pressed ||
                mouseState.RightButton == ButtonState.Pressed))
            {
                ispressed = true;
            }
        }
        #endregion
        #region list to array
        static public void f_convert(ref int[,] output, List<List<int>> input)
        {
            output = new int[input.Count, input[0].Count];
            for (int y = 0; y < output.GetLength(0); y++)
            {
                for (int x = 0; x < output.GetLength(1); x++)
                {
                    output[y, x] = input[y][x];
                }
            }
        }
        #endregion

        #endregion

        /*   test_tilemap = new int[edit_sizeY + 2, edit_sizeX + 2];
                for (int y = 0; y < test_tilemap.GetLength(0); y++)
                {
                    for (int x = 0; x < test_tilemap.GetLength(1); x++)
                    {
                        if (y == 0 || y == test_tilemap.GetLength(0) - 1 ||
                            x == 0 || x == test_tilemap.GetLength(1) - 1)
                        {
                            test_tilemap[y, x] = 1;
                        }
                        else if (y <= test_tilemap.GetLength(0) - 2 && x <= test_tilemap.GetLength(1) - 2)
                        {
                            test_tilemap[y, x] = edit_tilemap[y - 1][x - 1];
                        }
                    }
                }
                */
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.play
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            standurdised_box = Content.Load<Texture2D>("BasicShape");
            level = Content.Load<Texture2D>("room 1");
            pausescreen = Content.Load<Texture2D>("pause screen");
            pointer = Content.Load<Texture2D>("pointer");
            bar = Content.Load<Texture2D>("bar");
            grid = Content.Load<Texture2D>("grid");
            transparent = Content.Load<Texture2D>("Room Tiles/transparent");
            spikes = Content.Load<Texture2D>("spikes");
            //temporary

            b_play = Content.Load<Texture2D>("button/play");
            b_exit = Content.Load<Texture2D>("button/exit");
            b_level = Content.Load<Texture2D>("button/level editer");
            b_pause = Content.Load<Texture2D>("button/pause");
            b_menu = Content.Load<Texture2D>("button/menu");
            b_resume = Content.Load<Texture2D>("button/resume");
            b_save = Content.Load<Texture2D>("button/save");
            b_test = Content.Load<Texture2D>("button/test");
            // TODO: use this.Content to load your game content here
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() 
        {
            // TODO: Unload any non ContentManager content here
            // don't matter unless the game is biggggggggggggg
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mousepos_x = mouseState.X;
            mousepos_y = mouseState.Y;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.C)|| exiting)
            {
                fiel.WriteAllText("position_x.txt", _player.X.ToString());
                fiel.WriteAllText("position_y.txt", _player.Y.ToString());
                fiel.WriteAllText("notrightspeed.txt", x_velocityLeft.ToString());
                fiel.WriteAllText("fallspeed.txt", y_velocityDown.ToString());
                f_save.f_listsave(edit_roomTilemap, "rooms_map.txt");
                f_save.f_listsave(edit_tilemap, "list.txt");
                f_save.f_recsave(edit_rooms, "rooms.txt");
                Exit();
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                pressed_escape = false;
            switch (gamestate)
            {
                case "menu":
                    break;
                case "paused":
                    break;
                case "editing":
                    break;
                case "playing":
                    break;
                default:
                    break;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) & pressed_escape == false)
            {
                if (gamestate == "paused")
                    gamestate = "playing";
                else if (gamestate == "playing")
                    gamestate = "paused";
                pressed_escape = true;
            }
            #region buttons
            bool pressed_resume = false;
            f_button(resume, ref pressed_resume);
            if (pressed_resume &
                gamestate == "paused")
            {
                gamestate = "playing";
            }
            bool pressed_menu = false;
            f_button(menu, ref pressed_menu);
            if (pressed_menu & 
                gamestate == "paused")
            {
                gamestate = "menu";
            }
            bool pressed_play = false;
            f_button(play, ref pressed_play);
            if (pressed_play &
                gamestate == "menu")
            {
                gamestate = "playing";
                _player.X = int.Parse(fiel.ReadAllText("position_x.txt"));
                _player.Y = int.Parse(fiel.ReadAllText("position_y.txt"));
                x_velocityLeft = int.Parse(fiel.ReadAllText("notrightspeed.txt"));
                y_velocityDown = int.Parse(fiel.ReadAllText("fallspeed.txt"));
                camera_moveTo_x = 0;
                camera_moveTo_y = 0;
                zoom = 1;
            }
            bool pressed_exit = false;
            f_button(exit, ref pressed_exit);
            if (pressed_exit &
                gamestate == "menu")
            {
                exiting = true;
            }
            bool pressed_editor = false;
            f_button(level_editer, ref pressed_editor);
            if (pressed_editor & 
                gamestate == "menu")
            {
                gamestate = "editing";
            }
            bool pressed_menu2 = false;
            f_button(edit_menu, ref pressed_menu2);
            if (pressed_menu2 & 
                (gamestate == "editing" ||
                gamestate == "testing"))
            {
                gamestate = "menu";
            }
            bool pressed_test = false;
            f_button(test, ref pressed_test);
            if (pressed_test &
                (gamestate == "editing" ||
                gamestate == "testing") &
                !pressed_testButton)
            {
                test_tilemap = new int[edit_sizeY + 2, edit_sizeX + 2];
                for (int y = 0; y < test_tilemap.GetLength(0); y++)
                {
                    for (int x = 0; x < test_tilemap.GetLength(1); x++)
                    {
                        if (y == 0 || y == test_tilemap.GetLength(0) - 1 ||
                            x == 0 || x == test_tilemap.GetLength(1) - 1)
                        {
                            test_tilemap[y, x] = 1;
                        }
                        else if (y <= test_tilemap.GetLength(0) - 2 && x <= test_tilemap.GetLength(1) - 2)
                        {
                            test_tilemap[y, x] = edit_tilemap[y - 1][x - 1];
                        }
                    }
                }
                pressed_testButton = true;
                if (gamestate != "testing")
                {
                    f_convert(ref spike_calculation, edit_spikesUp);
                    gamestate = "testing";
                }
                else
                {
                    gamestate = "editing";
                }
                _player.X = 100;
                _player.Y = 100;
            }
            else if (!pressed_test)
            {
                pressed_testButton = false;
            }
            #endregion
            #region LEVEL EDITOR
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                f = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                f = 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                f = 3;
            }
            if (gamestate == "editing")
            {
                #region size
                if (Keyboard.GetState().IsKeyDown(Keys.X) &
                    pressed_x == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            edit_sizeX -= 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            edit_sizeX -= 100;
                        }
                        else
                        {
                            edit_sizeX -= 1;
                        }
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            edit_sizeX += 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            edit_sizeX += 100;
                        }
                        else
                        {
                            edit_sizeX += 1;
                        }
                    }
                    pressed_x = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Y) &
                    pressed_y == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            edit_sizeY -= 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            edit_sizeY -= 100;
                        }
                        else
                        {
                            edit_sizeY -= 1;
                        }
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            edit_sizeY += 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            edit_sizeY += 100;
                        }
                        else
                        {
                            edit_sizeY += 1;
                        }
                    }
                    pressed_y = true;
                    f_fill.f_fill(ref edit_tilemap, edit_sizeX, edit_sizeY);
                    f_fill.f_fill(ref edit_roomTilemap, edit_sizeX, edit_sizeY);
                    #endregion
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    y_offset -= 10;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    x_offset -= 10;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) &
                    y_offset <= -10)
                {
                    y_offset += 10;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) &
                    x_offset <= -10)
                {
                    x_offset += 10;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemMinus) &
                    edit_zoom >= 20 / 96)
                {
                    edit_zoom -= 0.01;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                {
                    edit_zoom += 0.01;
                }
                int y = (int)(((mousepos_y - y_offset) - 192) / (96 * edit_zoom));
                int x = (int)((mousepos_x - x_offset) / edit_zoom / 96);
                if (f == 1)
                {
                    if (edit_tilemap.Count == edit_sizeY &
                        edit_tilemap[edit_tilemap.Count - 1].Count == edit_sizeX)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed &
                            y < edit_sizeY & y >= 0 &
                            x < edit_sizeX)
                        {
                            edit_tilemap[y][x] = 1;
                        }
                        if (mouseState.RightButton == ButtonState.Pressed &
                            y < edit_sizeY & y >= 0 &
                            x < edit_sizeX)
                        {
                            edit_tilemap[y][x] = 0;
                        }
                    }
                }
                if (f == 2)
                {
                    if ((mouseState.RightButton == ButtonState.Pressed) == false)
                    {
                        if (edit_tilemap.Count == edit_sizeY)
                        {
                            if (edit_tilemap[edit_tilemap.Count - 1].Count == edit_sizeX)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed &
                                    !pressed_leftMouseButton)
                                {
                                    if (y < edit_sizeY & y >= 0 &
                                        x < edit_sizeX)
                                    {
                                        first_pointX = x;
                                        first_pointY = y;
                                    }
                                    pressed_leftMouseButton = true;
                                }
                                if ((mouseState.LeftButton == ButtonState.Pressed) == false & 
                                    pressed_leftMouseButton)
                                {
                                    int first_pointModifiedX = first_pointX * tilesize;
                                    int first_pointModifiedY = first_pointY * tilesize;
                                    Rectangle addedRectangle = new Rectangle(first_pointModifiedX, first_pointModifiedY, x * tilesize - first_pointModifiedX + tilesize, y * tilesize - first_pointModifiedY + tilesize + 192);
                                    bool overlap = false;
                                    for (int r = edit_currentRoom; r > 0; r--)
                                    {
                                        if (!(addedRectangle.X + addedRectangle.Width < edit_rooms[r].X ||
                                            addedRectangle.X > edit_rooms[r].X + edit_rooms[r].Width &&
                                            addedRectangle.Y + addedRectangle.Height < edit_rooms[r].Y ||
                                            addedRectangle.Y > edit_rooms[r].Y + edit_rooms[r].Height))
                                        {
                                            overlap = true;
                                        }
                                    }
                                    if (x * tilesize - first_pointModifiedX >= graphics.PreferredBackBufferWidth * zoom &
                                        y * tilesize - first_pointModifiedY >= graphics.PreferredBackBufferHeight * zoom &
                                        overlap == false)
                                    {
                                        edit_rooms.Add(addedRectangle);
                                        for(int t_y = first_pointY; t_y <= y; t_y++)
                                        {
                                            for (int t_x = first_pointX; t_x <= x; t_x++)
                                            {
                                                if (t_y == first_pointY &
                                                    t_x == first_pointX)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 2;
                                                }
                                                else 
                                                if (t_y == first_pointY &
                                                    t_x == x)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 4;
                                                }
                                                else
                                                if (t_y == y &
                                                    t_x == x)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 6;
                                                }
                                                else
                                                if (t_y == y &
                                                    t_x == first_pointX)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 8;
                                                }
                                                else
                                                if (t_y == first_pointY)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 3;
                                                }
                                                else
                                                if (t_x == x)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 5;
                                                }
                                                else
                                                if (t_y == y)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 7;
                                                }
                                                else
                                                if (t_x == first_pointX)
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 9;
                                                }
                                                else
                                                {
                                                    edit_roomTilemap[t_y][t_x] = 1;
                                                }
                                            }
                                        }
                                    }
                                    pressed_leftMouseButton = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        bool pressed = false;
                        for (int t = 0; t < edit_rooms.Count; t++)
                        {
                            f_button(new Rectangle(
                                (int)(edit_rooms[t].X * edit_zoom + x_offset), 
                                (int)(edit_rooms[t].Y * edit_zoom + y_offset), 
                                (int)(edit_rooms[t].Width * edit_zoom), 
                                (int)(edit_rooms[t].Height * edit_zoom)), ref pressed);
                            if (pressed)
                            {
                                int firstPointX = edit_rooms[t].X / 96;
                                int firstPointY = edit_rooms[t].Y / 96;
                                int lastPointX = (edit_rooms[t].X + edit_rooms[t].Width) / 96;
                                int lastPointY = (edit_rooms[t].Y + edit_rooms[t].Height) / 96;
                                for(int Yvalue = firstPointY; Yvalue < lastPointY; Yvalue++)
                                {
                                    for (int Xvalue = firstPointX; Xvalue < lastPointX;  Xvalue++)
                                    {
                                        edit_roomTilemap[Yvalue][Xvalue] = 0;
                                    }
                                }
                                edit_rooms.Remove(edit_rooms[t]);
                                break;
                            }
                        }
                    }
                }
                if (f == 3)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        spikeSide = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        spikeSide = 1;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        spikeSide = 2;
                    }
                    if (edit_tilemap.Count == edit_sizeY &
                        edit_tilemap[edit_tilemap.Count - 1].Count == edit_sizeX)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed &
                        y < edit_sizeY & y >= 0 &
                        x < edit_sizeX)
                        {
                            switch (spikeSide)
                            {
                                case 0:
                                    edit_spikesUp[y + 2][x + 1] = 1;
                                    break;
                                case 1:
                                    edit_spikesRight[y][x] = 1;
                                    break;
                                case 2:
                                    edit_spikesLeft[y + 1][x + 2] = 1;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (mouseState.RightButton == ButtonState.Pressed &
                            y < edit_sizeY & y >= 0 &
                            x < edit_sizeX)
                        {
                            switch (spikeSide)
                            {
                                case 0:
                                    edit_spikesUp[y + 2][x + 1] = 0;
                                    break;
                                case 1:
                                    edit_spikesRight[y][x] = 0;
                                    break;
                                case 2:
                                    edit_spikesLeft[y + 1][x + 2] = 0;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion
            #region debug
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                if (debug == true)
                {
                    debug = false;
                }
                if (debug == false)
                {
                    debug = true;
                }
            }
            #endregion
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                Texture2D sus = null;
                string impos = null;
                f_save.select(ref sus, ref impos);
            }
            //--------------------------------------------//
            //                                            //
            //                  MOVEMENT                  //
            //                                            //
            //--------------------------------------------//
            if (gamestate == "playing" ||
                gamestate == "testing")
            {
                #region MOVEMENT update
                    int[,] usedTileMap = tileMap;
                if (gamestate == "testing")
                {
                    usedTileMap = test_tilemap;
                }
                else
                {
                    usedTileMap = tileMap;
                }
                if (_timesincelastmove >= movedelay)
                {
                    f_collision._collide(
                        ref _player,
                        tilesize,
                        usedTileMap,
                        ref x_velocityLeft,
                        ref y_velocityDown,
                        ref iscoliding,
                        1);
                        _player.X -= x_velocityLeft;
                        _player.Y += y_velocityDown;
                        _timesincelastmove = 0;
                    if (iscoliding[0] == true & x_velocityLeft > 0) 
                    {
                        x_velocityLeft = 0;
                    }
                    if (iscoliding[1] == true & x_velocityLeft < 0)
                    {
                        x_velocityLeft = 0;
                    }
                    if (iscoliding[2] == true & y_velocityDown < 0)
                    {
                        y_velocityDown = 0;
                    }
                    if (iscoliding[3] == true & y_velocityDown > 0)
                    {
                        y_velocityDown = 0;
                    }
                    if (true)
                    {
                        aircheck[0] = false;
                        aircheck[1] = false;
                        aircheck[2] = false;
                        aircheck[3] = false;
                        int X = 1;
                        int Y = -1;
                        f_collision._collide(ref _player, tilesize, usedTileMap, ref X, ref Y, ref aircheck, 1);
                        X = 1;
                        Y = 1;
                        f_collision._collide(ref _player, tilesize, usedTileMap, ref X, ref Y, ref aircheck, 1);
                        X = -1;
                        Y = -1;
                        f_collision._collide(ref _player, tilesize, usedTileMap, ref X, ref Y, ref aircheck, 1);
                        X = -1;
                        Y = 1;
                        f_collision._collide(ref _player, tilesize, usedTileMap, ref X, ref Y, ref aircheck, 1);
                        iscoliding[0] = aircheck[0];
                        iscoliding[1] = aircheck[1];
                        iscoliding[2] = aircheck[2];
                        iscoliding[3] = aircheck[3];
                    }
                    #region spikes
                    int x = 0;
                    int y = 1;
                    isSliping = new bool[] { false, false, false, false };
                    f_convert(ref spike_calculation, edit_spikesUp);
                    f_collision._collide(ref _player, tilesize, spike_calculation, ref x, ref y, ref isSliping, 1);
                    if (isSliping[3])
                    {
                        dead = true;
                    }
                    x = 1;
                    y = 0;
                    f_convert(ref spike_calculation, edit_spikesRight);
                    f_collision._collide(ref _player, tilesize, spike_calculation, ref x, ref y, ref isSliping, 1);
                    if (isSliping[0])
                    {
                        dead = true;
                    }
                    x = -1;
                    y = 0;
                    f_convert(ref spike_calculation, edit_spikesLeft);
                    f_collision._collide(ref _player, tilesize, spike_calculation, ref x, ref y, ref isSliping, 1);
                    if (isSliping[1])
                    {
                        dead = true;
                    }

                    if (!(isSliping[0] || isSliping[1] || isSliping[2] || isSliping[3]))
                    {
                        dead = false;
                    }
                    #endregion
                }
                #endregion
                #region camera
                r_level = new Rectangle(
                    0,
                    0,
                    (int)((tileMap.GetLength(1) - 2) * tilesize * zoom),
                    (int)((tileMap.GetLength(0) - 5) * tilesize * zoom));
                if (gamestate == "testing")
                {
                    for (int r = 0; r < edit_rooms.Count; r++)
                    {
                        if (_player.X + _player.Width > edit_rooms[r].X + tilesize &
                            _player.X < edit_rooms[r].X + edit_rooms[r].Width + tilesize &
                            _player.Y + _player.Height > edit_rooms[r].Y &
                            _player.Y < edit_rooms[r].Y + edit_rooms[r].Height &
                            r != test_currentRoom)
                        {
                            test_currentRoom = r;
                        }
                    }
                }
                if (gamestate == "testing")
                {
                    r_level = edit_rooms[test_currentRoom];
                }
                if (r_level.Height > graphics.PreferredBackBufferHeight)
                {
                    wiggleroom_y = (int)((r_level.Height - graphics.PreferredBackBufferHeight) * zoom);
                    if (r_level.Y - camera_moveTo_y < 0
                        & camera_moveTo_y > (int)((_player.Y + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2)
                    {
                        camera_moveTo_y = (int)((_player.Y + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2;
                    }
                    if (r_level.Y - camera_moveTo_y + r_level.Height > graphics.PreferredBackBufferHeight
                        & camera_moveTo_y < (int)((_player.Y + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2)
                    {
                        camera_moveTo_y = (int)((_player.Y + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2;
                    }
                    if (camera_moveTo_y - r_level.Y < 0)
                    {
                        camera_moveTo_y = 0 + r_level.Y;
                    }
                    if (camera_moveTo_y > wiggleroom_y + r_level.Y)
                    {
                        camera_moveTo_y = wiggleroom_y + r_level.Y;
                    }
                }
                if (r_level.Width > graphics.PreferredBackBufferWidth)
                {
                    wiggleroom_x = (int)((r_level.Width - graphics.PreferredBackBufferWidth) * zoom);
                    if (r_level.X - camera_moveTo_x < 0
                        & camera_moveTo_x > (int)((_player.X  + _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2)
                    {
                        camera_moveTo_x = (int)((_player.X + _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2;
                    }
                    if (r_level.X - camera_moveTo_x + r_level.Width > graphics.PreferredBackBufferWidth
                        & camera_moveTo_x < (int)((_player.X + _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2)
                    {
                        camera_moveTo_x = (int)((_player.X + _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2;
                    }
                    if (camera_moveTo_x - r_level.X < 0)
                    {
                        camera_moveTo_x = 0 + r_level.X;
                    }
                    if (camera_moveTo_x > wiggleroom_x + r_level.X)
                    {
                        camera_moveTo_x = wiggleroom_x + r_level.X;
                    }
                }
                // TODO: 
                camera_move_y += (int)((camera_moveTo_y - camera_move_y) / 16);
                camera_move_x += (int)((camera_moveTo_x - camera_move_x) / 16);
                if ((int)((camera_moveTo_y - camera_move_y) / 16) > -1 ||
                    (int)((camera_moveTo_y - camera_move_y) / 16) < 1)
                {
                    if (camera_moveTo_y > camera_move_y)
                        camera_move_y += 1;
                    if (camera_moveTo_y < camera_move_y)
                        camera_move_y -= 1;
                }

                if ((int)((camera_moveTo_x - camera_move_x) / 16) < 1)
                {
                    if (camera_moveTo_x > camera_move_x)
                        camera_move_x += 1;
                    if (camera_moveTo_x < camera_move_x)
                        camera_move_x -= 1;
                }
                #endregion
                #region movement
                #region squish
                if (Keyboard.GetState().IsKeyDown(Keys.S)
                    & iscoliding[3])
                {

                    is_crouching = true;
                }
                else
                {
                    is_crouching = false;
                }
                #endregion
                #region HOROZONTAL MOVEMENT
                if (C_timeSinceLastAccelerationUpdate > t_accelerationDelay)
                {
                    if (is_crouching == false
                        & (wall_climb == false ||
                        iscoliding[3]))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.D)
                            & !Keyboard.GetState().IsKeyDown(Keys.A)
                            & iscoliding[1] == false)
                        {
                            if (x_velocityLeft > -speedcap)
                                x_velocityLeft -= speed;
                            else
                            {

                            }
                            
                            C_timeSinceLastAccelerationUpdate = 0;
                            iscoliding[0] = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.A)
                            & !Keyboard.GetState().IsKeyDown(Keys.D)
                            & iscoliding[0] == false)
                        {
                            if (x_velocityLeft < speedcap)
                                x_velocityLeft += speed;
                            C_timeSinceLastAccelerationUpdate = 0;
                            iscoliding[1] = false;
                        }
                    }
                }
                #endregion
                #region JUMP
                if (iscoliding[3] == true
                    & Keyboard.GetState().IsKeyDown(Keys.Space)
                    & autojustpreventer == false)
                {
                    _timetilljumpslowdown = 0;
                    iscoliding[3] = false;
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        x_velocityLeft -= speed;
                        if (Keyboard.GetState().IsKeyDown(Keys.S) &
                            !isSliping[3])
                        {
                            x_velocityLeft -= speed;
                            y_velocityDown -= (jumpspeed * 3) / 4;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.W) &
                            !isSliping[3])
                        {
                            y_velocityDown += (int)(x_velocityLeft * 1.5);
                            x_velocityLeft += speed;
                        }
                        else
                        {
                            y_velocityDown -= jumpspeed;
                        }
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        x_velocityLeft += speed;
                        if (Keyboard.GetState().IsKeyDown(Keys.S) &
                            !isSliping[3])
                        {
                            x_velocityLeft += speed;
                            y_velocityDown -= (jumpspeed * 3) / 4;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.W) &
                            !isSliping[3])
                        {
                            y_velocityDown -= (int)(x_velocityLeft * 1.5);
                            x_velocityLeft -= speed;
                        }
                        else
                        {
                            y_velocityDown -= jumpspeed;
                        }
                    }
                    else
                    {
                        y_velocityDown -= jumpspeed;
                    }
                
                    
                    autojustpreventer = true;
                }
                if (wall_climb == true
                    & Keyboard.GetState().IsKeyDown(Keys.Space)
                    & autojustpreventer == false)
                {
                    if (y_velocityDown < 0)
                        y_velocityDown = 0;
                    y_velocityDown -= (int)(jumpspeed * 0.75);
                    _timetilljumpslowdown = 0;
                    wall_climb = false;
                    #region left
                    if (iscoliding[0])
                    {
                        x_velocityLeft -= (int)(jumpspeed * 0.8);
                    }
                    if (iscoliding[0]
                        & Keyboard.GetState().IsKeyDown(Keys.A)
                        & !Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        x_velocityLeft += (int)(jumpspeed * 0.4);
                    }
                    if (iscoliding[0]
                        & Keyboard.GetState().IsKeyDown(Keys.D)
                        & !Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        x_velocityLeft -= (int)(jumpspeed * 0.4);
                    }
                    #endregion
                    #region right
                    if (iscoliding[1])
                    {
                        x_velocityLeft += (int)(jumpspeed * 0.8);
                    }

                    if (iscoliding[1]
                        & Keyboard.GetState().IsKeyDown(Keys.D)
                        & !Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        x_velocityLeft -= (int)(jumpspeed * 0.4);
                    }
                    if (iscoliding[1]
                        & Keyboard.GetState().IsKeyDown(Keys.A)
                        & !Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        x_velocityLeft += (int)(jumpspeed * 0.4);
                    }
                    #endregion
                    autojustpreventer = true;
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    autojustpreventer = false;
                }

                #endregion
                #region GRAVITY
                if (iscoliding[3] == false
                    & y_velocityDown >= -4
                    & y_velocityDown < fallcap1
                    & _timesincelastfallacc >= falldelay
                    & wall_climb == false)
                {
                    y_velocityDown += gravspeed;
                    _timesincelastfallacc = 0;
                }


                if (y_velocityDown > fallcap2
                    & C_timesincelastairrescheck < airresdelay)
                {
                    y_velocityDown -= drag;
                    C_timesincelastairrescheck = 0;
                }
                if ((!Keyboard.GetState().IsKeyDown(Keys.Space)
                    & C_timesincelastjumpslowdown >= t_jumpslowdowndelay
                    & _timetilljumpslowdown >= jumpvariation_lower
                    || _timetilljumpslowdown >= jumpvariation_upper)
                    & y_velocityDown < 0
                    & C_timesincelastjumpslowdown >= t_jumpslowdowndelay)
                {
                    y_velocityDown += gravspeed;
                    C_timesincelastjumpslowdown = 0;
                }
                #endregion
                #region FRICTION
                // left
                if (((!Keyboard.GetState().IsKeyDown(Keys.A)
                | Keyboard.GetState().IsKeyDown(Keys.D))
                & x_velocityLeft > 0)
                | x_velocityLeft > speedcap)
                {
                    if (iscoliding[3] & 
                        C_timeSinceLastFrictionUpdate > t_fricdelay)
                    {
                        if (x_velocityLeft >= friction)
                        {
                            x_velocityLeft -= friction;
                        }
                        else
                        {
                            x_velocityLeft = 0;
                        }
                        C_timeSinceLastFrictionUpdate = 0;
                    }
                    if (C_timesincelastairrescheck > airresdelay & iscoliding[3] == false)
                    {
                        if (x_velocityLeft >= drag)
                        {
                            x_velocityLeft -= drag;
                        }
                        else
                        {
                            x_velocityLeft = 0;
                        }
                        C_timesincelastairrescheck = 0;
                    }
                }
                // right
                if (((!Keyboard.GetState().IsKeyDown(Keys.D)
                | Keyboard.GetState().IsKeyDown(Keys.A))
                & x_velocityLeft < 0)
                | x_velocityLeft < -speedcap)
                {
                    if (iscoliding[3] &
                        C_timeSinceLastFrictionUpdate > t_fricdelay)
                    {
                        if (x_velocityLeft <= -friction)
                        {
                            x_velocityLeft += friction;
                        }
                        else
                        {
                            x_velocityLeft = 0;
                        }
                        C_timeSinceLastFrictionUpdate = 0;
                    }
                    if (C_timesincelastairrescheck > airresdelay & iscoliding[3] == false)
                    {
                        if (x_velocityLeft <= -drag)
                        {
                            x_velocityLeft += drag;
                        }
                        else
                        {
                            x_velocityLeft = 0;
                        }
                        C_timesincelastairrescheck = 0;
                    }
                }


                #endregion
                #region wall climb
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)
                    & ((iscoliding[1] & !isSliping[1])
                    || (iscoliding[0]) & !isSliping[0]))
                {
                    wall_climb = true;
                    if (y_velocityDown > 0)
                    {
                        y_velocityDown = 0;
                    }
                }
                else
                {
                    wall_climb = false;
                }
                if (wall_climb == true
                    & Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (y_velocityDown > climb_speed)
                    {
                        y_velocityDown = climb_speed;
                    }
                }
                if (wall_climb == true
                    & Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (y_velocityDown < slip_speed)
                    {
                        y_velocityDown = slip_speed;
                    }

                }
                #endregion
                #endregion
                base.Update(gameTime);
                #region timers
                C_timeSinceLastAccelerationUpdate += 1;
                C_timeSinceLastFrictionUpdate += 1;
                _timesincelastmove += 1;
                _timesincelastfallacc += 1;
                C_timesincelastairrescheck += 1;
                C_timesincelastjumpslowdown += 1;
                if (iscoliding[3] == false)
                {
                    _timetilljumpslowdown += 1;
                }
                #endregion
            } 
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            double used_zoom;
            if (gamestate == "testing" ||
                gamestate == "playing")
                used_zoom = zoom;
            else
                used_zoom = edit_zoom;
            int offsetY = 0;
            int offsetX = 0;
            if (gamestate == "playing" ||
                gamestate == "testing")
            {
                offsetX = camera_move_x;
                offsetY = camera_move_y;
            }
            else if (gamestate == "editing")
            {
                offsetX = -x_offset;
                offsetY = -y_offset;
            }
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (gamestate == "playing" ||
                gamestate == "testing" ||
                gamestate == "paused")
            {
                int x = (int)((_player.X - tilesize) / 12) * 12;
                int y = (int)((_player.Y) / 12) * 12;
                if (gamestate == "playing" ||
                    gamestate == "paused")
                {
                    spriteBatch.Draw(level, new Rectangle(
                        (int)((r_level.X * used_zoom) - offsetX),
                        (int)((r_level.Y * used_zoom) - offsetY),
                        r_level.Width,
                        r_level.Height),
                        Color.White);
                    spriteBatch.Draw(standurdised_box, new Rectangle(
                        (int)((x) * used_zoom - offsetX),
                        (int)((y - tilesize) * used_zoom - offsetY - tilesize * 3),
                        (int)(_player.Width * used_zoom),
                        (int)(_player.Height * used_zoom)),
                        Color.White);
                }
                else
                {
                    if (dead)
                    {
                        spriteBatch.Draw(standurdised_box, new Rectangle(
                            (int)((x) * used_zoom - offsetX),
                            (int)((y - tilesize) * used_zoom - offsetY + 192),
                            (int)(_player.Width * used_zoom),
                            (int)(_player.Height * used_zoom)),
                            Color.Red);
                    }
                    else
                        spriteBatch.Draw(standurdised_box, new Rectangle(
                            (int)((x) * used_zoom - offsetX),
                            (int)((y - tilesize) * used_zoom - offsetY + 192),
                            (int)(_player.Width * used_zoom),
                            (int)(_player.Height * used_zoom)),
                            Color.White);
                }   
            }
            if (gamestate == "paused")
            {
                spriteBatch.Draw(pausescreen, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.Draw(b_pause, paused, Color.White);
                spriteBatch.Draw(b_menu, menu, Color.White);
                spriteBatch.Draw(b_resume, resume, Color.White);
            }
            if (gamestate == "menu")
            {
                spriteBatch.Draw(standurdised_box, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black);
                spriteBatch.Draw(b_play, play, Color.White);
                spriteBatch.Draw(b_exit, exit, Color.White);
                spriteBatch.Draw(b_level, level_editer, Color.White);
            }
            if (gamestate == "editing" ||
                gamestate == "testing")
            {
                for (int y = 0; y < edit_sizeY; y ++)
                {
                    for (int x = 0; x < edit_sizeX; x++)
                    {
                        Rectangle grid_tile = new Rectangle(
                            (int)(x * tilesize * used_zoom) - offsetX,
                            (int)(y * tilesize * used_zoom) + 192 - offsetY,
                            (int)(tilesize * used_zoom),
                            (int)(tilesize * used_zoom));
                        if (edit_tilemap[y][x] == 0)
                        {
                            spriteBatch.Draw(grid, 
                                grid_tile, 
                                Color.Red);
                        }
                        else if (edit_tilemap[y][x] == 1)
                        {
                            spriteBatch.Draw(standurdised_box, 
                                grid_tile, 
                                Color.Red);
                        }
                    }
                }
                if (f == 3)
                {
                    switch (spikeSide)
                    {
                        case 0:
                            spriteBatch.Draw(spikes,
                                new Rectangle(
                                    (int)((int)(mousepos_x / (tilesize * used_zoom)) * (tilesize * used_zoom)), 
                                    (int)((int)(mousepos_y / (tilesize * used_zoom)) * (tilesize * used_zoom)), 
                                    (int)(tilesize * used_zoom), 
                                    (int)(tilesize * used_zoom)),
                                new Rectangle(0, 0, 16, 16),
                                Color.Red);
                            break;
                        case 1:
                            spriteBatch.Draw(spikes,
                                new Rectangle(
                                    (int)((int)(mousepos_x / (tilesize * used_zoom)) * (tilesize * used_zoom)),
                                    (int)((int)(mousepos_y / (tilesize * used_zoom)) * (tilesize * used_zoom)),
                                    (int)(tilesize * used_zoom),
                                    (int)(tilesize * used_zoom)),
                                new Rectangle(16, 0, 16, 16),
                                Color.Red);
                            break;
                        case 2:
                            spriteBatch.Draw(spikes,
                                new Rectangle(
                                    (int)((int)(mousepos_x / (tilesize * edit_zoom)) * (tilesize * used_zoom)),
                                    (int)((int)(mousepos_y / (tilesize * edit_zoom)) * (tilesize * used_zoom)),
                                    (int)(tilesize * used_zoom),
                                    (int)(tilesize * used_zoom)),
                                new Rectangle(0, 16, 16, 16),
                                Color.Red);
                            break;
                    }
                }
                f_fill.f_fill(ref edit_spikesUp, edit_sizeX, edit_sizeY);
                f_fill.f_fill(ref edit_spikesRight, edit_sizeX, edit_sizeY);
                f_fill.f_fill(ref edit_spikesDown, edit_sizeX, edit_sizeY);
                f_fill.f_fill(ref edit_spikesLeft, edit_sizeX, edit_sizeY);
                for (int y = 0; y < edit_sizeY; y++)
                {
                    for (int x = 0; x < edit_sizeX; x++)
                    {
                        Rectangle grid_tile = new Rectangle(
                            (int)(x * tilesize * used_zoom) - offsetX,
                            (int)(y * tilesize * used_zoom) + 192 - offsetY,
                            (int)(tilesize * used_zoom),
                            (int)(tilesize * used_zoom));
                        if (edit_spikesUp[y][x] == 1)
                        {
                            grid_tile.Y -= (int)((tilesize * 2) * used_zoom);
                            grid_tile.X -= (int)(tilesize * used_zoom);
                            spriteBatch.Draw(spikes,
                                grid_tile,
                                new Rectangle(0, 0, 16, 16),
                                Color.Red);
                            grid_tile.Y += (int)((tilesize * 2) * used_zoom);
                            grid_tile.X += (int)(tilesize * used_zoom);
                        }
                        if (edit_spikesRight[y][x] == 1)
                        {
                            grid_tile.Y -= (int)(tilesize * used_zoom);
                            spriteBatch.Draw(spikes,
                                grid_tile,
                                new Rectangle(16, 0, 16, 16),
                                Color.Red);
                            grid_tile.Y += (int)(tilesize * used_zoom);
                        }
                        if (edit_spikesLeft[y][x] == 1)
                        {
                            grid_tile.Y -= (int)(tilesize * used_zoom);
                            grid_tile.X -= (int)((tilesize * 2) * used_zoom);
                            spriteBatch.Draw(spikes,
                                grid_tile,
                                new Rectangle(0, 16, 16, 16),
                                Color.Red);
                            grid_tile.Y += (int)(tilesize * used_zoom);
                            grid_tile.X += (int)((tilesize * 2) * used_zoom);
                        }
                    }
                }
                if (f == 2)
                {
                    for (int y = 0; y < edit_sizeY; y++)
                    {
                        for (int x = 0; x < edit_sizeX; x++)
                        {
                            Rectangle grid_tile = new Rectangle(
                                (int)(x * tilesize * used_zoom) - offsetX,
                                (int)(y * tilesize * used_zoom) + 192 - offsetY,
                                (int)(tilesize * used_zoom),
                                (int)(tilesize * used_zoom));
                            if (edit_roomTilemap[y][x] == 1)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(16, 16, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 2)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(0, 0, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 3)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(16, 0, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 4)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(32, 0, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 5)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(32, 16, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 6)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(32, 32, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 7)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(16, 32, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 8)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(0, 32, 16, 16),
                                    Color.Green);
                            }
                            else if (edit_roomTilemap[y][x] == 9)
                            {
                                spriteBatch.Draw(transparent,
                                    grid_tile,
                                    new Rectangle(0, 16, 16, 16),
                                    Color.Green);
                            }
                        }
                    }
                }
                spriteBatch.Draw(bar, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, 192), Color.White);
                spriteBatch.Draw(b_menu, edit_menu, Color.White);
                spriteBatch.Draw(b_save, save, Color.White);
                spriteBatch.Draw(b_test, test, Color.White);
                if (!Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    pressed_x = false;
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    pressed_y = false;
                }
            }
            int variation = 192;
            SpriteFont font;
            font = Content.Load<SpriteFont>("Font");
            if (true)
            {
                spriteBatch.DrawString(font, "x :" + _player.X + "  Y :" + (_player.Y + _player.Height), new Vector2(50, 50 + variation), Color.White);
                spriteBatch.DrawString(font, "bot :" + iscoliding[3] + "  top :" + iscoliding[2], new Vector2(50, 70 + variation), Color.White);
                spriteBatch.DrawString(font, "lef :" + iscoliding[0] + "  rit :" + iscoliding[1], new Vector2(50, 90 + variation), Color.White);
                spriteBatch.DrawString(font, "x-speed :" + x_velocityLeft + "  Y-speed :" + y_velocityDown, new Vector2(50, 110 + variation), Color.White);
                spriteBatch.DrawString(font, "camra_move_x :" + camera_move_x + "  camera_move_y :" + camera_move_y, new Vector2(50, 130 + variation), Color.White);
                spriteBatch.DrawString(font, "temp 1 :" + dead + "  temp 2 :" + level.Width, new Vector2(50, 150 + variation), Color.White);
                spriteBatch.DrawString(font, "temp 3 :" + C_timeSinceLastAccelerationUpdate + "  temp 4 :" + C_timesincelastairrescheck, new Vector2(50, 170 + variation), Color.White);
            }
            //spriteBatch.Draw(Player, level_rec, Color.Black);
            spriteBatch.Draw(pointer, new Rectangle(mousepos_x, mousepos_y, 35, 45), Color.White); 

            spriteBatch.End();

            // TODO: Add your drawing code here
            graphics.IsFullScreen = true;
            base.Draw(gameTime);
        }
    }
}
