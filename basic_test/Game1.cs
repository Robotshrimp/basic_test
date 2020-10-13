using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace basic_test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        int[,] tileMap;
        
        int tileWidth = 96;
        int tileHeight = 96;
        bool is_crouching = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D Player;
        private Texture2D level;
        public Rectangle _player = new Rectangle(1440, 1056, 160, 80);
        private Rectangle level_rec = new Rectangle(0, 0, 0, 0);//(0, 300, 1024, 1024);
        private int _fallspeed = -30;
        private int _notrightspeed = -50;
        
        private double _timesincelastacc = 0;
        private double _timesincelastfric = 0;
        private double _timesincelastmove = 0;
        private double _timesincelastairrescheck = 0;
        private double _timesincelastfallacc = 0;
        private double _timetilljumpslowdown = 0;
        private double _timesincelastjumpslowdown = 0;
            //movement variables
        //horozontal
        private int speed = 4;
        private int friction = 5;
        private int speedcap = 12;
        //vertical
        private int fallcap1 = 25;
        private int fallcap2 = 40;
        private int drag = 3;
        private int gravspeed = 10;
        private int jumpspeed = 15;
        private bool autojustpreventer = false;
        //timer variables
        //vertical
        private double accdelay = 0.1;
        private double fricdelay = 0;
        private double jumpslowdowndelay = 0.10;
        private double jumpvariation = 0.30;
        //vertical
        private double falldelay = 0.05;
        private double airresdelay = 0.01;
        //main movement
        private double movedelay = 0;
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

            base.Initialize();
            _player.Width = 80;
            _player.Height = 160;
            tileMap = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1},
                {1,1,1,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1},
                {1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1},
                {1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
            
        }
        #region x check
        static public void f_slope_check(
            int till_rec_dif,
            int tilesize,
            double div_slope,
            int player_dem_neg,
            int player_dem_pos,
            int tilemap,
            ref int _res,
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
                        _res = change;
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
                    else if (isgoingup == false)
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
                    else if (isgoingup == false)
                    {
                        isrelivant = true;
                        relivant_rectangle = collision_detector[3];
                        y_restricter = relivant_rectangle.Y + relivant_rectangle.Height;
                    }
                    x_restricter = relivant_rectangle.X + relivant_rectangle.Width;
                }
            }
            int left_tile = relivant_rectangle.Left / tile_size;
            int right_tile = relivant_rectangle.Right / tile_size;
            int top_tile = relivant_rectangle.Top / tile_size;
            int bottom_tile = relivant_rectangle.Bottom / tile_size;
            if (isrelivant == true)
            {

                for (int b = 1; b <= 2; b++)
                {
                    left_tile = relivant_rectangle.Left / tile_size;
                    right_tile = relivant_rectangle.Right / tile_size;
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

                    if (relivant_rectangle.Right - 1 / tile_size == right_tile - 1)
                    {
                        right_tile -= 1;
                    }

                    double slope = 0;

                    if (affected_varx != 0)
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
                                    if (isgoingup == true)
                                    {
                                        if (tile_check.X + tile_size - relivant_rectangle.X <= affected_varx)
                                        {
                                            f_slope_check (
                                                tile_check.Y - relivant_rectangle.Y, 
                                                tile_size, 
                                                (tile_check.X + tile_size - relivant_rectangle.X) * slope, 
                                                0, 
                                                player.Height, 
                                                mapOfTiles[y, x + 1], 
                                                ref x_restricter, 
                                                ref side_touching_wall[0], 
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
                                                    0,
                                                    player.Width,
                                                    mapOfTiles[y + 1, x],
                                                    ref y_restricter,
                                                    ref side_touching_wall[2],
                                                    y_restricter < tile_check.Y + tile_size,
                                                    tile_check.Y + tile_size);
                                            }
                                        }
                                        if (affected_varx == 0)
                                        {
                                            if (mapOfTiles[y + 1, x] != 1)
                                            {
                                                if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
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
                                    else
                                    {
                                        if (tile_check.X + tile_size - relivant_rectangle.X <= affected_varx)
                                        {
                                            f_slope_check(
                                                tile_check.Y - relivant_rectangle.Y,
                                                tile_size,
                                                relivant_rectangle.Height + (tile_check.X + tile_size - relivant_rectangle.X) * slope,
                                                player.Height,
                                                0,
                                                mapOfTiles[y, x + 1],
                                                ref x_restricter,
                                                ref side_touching_wall[0],
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
                                                    0,
                                                    player.Width,
                                                    mapOfTiles[y - 1, x],
                                                    ref y_restricter,
                                                    ref side_touching_wall[3],
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
                                }
                                else
                                {

                                    if (isgoingup == true)
                                    {
                                        if (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X <= -affected_varx)
                                        {                                            
                                            f_slope_check(
                                                tile_check.Y - relivant_rectangle.Y,
                                                tile_size,
                                                (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope,
                                                0,
                                                player.Height,
                                                mapOfTiles[y, x - 1],
                                                ref x_restricter,
                                                ref side_touching_wall[1],
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
                                                    player.Width,
                                                    0,
                                                    mapOfTiles[y + 1, x],
                                                    ref y_restricter,
                                                    ref side_touching_wall[2],
                                                    y_restricter < tile_check.Y + tile_size,
                                                    tile_check.Y + tile_size);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X <= -affected_varx)
                                        {
                                            f_slope_check(
                                                (tile_check.Y - relivant_rectangle.Y),
                                                tile_size,
                                                (relivant_rectangle.Height - (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope),
                                                player.Height,
                                                0,
                                                mapOfTiles[y, x - 1],
                                                ref x_restricter,
                                                ref side_touching_wall[1],
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
                                                    player.Width,
                                                    0,
                                                    mapOfTiles[y - 1, x],
                                                    ref y_restricter,
                                                    ref side_touching_wall[3],
                                                    y_restricter > tile_check.Y,
                                                    tile_check.Y);
                                            }
                                        }
                                    }
                                }
                            }
                        }                        
                    }
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

                    
                }
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
            level = Content.Load<Texture2D>("test_level");
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //--------------------------------------------//
            //                                            //
            //                  MOVEMENT                  //
            //                                            //
            //--------------------------------------------//
            
            if (_timesincelastmove >= movedelay)
            {
                _collide(ref _player, tileHeight, tileMap, ref _notrightspeed, ref _fallspeed, iscoliding);   
                _player.X -= _notrightspeed;
                _player.Y += _fallspeed;
                _timesincelastmove = 0;
                if (iscoliding[0] == true
                | iscoliding[1] == true)
                {
                    _notrightspeed = 0;
                }
                if (iscoliding[2] == true
                    | iscoliding[3] == true)
                {
                    _fallspeed = 0;
                }
            }
            
            if(true)
            {
                aircheck[0] = false;
                aircheck[1] = false;
                aircheck[2] = false;
                aircheck[3] = false;
                int X = 1;
                int Y = -1;
                _collide(ref _player, tileHeight, tileMap, ref X, ref Y, aircheck);
                X = 1;
                Y = 1;
                _collide(ref _player, tileHeight, tileMap, ref X, ref Y, aircheck);
                X = -1;
                Y = -1;
                _collide(ref _player, tileHeight, tileMap, ref X, ref Y, aircheck);
                X = -1;
                Y = 1;
                _collide(ref _player, tileHeight, tileMap, ref X, ref Y, aircheck);
                iscoliding[0] = aircheck[0];
                iscoliding[1] = aircheck[1];
                iscoliding[2] = aircheck[2];
                iscoliding[3] = aircheck[3];
            }



            if (_player.Height > 160)
            {
                _player.Height -= 4;
                _player.Y += 4;
            }
            if (_player.Width < 80)
            {
                _player.Width += 4;
                _player.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S)
                & iscoliding[3])
            {
                if (is_crouching == false)
                {
                    _player.X -= (100 - _player.Width)/2 ;
                    _player.Y += _player.Height - 80;
                }
                is_crouching = true;
                _player.Height = 80;
                _player.Width = 100;
            }
            else
            {
                if (_player.Height < 160)
                {
                    _player.Height += 8;
                    _player.Y -= 8;
                }
                if (_player.Width > 80)
                {
                    _player.Width -= 8;
                    _player.X += 4;
                }
                is_crouching = false;
            }

            #region COLLISION
            /*
            if (_player.Bottom > 2000)
            {
                _isgrounded = true;
                if (_fallspeed > 17)
                {
                    _Squish(_fallspeed, _fallspeed * 2, ref _player);
                }
                _fallspeed = 0;
            }
            else
            {
                _isgrounded = false;
            }
            if (_player.Intersects(level_rec))
            {
                _isgrounded = true;
                _fallspeed = 0;
            }
            /*
            int left_tile = _player.Left / tileHeight;
            int right_tile = _player.Right / tileHeight;
            int top_tile = _player.Top / tileHeight;
            int bottom_tile = _player.Bottom / tileHeight;

            if (left_tile < 0)
            {
                left_tile = 0;
            }
            if (right_tile > tileMap.GetLength(1))
            {
                right_tile = tileMap.GetLength(1);
            }
            if (top_tile < 0)
            {
                top_tile = 0;
            }
            if (bottom_tile > tileMap.GetLength(0))
            {
                bottom_tile = tileMap.GetLength(0);
            }

            bool any_collision = false;
            for (int i = left_tile - 1; i <= right_tile; i++)
            {
                for (int j = top_tile - 1; j <= bottom_tile; j++)
                {
                    if (tileMap[i,j] == 1)
                    {
                        any_collision = true;

                    }
                }
            }
            */
            #endregion
            #region HOROZONTAL MOVEMENT
            if (_timesincelastacc > accdelay)
            {
                if (is_crouching == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D)
                    & (_notrightspeed > -speedcap)
                    & iscoliding[1] == false)
                    {
                        _notrightspeed -= speed;
                        _timesincelastacc = 0;
                        iscoliding[0] = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A)
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
                    _notrightspeed -= 10;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _notrightspeed += 10;
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
                & _timesincelastfallacc >= falldelay)
            {
                _fallspeed += gravspeed;
                _timesincelastfallacc = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)
                & _fallspeed >= -4
                & _fallspeed < fallcap2
                & _timesincelastfallacc >= falldelay
                & iscoliding[3] == false)
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
                || _timetilljumpslowdown >= jumpvariation)
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
                if((!Keyboard.GetState().IsKeyDown(Keys.A)
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
                if ((!Keyboard.GetState().IsKeyDown(Keys.D)
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
                if ((!Keyboard.GetState().IsKeyDown(Keys.A)
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
                if ((!Keyboard.GetState().IsKeyDown(Keys.D)
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
            
            //--------------------------------------------//
            //                                            //
            //               JUMP PREVENTER               //
            //                                            //
            //--------------------------------------------//
            

            

            // TODO: Add your update logic here

            base.Update(gameTime);
            //--------------------------------------------//
            //                                            //
            //                   TIMERS                   //
            //                                            //
            //--------------------------------------------//
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            #region

            #endregion
            spriteBatch.Draw(Player, _player, Color.Black);
            spriteBatch.Draw(level, new Rectangle(0, 0, (tileMap.GetLength(1) - 3) * tileHeight, (tileMap.GetLength(0) - 2) * tileHeight), Color.White);
            //spriteBatch.Draw(Player, level_rec, Color.Black);
            if (false)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    for (int x = 0; x < tileMap.GetLength(1); x++)
                    {

                        spriteBatch.Draw(
                            Player,
                            new Vector2(x * tileWidth, y * tileHeight),
                            Color.Black);
                    }
                }
            }
            spriteBatch.End();

            // TODO: Add your drawing code here
            graphics.IsFullScreen = true;
            base.Draw(gameTime);

        }
    }
}
