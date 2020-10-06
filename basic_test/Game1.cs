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
        public Rectangle _player = new Rectangle(576, 480, 96, 48);
        private Rectangle level_rec = new Rectangle(0, 0, 0, 0);//(0, 300, 1024, 1024);
        private int _fallspeed = 10;
        private int _notrightspeed = 10;
        
        private double _timesincelastacc = 0;
        private double _timesincelastfric = 0;
        private double _timesincelastmove = 0;
        private double _timesincelastairrescheck = 0;
        private double _timesincelastfallacc = 0;
        private double _timetilljumpslowdown = 0;
        private double _timesincelastjumpslowdown = 0;
            //movement variables
        //horozontal
        private int speed = 6;
        private int friction = 3;
        private int speedcap = 18;
        private int midaircap = 12;
        //vertical
        private int fallcap1 = 36;
        private int fallcap2 = 48;
        private int drag = 1;
        private int gravspeed = 6;
        private int jumpspeed = 24;
        //timer variables
        //vertical
        private double accdelay = 0.1;
        private double fricdelay = 0;
        private double jumpslowdowndelay = 0.07;
        private double jumpvariation = 0.15;
        //vertical
        private double falldelay = 0.05;
        private double airresdelay = 0.1;
        //main movement
        private double movedelay = 0;
        private bool[] iscoliding =
        {
            false,
            false,
            false,
            false,
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
            _player.Width = 96;
            _player.Height = 192;
            tileMap = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1},
                {1,1,1,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1},
                {1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1},
                {1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1},
                {1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,1},
                {1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1},
                {1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1},
                {1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            };
            
        }
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
                    }
                    else if (isgoingup == false)
                    {
                        isrelivant = true;
                        relivant_rectangle = collision_detector[1];
                        y_restricter = relivant_rectangle.Y + relivant_rectangle.Height;
                    }
                }
                else
                {
                    if (isgoingup == true)
                    {
                        isrelivant = true;
                        relivant_rectangle = collision_detector[2];
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
                                            if (tile_check.Y - relivant_rectangle.Y >= (tile_check.X + tile_size - relivant_rectangle.X) * slope
                                                && tile_check.Y - relivant_rectangle.Y + tile_size <= (tile_check.X + tile_size - relivant_rectangle.X) * slope + player.Height)
                                            {
                                                if (mapOfTiles[y, x + 1] != 1)
                                                {
                                                    if (x_restricter < tile_check.X + tile_size)
                                                    {
                                                        x_restricter = tile_check.X + tile_size;
                                                        side_touching_wall[0] = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                        {
                                            if (affected_varx != 0
                                                && slope != 0
                                                && tile_check.X - relivant_rectangle.X + tile_size >= (tile_check.Y + tile_size - relivant_rectangle.Y) / slope
                                                && tile_check.X - relivant_rectangle.X <= tile_check.Y + tile_size - relivant_rectangle.Y / slope + player.Width)
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

                                        if (affected_varx == 0)
                                        {
                                            if (y_restricter < tile_check.Y + tile_size)
                                            {
                                                y_restricter = tile_check.Y + tile_size;
                                                side_touching_wall[2] = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (tile_check.X + tile_size - relivant_rectangle.X <= affected_varx)
                                        {
                                            if (tile_check.Y - relivant_rectangle.Y + tile_size >= relivant_rectangle.Height + (tile_check.X + tile_size - relivant_rectangle.X) * slope - player.Height
                                                && tile_check.Y - relivant_rectangle.Y <= relivant_rectangle.Height + (tile_check.X + tile_size - relivant_rectangle.X) * slope + player.Height)
                                            {
                                                if (mapOfTiles[y, x + 1] != 1)
                                                {
                                                    if (x_restricter < tile_check.X + tile_size)
                                                    {
                                                        x_restricter = tile_check.X + tile_size;
                                                        side_touching_wall[0] = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                        {
                                            if (affected_varx != 0 &&
                                                slope != 0
                                                && tile_check.X - relivant_rectangle.X + tile_size >= relivant_rectangle.Y + (relivant_rectangle.Height - tile_check.Y) / -slope
                                                && tile_check.X - relivant_rectangle.X <= relivant_rectangle.Y + (relivant_rectangle.Height - tile_check.Y) / -slope + player.Width)
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
                                        if (affected_varx == 0)
                                        {
                                            if (y_restricter > tile_check.Y)
                                            {
                                                y_restricter = tile_check.Y;
                                                side_touching_wall[3] = true;
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
                                            if (tile_check.Y - relivant_rectangle.Y + tile_size >= (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope
                                                && tile_check.Y - relivant_rectangle.Y <= (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope + player.Height)
                                            {

                                                if (mapOfTiles[y, x - 1] != 1)
                                                {
                                                    if (x_restricter > tile_check.X)
                                                    {
                                                        x_restricter = tile_check.X;
                                                        side_touching_wall[1] = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (tile_check.Y + tile_size - relivant_rectangle.Y <= -affected_vary)
                                        {
                                            if (slope != 0
                                                && tile_check.X - relivant_rectangle.X <= relivant_rectangle.Width + (tile_check.Y + tile_size - relivant_rectangle.Y) / slope
                                                && tile_check.X - relivant_rectangle.X + tile_size >= relivant_rectangle.Width + (tile_check.Y + tile_size - relivant_rectangle.Y) / slope - player.Width)
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
                                    else
                                    {
                                        // debuged?
                                        if (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X <= -affected_varx)
                                        {
                                            if (tile_check.Y - relivant_rectangle.Y <= relivant_rectangle.Height - (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope
                                                && tile_check.Y - relivant_rectangle.Y + tile_size >= relivant_rectangle.Height - (relivant_rectangle.X + relivant_rectangle.Width - tile_check.X) * slope - player.Height)
                                            {
                                                if (mapOfTiles[y, x - 1] != 1)
                                                {
                                                    if (x_restricter > tile_check.X)
                                                    {
                                                        x_restricter = tile_check.X;
                                                        side_touching_wall[1] = true;
                                                    }
                                                }
                                            }
                                        }
                                        // debuged
                                        if (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y <= affected_vary)
                                        {
                                            if (slope != 0
                                                && tile_check.X - relivant_rectangle.X <= relivant_rectangle.Width - (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / slope
                                                && tile_check.X - relivant_rectangle.X + tile_size >= relivant_rectangle.Width - (relivant_rectangle.Y + relivant_rectangle.Height - tile_check.Y) / slope - player.Width)
                                            {

                                                if (mapOfTiles[
                                                    y - 1, x] != 1)
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
                            }
                        }                        
                    }
                    

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
                        relivant_rectangle.Height = relivant_rectangle.Y + relivant_rectangle.Height - x_restricter;
                        relivant_rectangle.Y = x_restricter;
                    }
                    else
                    {
                        relivant_rectangle.Height = y_restricter - relivant_rectangle.Y;
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
            int toptile = _player.Top / tileHeight;
            int bottomtile = _player.Bottom / tileHeight;
            int lefttile = _player.Left / tileHeight;
            int righttile = _player.Right / tileHeight;

            if (lefttile < 0)
            {
                lefttile = 0;
            }
            if (righttile > tileMap.GetLength(1))
            {
                righttile = tileMap.GetLength(1);
            }
            if (toptile < 0)
            {
                toptile = 0;
            }
            if (bottomtile > tileMap.GetLength(0))
            {
                bottomtile = tileMap.GetLength(0);
            }

            if (tileMap[bottomtile + 1, lefttile] == 0
                && tileMap[bottomtile + 1, righttile] == 0)
            {
                iscoliding[3] = false;
            }
            if (tileMap[toptile - 1, lefttile] == 0
                && tileMap[toptile - 1, righttile] == 0)
            {
                iscoliding[2] = false;
            }
            if (tileMap[bottomtile, lefttile - 1] == 0
                && tileMap[toptile, lefttile - 1] == 0)
            {
                iscoliding[0] = false;
            }
            if (tileMap[bottomtile, righttile + 1] == 0
                && tileMap[toptile, righttile + 1] == 0)
            {
                iscoliding[1] = false;
            }
            if (_player.Height > 192)
            {
                _player.Height -= 4;
                _player.Y += 4;
            }
            if (_player.Width < 96)
            {
                _player.Width += 4;
                _player.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S)
                & iscoliding[3])
            {
                if (is_crouching == false)
                {
                    _player.X -= (120 - _player.Width)/2 ;
                    _player.Y += _player.Height - 96;
                }
                is_crouching = true;
                _player.Height = 96;
                _player.Width = 120;
            }
            else
            {
                if (_player.Height < 192)
                {
                    _player.Height += 8;
                    _player.Y -= 8;
                }
                if (_player.Width > 96)
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
                if (iscoliding[3] == true
                & is_crouching == false)
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
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D)
                    & (_notrightspeed > -midaircap)
                    & iscoliding[1] == false)
                    {
                        _notrightspeed -= speed / 2;
                        _timesincelastacc = 0;
                        iscoliding[0] = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A)
                    & (_notrightspeed < midaircap)
                    & iscoliding[0] == false)
                    {
                        _notrightspeed += speed / 2;
                        _timesincelastacc = 0;
                        iscoliding[1] = false;
                    }
                }
            }
            #endregion
            #region JUMP
            if (iscoliding[3] == true
                & Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _fallspeed -= jumpspeed;
                _timetilljumpslowdown = 0;
                _player.Height += jumpspeed * 2;
                _player.Y -= jumpspeed * 2;
                _player.Width -= jumpspeed;
                _player.X += jumpspeed / 2;
                iscoliding[3] = false;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _notrightspeed -= 9;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _notrightspeed += 9;
                }
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
                | _timetilljumpslowdown >= jumpvariation)
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
            if (_timesincelastfric >= fricdelay)
            {
                if((!Keyboard.GetState().IsKeyDown(Keys.A)
                & _notrightspeed > 0)
                | _notrightspeed > speedcap)
                {
                    _notrightspeed -= friction;
                    _timesincelastfric = 0;
                }
                if ((!Keyboard.GetState().IsKeyDown(Keys.D)
                & _notrightspeed < 0)
                | _notrightspeed < -speedcap)
                {
                    _notrightspeed += friction;
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
            if (iscoliding[3] == true)
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
            spriteBatch.Draw(level, new Rectangle(0, 0, tileMap.GetLength(1) * tileHeight, tileMap.GetLength(0) * tileHeight), Color.White);
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
