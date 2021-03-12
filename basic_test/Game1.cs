using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;


namespace basic_test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        
        #region variables
        int[,] tileMap;
        double zoom = 1;
        int tilesize = 96;
        bool is_crouching = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseState;
        int mousepos_x;
        int mousepos_y;
        private Texture2D Player;
        private Texture2D level;
        private Texture2D pausescreen;
        private Texture2D pointer;
        private Texture2D bar;
        private Texture2D grid;

        private Texture2D menu_b;
        private Texture2D play_button;
        private Texture2D exit_button;
        private Texture2D level_button;
        private Texture2D pause_text;
        private Texture2D Menu_text;
        private Texture2D resume_text;
        public Rectangle _player = new Rectangle(96, 288, 160, 80);
        Rectangle r_level = new Rectangle(0,0,0,0);
        private int _fallspeed = -12;
        private int _notrightspeed = -0;
        bool debug = false;
        bool is_paused;
        bool is_in_menu = true;
        bool escape_is_pressed;
        bool M_is_pressed;
        bool pressed_x;
        bool pressed_y;
        int wiggleroom_x = 0;
        int wiggleroom_y = 0;
        int camera_moveTo_x = 0;
        int camera_moveTo_y = 0;
        int camera_move_x = 0;
        int camera_move_y = 0;
        int size_x = 10;
        int size_y = 10;
        bool is_editing;
        List<List<int>> edit_tilemap = new List<List<int>>();
        //movement variables

        //horozontal

        private int speed = 5;
        private int friction = 2;
        private int speedcap = 18;

        //vertical
        
        private int fallcap1 = 24;
        private int fallcap2 = 36;
        private int drag = 1;
        private int gravspeed = 3;
        private int jumpspeed = 20;
        private bool autojustpreventer = false;

        int climb_speed = -6;
        int slip_speed = 8;

        //timer variables

        //horozontal

        private double accdelay = 0.1;
        private double _timesincelastacc = 0;

        private double fricdelay = 0;
        private double _timesincelastfric = 0;

        //vertical

        private double jumpslowdowndelay = 0;
        private double _timesincelastjumpslowdown = 0;

        private double jumpvariation_upper = 0.2;
        private double jumpvariation_lower = 0.1;
        private double _timetilljumpslowdown = 0;

        private double falldelay = 0;
        private double _timesincelastfallacc = 0;

        private double airresdelay = 0.01;
        private double _timesincelastairrescheck = 0;

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









        Rectangle paused;
        Rectangle menu;
        Rectangle resume;
        Rectangle play;
        Rectangle level_editer;
        Rectangle exit;
        Rectangle edit_menu;
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
            base.Initialize();
            _player.Width = 84;
            _player.Height = 168;
            pressed_x = false;
            pressed_y = false;
            if (!File.Exists("position_x.txt"))
                File.WriteAllText("position_x.txt", _player.X.ToString());
            if (!File.Exists("position_y.txt"))
                File.WriteAllText("position_y.txt", _player.Y.ToString());
            if (!File.Exists("notrightspeed.txt"))
                File.WriteAllText("notrightspeed.txt", _notrightspeed.ToString());
            if (!File.Exists("fallspeed.txt"))
                File.WriteAllText("fallspeed.txt", _fallspeed.ToString());
            
            _player.X = int.Parse(File.ReadAllText("position_x.txt"));
            _player.Y = int.Parse(File.ReadAllText("position_y.txt"));
            _notrightspeed = int.Parse(File.ReadAllText("notrightspeed.txt"));
            _fallspeed = int.Parse(File.ReadAllText("fallspeed.txt")); 
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
        #region button function
        static public void f_button(
            Rectangle dimentions,
            ref bool ispressed)
        {
            MouseState mouseState = Mouse.GetState();
            int mousepos_x = mouseState.X;
            int mousepos_y = mouseState.Y;
            if (mouseState.X > dimentions.X &
                mouseState.X < dimentions.X + dimentions.Width &
                mouseState.Y > dimentions.Y &
                mouseState.Y < dimentions.Y + dimentions.Height &
                mouseState.LeftButton == ButtonState.Pressed)
            {
                ispressed = true;
            }
        }
        #endregion
        #region slope check
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


        #endregion
        #region squishy function
        static public void _Squish(
            int horozontalSquish, 
            int verticalSquish, 
            ref Rectangle _objectSquished)
        {

            _objectSquished.Width += horozontalSquish;
            _objectSquished.Height -= verticalSquish;
            _objectSquished.X -= horozontalSquish / 2;
            _objectSquished.Y += verticalSquish;
        }
        #endregion
        #region collision detector function
        static public void _collide(
            ref Rectangle player, 
            int tile_size, 
            int[,] mapOfTiles, 
            ref int affected_varx, 
            ref int affected_vary,
            bool[] side_touching_wall)
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
                            if (mapOfTiles[y, x] == 1)
                            {
                                Rectangle tile_check = new Rectangle(x * tile_size, y * tile_size, tile_size, tile_size);
                                if (isgoingleft == true)
                                {
                                    #region upper left
                                    if (isgoingup == true)
                                    {
                                        if (tile_check.X + tile_size - relivant_rectangle.X <= affected_varx)
                                        {
                                            f_slope_check (
                                                tile_check.Y - relivant_rectangle.Y, 
                                                tile_size, 
                                                (tile_check.X + tile_size - relivant_rectangle.X) * slope, 
                                                0, player.Height, 
                                                mapOfTiles[y, x + 1], 
                                                ref x_restricter, ref side_touching_wall[0], 
                                                x_restricter < tile_check.X + tile_size, 
                                                tile_check.X + tile_size);                                            
                                        }
                                        if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                        {
                                            if (affected_varx != 0
                                                && slope != 0)
                                            {
                                                f_slope_check(
                                                    tile_check.X - relivant_rectangle.X,
                                                    tile_size,
                                                    (tile_check.Y + tile_size - relivant_rectangle.Y) / slope, 
                                                    0, player.Width,
                                                    mapOfTiles[y + 1, x],
                                                    ref y_restricter, ref side_touching_wall[2],
                                                    y_restricter < tile_check.Y + tile_size,
                                                    tile_check.Y + tile_size);
                                            }
                                        }
                                        if (affected_varx == 0)
                                        {                                            
                                            if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                            {
                                                if (mapOfTiles[y + 1, x] != 1)
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
                                            f_slope_check(
                                                tile_check.Y - relivant_rectangle.Y,
                                                tile_size,
                                                relivant_rectangle.Height + (tile_check.X + tile_size - relivant_rectangle.X) * slope,
                                                player.Height, 0,
                                                mapOfTiles[y, x + 1],
                                                ref x_restricter, ref side_touching_wall[0],
                                                x_restricter < tile_check.X + tile_size,
                                                tile_check.X + tile_size);
                                        }

                                        if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                        {
                                            if (affected_varx != 0
                                                && slope != 0)
                                            {
                                                f_slope_check(
                                                    tile_check.X - relivant_rectangle.X,
                                                    tile_size,
                                                    (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / -slope, 
                                                    0, player.Width,
                                                    mapOfTiles[y - 1, x],
                                                    ref y_restricter, ref side_touching_wall[3],
                                                    y_restricter > tile_check.Y,
                                                    tile_check.Y);
                                            }
                                        }
                                        if (affected_varx == 0)
                                        {
                                            if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                            {
                                                if (mapOfTiles[y - 1, x] != 1)
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
                                            f_slope_check(
                                                tile_check.Y - relivant_rectangle.Y,
                                                tile_size,
                                                (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope, 
                                                0, player.Height,
                                                mapOfTiles[y, x - 1],
                                                ref x_restricter, ref side_touching_wall[1],
                                                x_restricter > tile_check.X,
                                                tile_check.X);
                                        }
                                        if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                        {
                                            if (slope != 0)
                                            {
                                                f_slope_check(
                                                    tile_check.X - relivant_rectangle.X,
                                                    tile_size,
                                                    relivant_rectangle.Width + (tile_check.Y + tile_size - relivant_rectangle.Y) / slope,
                                                    player.Width, 0,
                                                    mapOfTiles[y + 1, x],
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
                                            f_slope_check(
                                                (tile_check.Y - relivant_rectangle.Y),
                                                tile_size,
                                                (relivant_rectangle.Height - (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope),
                                                player.Height, 0,
                                                mapOfTiles[y, x - 1],
                                                ref x_restricter, ref side_touching_wall[1],
                                                x_restricter > tile_check.X,
                                                tile_check.X);
                                        }
                                        if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                        {
                                            if (slope != 0)
                                            {
                                                f_slope_check(
                                                    tile_check.X - relivant_rectangle.X,
                                                    tile_size,
                                                    relivant_rectangle.Width - (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / slope,
                                                    player.Width, 0,
                                                    mapOfTiles[y - 1, x],
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
        #endregion

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.play
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("BasicShape");
            level = Content.Load<Texture2D>("room 1");
            pausescreen = Content.Load<Texture2D>("menu background");
            pointer = Content.Load<Texture2D>("pointer");
            bar = Content.Load<Texture2D>("bar");
            grid = Content.Load<Texture2D>("grid");

            //temporary

            menu_b = Content.Load<Texture2D>("menu");
            play_button = Content.Load<Texture2D>("play button");
            exit_button = Content.Load<Texture2D>("exit button");
            level_button = Content.Load<Texture2D>("level editer button");
            pause_text = Content.Load<Texture2D>("pause button");
            Menu_text = Content.Load<Texture2D>("menu button");
            resume_text = Content.Load<Texture2D>("resume button");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.C))
            {
                File.WriteAllText("position_x.txt", _player.X.ToString());
                File.WriteAllText("position_y.txt", _player.Y.ToString());
                File.WriteAllText("notrightspeed.txt", _notrightspeed.ToString());
                File.WriteAllText("fallspeed.txt", _fallspeed.ToString());
                Exit();
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                escape_is_pressed = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.M))
                M_is_pressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) & escape_is_pressed == false)
            {
                if (is_paused == true)
                    is_paused = false;
                else
                    is_paused = true;
                escape_is_pressed = true;
            }             
            if (Keyboard.GetState().IsKeyDown(Keys.M) & M_is_pressed == false)
            {
                if (is_in_menu == true)
                    is_in_menu = false;
                else
                    is_in_menu = true;
                M_is_pressed = true;
            }
            #region buttons
            bool pressed_resume = false;
            f_button(resume, ref pressed_resume);
            if (pressed_resume &
                is_paused)
            {
                is_paused = false;
            }
            bool pressed_menu = false;
            f_button(menu, ref pressed_menu);
            if (pressed_menu & 
                is_paused)
            {
                is_in_menu = true;
            }
            bool pressed_play = false;
            f_button(play, ref pressed_play);
            if (pressed_play &
                is_in_menu)
            {
                is_in_menu = false;
                is_paused = false;
            }
            bool pressed_exit = false;
            f_button(exit, ref pressed_exit);
            if (pressed_exit &
                is_in_menu &
                !is_editing)
            {
                File.WriteAllText("position_x.txt", _player.X.ToString());
                File.WriteAllText("position_y.txt", _player.Y.ToString());
                File.WriteAllText("notrightspeed.txt", _notrightspeed.ToString());
                File.WriteAllText("fallspeed.txt", _fallspeed.ToString());
                Exit();
            }
            bool pressed_editor = false;
            f_button(level_editer, ref pressed_editor);
            if (pressed_editor & 
                is_in_menu)
            {
                is_editing = true;
                is_in_menu = false;
            }

            #endregion
            #region LEVEL EDITOR
            if (is_editing)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.X) &
                    pressed_x == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            size_x -= 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            size_x -= 100;
                        }
                        else
                        {
                            size_x -= 1;
                        }
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            size_x += 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            size_x += 100;
                        }
                        else
                        {
                            size_x += 1;
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
                            size_y -= 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            size_y -= 100;
                        }
                        else
                        {
                            size_y -= 1;
                        }
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            size_y += 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        {
                            size_y += 100;
                        }
                        else
                        {
                            size_y += 1;
                        }
                    }
                    pressed_y = true;
                }
                if (edit_tilemap.Count == size_y)
                {
                    if (edit_tilemap[edit_tilemap.Count - 1].Count == size_x)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if ((int)((mousepos_y - 200) / 96) < size_y &
                                (int)(mousepos_x / 96) < size_x)
                                edit_tilemap[(int)((mousepos_y - 200) / 96)][(int)(mousepos_x / 96)] = 1;
                        }
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            if ((int)((mousepos_y - 200) / 96) < size_y &
                                (int)(mousepos_x / 96) < size_x)
                                edit_tilemap[(int)((mousepos_y - 200) / 96)][(int)(mousepos_x / 96)] = 0;
                        }
                    }
                }
            }
            #endregion
            //--------------------------------------------//
            //                                            //
            //                  MOVEMENT                  //
            //                                            //
            //--------------------------------------------//
            #region MOVEMENT update
            if (is_in_menu == false & is_paused == false & is_editing == false)
            {
                if (_timesincelastmove >= movedelay)
                {
                    _collide(ref _player, tilesize, tileMap, ref _notrightspeed, ref _fallspeed, iscoliding);
                    _player.X -= _notrightspeed;
                    _player.Y += _fallspeed;
                    _timesincelastmove = 0;
                    if (iscoliding[0] == true & _notrightspeed > 0)
                    {
                        _notrightspeed = 0;
                    }
                    if (iscoliding[1] == true & _notrightspeed < 0)
                    {
                        _notrightspeed = 0;
                    }
                    if (iscoliding[2] == true
                        | iscoliding[3] == true)
                    {
                        _fallspeed = 0;
                    }
                    if (iscoliding[3] == true)
                    {
                        _fallspeed = 0;
                    }
                    if (true)
                    {
                        aircheck[0] = false;
                        aircheck[1] = false;
                        aircheck[2] = false;
                        aircheck[3] = false;
                        int X = 1;
                        int Y = -1;
                        _collide(ref _player, tilesize, tileMap, ref X, ref Y, aircheck);
                        X = 1;
                        Y = 1;
                        _collide(ref _player, tilesize, tileMap, ref X, ref Y, aircheck);
                        X = -1;
                        Y = -1;
                        _collide(ref _player, tilesize, tileMap, ref X, ref Y, aircheck);
                        X = -1;
                        Y = 1;
                        _collide(ref _player, tilesize, tileMap, ref X, ref Y, aircheck);
                        iscoliding[0] = aircheck[0];
                        iscoliding[1] = aircheck[1];
                        iscoliding[2] = aircheck[2];
                        iscoliding[3] = aircheck[3];
                    }
                }
                #endregion
                #region camera
                r_level = new Rectangle(
                    0,
                    0,
                    (int)((tileMap.GetLength(1) - 2) * tilesize * zoom),
                    (int)((tileMap.GetLength(0) - 5) * tilesize * zoom));
                if (r_level.Height > graphics.PreferredBackBufferHeight)
                {
                    wiggleroom_y = (int)((r_level.Height - graphics.PreferredBackBufferHeight) * zoom);
                    if (r_level.Y - camera_moveTo_y < 0
                        & camera_moveTo_y > (int)((_player.Y - tilesize * 4 + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2)
                    {
                        camera_moveTo_y = (int)((_player.Y - tilesize * 4 + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2;
                    }
                    if (r_level.Y - camera_moveTo_y + r_level.Height > graphics.PreferredBackBufferHeight
                        & camera_moveTo_y < (int)((_player.Y - tilesize * 4 + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2)
                    {
                        camera_moveTo_y = (int)((_player.Y - tilesize * 4 + _player.Height / 2) * zoom) - graphics.PreferredBackBufferHeight / 2;
                    }
                    if (camera_moveTo_y < 0)
                    {
                        camera_moveTo_y = 0;
                    }
                    if (camera_moveTo_y > wiggleroom_y / 2)
                    {
                        camera_moveTo_y = wiggleroom_y / 2;
                    }
                }
                if (r_level.Width > graphics.PreferredBackBufferWidth)
                {
                    wiggleroom_x = (int)((r_level.Width - graphics.PreferredBackBufferWidth) * zoom);
                    if (r_level.X - camera_moveTo_x < 0
                        & camera_moveTo_x > (int)((_player.X - tilesize - _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2)
                    {
                        camera_moveTo_x = (int)((_player.X - tilesize - _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2;
                    }
                    if (r_level.X - camera_moveTo_x + r_level.Width > graphics.PreferredBackBufferWidth
                        & camera_moveTo_x < (int)((_player.X - tilesize - _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2)
                    {
                        camera_moveTo_x = (int)((_player.X - tilesize - _player.Width / 2) * zoom) - graphics.PreferredBackBufferWidth / 2;
                    }
                    if (camera_moveTo_x < 0)
                    {
                        camera_moveTo_x = 0;
                    }
                    if (camera_moveTo_x > wiggleroom_x / 2)
                    {
                        camera_moveTo_x = wiggleroom_x / 2;
                    }
                }
                // TODO: 
                if (false)
                {
                    if (((_player.X - tilesize) * zoom - camera_move_x) - (_player.Width * zoom / 2) < graphics.PreferredBackBufferWidth / 7 * 2)
                    {
                        camera_move_x += (int)((camera_moveTo_x - camera_move_x) / 8);
                    }
                    if (((_player.X - tilesize) * zoom - camera_move_x) - (_player.Width * zoom / 2) > graphics.PreferredBackBufferWidth / 7 * 4)
                    {
                        camera_move_x += (int)((camera_moveTo_x - camera_move_x) / 8);
                    }
                    if (((_player.Y) * zoom - camera_move_y) - (_player.Height * zoom) / 2 < graphics.PreferredBackBufferHeight / 7 * 2)
                    {
                        camera_move_y += (int)((camera_moveTo_y - camera_move_y) / 8);
                    }
                    if (((_player.Y) * zoom - camera_move_y) - (_player.Height * zoom) / 2 > graphics.PreferredBackBufferHeight / 7 * 4)
                    {
                        camera_move_y += (int)((camera_moveTo_y - camera_move_y) / 8);
                    }
                }
                camera_move_y += (int)((camera_moveTo_y - camera_move_y) / 16);
                camera_move_x += (int)((camera_moveTo_x - camera_move_x) / 16);

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
                if (_timesincelastacc > accdelay)
                {
                    if (is_crouching == false
                        & wall_climb == false)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.D)
                            & !Keyboard.GetState().IsKeyDown(Keys.A)
                        & (_notrightspeed > -speedcap)
                        & iscoliding[1] == false)
                        {
                            _notrightspeed -= speed;
                            _timesincelastacc = 0;
                            iscoliding[0] = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.A)
                            & !Keyboard.GetState().IsKeyDown(Keys.D)
                        & (_notrightspeed < speedcap)
                        & iscoliding[0] == false)
                        {
                            _notrightspeed += speed;
                            _timesincelastacc = 0;
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
                    _fallspeed -= jumpspeed;
                    _timetilljumpslowdown = 0;
                    //_Squish(jumpspeed, jumpspeed * 2, ref _player);
                    iscoliding[3] = false;
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        _notrightspeed -= 5;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        _notrightspeed += 5;
                    }
                    autojustpreventer = true;
                }
                if (wall_climb == true
                    & Keyboard.GetState().IsKeyDown(Keys.Space)
                    & autojustpreventer == false)
                {
                    _fallspeed -= (int)(jumpspeed * 0.75);
                    _timetilljumpslowdown = 0;
                    wall_climb = false;
                    if (iscoliding[0])
                    {
                        _notrightspeed -= (int)(jumpspeed * 0.8);
                    }
                    if (iscoliding[1])
                    {
                        _notrightspeed += (int)(jumpspeed * 0.8);
                    }
                    if (iscoliding[0]
                        & Keyboard.GetState().IsKeyDown(Keys.A)
                        & !Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        _notrightspeed += (int)(jumpspeed * 0.4);
                    }
                    if (iscoliding[1]
                        & Keyboard.GetState().IsKeyDown(Keys.D)
                        & !Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        _notrightspeed -= (int)(jumpspeed * 0.4);
                    }
                    if (iscoliding[0]
                        & Keyboard.GetState().IsKeyDown(Keys.D)
                        & !Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        _notrightspeed -= (int)(jumpspeed * 0.4);
                    }
                    if (iscoliding[1]
                        & Keyboard.GetState().IsKeyDown(Keys.A)
                        & !Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        _notrightspeed += (int)(jumpspeed * 0.4);
                    }
                    autojustpreventer = true;
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    autojustpreventer = false;
                }

                #endregion
                #region GRAVITY
                if (iscoliding[3] == false
                    & _fallspeed >= -4
                    & _fallspeed < fallcap1
                    & _timesincelastfallacc >= falldelay
                    & wall_climb == false)
                {
                    _fallspeed += gravspeed;
                    _timesincelastfallacc = 0;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S)
                    & _fallspeed >= -4
                    & _fallspeed < fallcap2
                    & _timesincelastfallacc >= falldelay
                    & iscoliding[3] == false
                    & wall_climb == false)
                {
                    _fallspeed += gravspeed;
                    _timesincelastfallacc = 0;
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.S)
                    & _fallspeed > fallcap1)
                {
                    _fallspeed -= gravspeed;
                }


                if (_fallspeed > fallcap2
                    & _timesincelastairrescheck < airresdelay)
                {
                    _fallspeed -= drag;
                    _timesincelastairrescheck = 0;
                }
                if ((!Keyboard.GetState().IsKeyDown(Keys.Space)
                    & _timesincelastjumpslowdown >= jumpslowdowndelay
                    & _timetilljumpslowdown >= jumpvariation_lower
                    || _timetilljumpslowdown >= jumpvariation_upper)
                    & _fallspeed < 0
                    & _timesincelastjumpslowdown >= jumpslowdowndelay)
                {
                    _fallspeed += gravspeed;
                    _timesincelastjumpslowdown = 0;
                }
                #endregion
                #region FRICTION
                if (is_crouching == true)
                {
                    _notrightspeed = 0;
                }
                if (_timesincelastfric >= fricdelay
                    && iscoliding[3] == true)
                {
                    if (((!Keyboard.GetState().IsKeyDown(Keys.A)
                    | Keyboard.GetState().IsKeyDown(Keys.D))
                    & _notrightspeed > 0)
                    | _notrightspeed > speedcap)
                    {
                        if (_notrightspeed >= friction)
                        {
                            _notrightspeed -= friction;
                        }
                        else
                        {
                            _notrightspeed = 0;
                        }
                        _timesincelastfric = 0;
                    }
                    if (((!Keyboard.GetState().IsKeyDown(Keys.D)
                    | Keyboard.GetState().IsKeyDown(Keys.A))
                    & _notrightspeed < 0)
                    | _notrightspeed < -speedcap)
                    {
                        if (_notrightspeed <= friction)
                        {
                            _notrightspeed += friction;
                        }
                        else
                        {
                            _notrightspeed = 0;
                        }
                        _timesincelastfric = 0;
                    }
                }
                else if (_timesincelastacc >= airresdelay)
                {
                    if (((!Keyboard.GetState().IsKeyDown(Keys.A)
                    | Keyboard.GetState().IsKeyDown(Keys.D))
                    & _notrightspeed > 0)
                    | _notrightspeed > speedcap)
                    {
                        if (_notrightspeed >= drag)
                        {
                            _notrightspeed -= drag;
                        }
                        else
                        {
                            _notrightspeed = 0;
                        }
                        _timesincelastfric = 0;
                    }
                    if (((!Keyboard.GetState().IsKeyDown(Keys.D)
                    | Keyboard.GetState().IsKeyDown(Keys.A))
                    & _notrightspeed < 0)
                    | _notrightspeed < -speedcap)
                    {
                        if (_notrightspeed <= drag)
                        {
                            _notrightspeed += drag;
                        }
                        else
                        {
                            _notrightspeed = 0;
                        }
                        _timesincelastfric = 0;
                    }
                }
                #endregion
                #region wall climb
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)
                    & (iscoliding[1]
                    || iscoliding[0]))
                {
                    wall_climb = true;
                    if (_fallspeed > 0)
                    {
                        _fallspeed = 0;
                    }
                }
                else
                {
                    wall_climb = false;
                }
                if (wall_climb == true
                    & Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (_fallspeed > climb_speed)
                    {
                        _fallspeed = climb_speed;
                    }
                }
                if (wall_climb == true
                    & Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (_fallspeed < slip_speed)
                    {
                        _fallspeed = slip_speed;
                    }

                }
                #endregion
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
                base.Update(gameTime);
                #region timers
                _timesincelastacc += gameTime.ElapsedGameTime.TotalSeconds;
                _timesincelastfric += gameTime.ElapsedGameTime.TotalSeconds;
                _timesincelastmove += gameTime.ElapsedGameTime.TotalSeconds;
                _timesincelastfallacc += gameTime.ElapsedGameTime.TotalSeconds;
                _timesincelastairrescheck += gameTime.ElapsedGameTime.TotalSeconds;
                _timesincelastjumpslowdown += gameTime.ElapsedGameTime.TotalSeconds;
                if (iscoliding[3] == false)
                {
                    _timetilljumpslowdown += gameTime.ElapsedGameTime.TotalSeconds;
                }
               
            }
            #endregion
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            spriteBatch.Begin();
            if (is_in_menu == false & 
                is_editing == false)
            {
                int x = (int)(_player.X / 12) * 12;
                int y = (int)(_player.Y / 12) * 12;
                spriteBatch.Draw(level, new Rectangle(
                    (int)((r_level.X * zoom) - camera_move_x),
                    (int)((r_level.Y * zoom) - camera_move_y),
                    r_level.Width,
                    r_level.Height),
                    Color.White);

                spriteBatch.Draw(Player, new Rectangle(
                    (int)((x - tilesize) * zoom - camera_move_x),
                    (int)((y - tilesize * 4) * zoom - camera_move_y),
                    (int)(_player.Width * zoom),
                    (int)(_player.Height * zoom)),
                    Color.White);
                SpriteFont font;
                font = Content.Load<SpriteFont>("bruh");
                if (debug)
                {
                    spriteBatch.DrawString(font, "x :" + _player.X + "  Y :" + (_player.Y + _player.Height), new Vector2(50, 50), Color.White);
                    spriteBatch.DrawString(font, "bot :" + iscoliding[3] + "  top :" + iscoliding[2], new Vector2(50, 70), Color.White);    
                    spriteBatch.DrawString(font, "lef :" + iscoliding[0] + "  rit :" + iscoliding[1], new Vector2(50, 90), Color.White);
                    spriteBatch.DrawString(font, "x-speed :" + _notrightspeed + "  Y-speed :" + _fallspeed, new Vector2(50, 110), Color.White);
                    spriteBatch.DrawString(font, "camra_move_x :" + camera_move_x + "  camera_move_y :" + camera_move_y, new Vector2(50, 130), Color.White);
                    spriteBatch.DrawString(font, "temp 1 :" + mousepos_x + "  temp 2 :" + mousepos_y, new Vector2(50, 150), Color.White);
                }
            }
            if (is_paused & 
                is_in_menu == false &
                is_editing == false)
            {
                spriteBatch.Draw(pausescreen, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.Draw(pause_text, paused, Color.White);
                spriteBatch.Draw(Menu_text, menu, Color.White);
                spriteBatch.Draw(resume_text, resume, Color.White);
            }
            if (is_in_menu &
                is_editing == false)
            {
                spriteBatch.Draw(menu_b, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black);
                spriteBatch.Draw(play_button, play, Color.White);
                spriteBatch.Draw(exit_button, exit, Color.White);
                spriteBatch.Draw(level_button, level_editer, Color.White);
            }
            if (is_editing)
            {
                spriteBatch.Draw(bar, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, 200), Color.White);
                spriteBatch.Draw(Menu_text, new Rectangle(40, 40, 228 * 2, 60 * 2), Color.White);
                if (edit_tilemap.Capacity < size_y)
                    edit_tilemap.Capacity = size_y;
                for (int i = edit_tilemap.Count; i < edit_tilemap.Capacity; i++)
                {
                    edit_tilemap.Add(new List<int>());
                }
                for (int y = 0; y < size_y; y ++)
                {
                    if (edit_tilemap[y].Capacity < size_x)
                        edit_tilemap[y].Capacity = size_x;
                    for (int i = edit_tilemap[y].Count; i < edit_tilemap[y].Capacity; i++)
                    {
                        edit_tilemap[y].Add(0);
                    }
                    for (int x = 0; x < size_x; x++)
                    {
                        if (edit_tilemap[y][x] == 0)
                        {
                            spriteBatch.Draw(grid, new Rectangle(x * 96, y * 96 + 200, 96, 96), Color.White);
                        }
                        if (edit_tilemap[y][x] == 1)
                        {
                            spriteBatch.Draw(Player, new Rectangle(x * 96, y * 96 + 200, 96, 96), Color.Black);
                        }
                    }
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    pressed_x = false;
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    pressed_y = false;
                }
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
