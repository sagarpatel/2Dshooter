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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;



namespace _2Dshooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
                
        KeyboardState keyboardState;
        KeyboardState keyboardState_before = Keyboard.GetState();
        GamePadState gamePadState;
        GamePadState gamePadState_before = GamePad.GetState(PlayerIndex.One);


        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;

          
        Random random = new Random();

      
        int Window_Height = 720;
        int Window_Width = 1280;

 
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //videoPlayer = new VideoPlayer();

            
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

            graphics.PreferredBackBufferHeight = Window_Height;
            graphics.PreferredBackBufferWidth = Window_Width;
            
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "2D Experiment";



            base.Initialize();
        }

  
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            startScreen = new StartScreen(this, spriteBatch, Content.Load<SpriteFont>("Fonts\\SpriteFont1"), Content.Load<Texture2D>("alienmetal"));
            Components.Add(startScreen);
            startScreen.Hide();

            actionScreen = new ActionScreen(this, spriteBatch, Content.Load<Texture2D>("greenmetal"), graphics);
            Components.Add(actionScreen);
            actionScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

           
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        protected override void Update(GameTime gameTime)
        {

            // To check FPS
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }


            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);


           // videoPlayer.Play(myVideoFile);



            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            // Exit Game if ESC is pressed
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }


            //Toggle fullscreen

            if (keyboardState.IsKeyDown(Keys.T) && keyboardState_before.IsKeyUp(Keys.T))
            {
                graphics.ToggleFullScreen();
            }

            if (gamePadState.DPad.Up == ButtonState.Pressed && gamePadState_before.DPad.Up == ButtonState.Released)
            {
                graphics.ToggleFullScreen();
            }




            if (activeScreen == startScreen)
            {
                //if (CheckKey(Keys.Enter))
                if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)  
                {
                    if (startScreen.SelectedIndex == 0)
                    {
                        activeScreen.Hide();
                        activeScreen = actionScreen;
                        activeScreen.Show();
                    }
                    if (startScreen.SelectedIndex == 1)
                    {
                        this.Exit();
                    }
                }
            }
         


            base.Update(gameTime);
            keyboardState_before = keyboardState;
            gamePadState_before = gamePadState;
        }

      
        protected override void Draw(GameTime gameTime)
        {
          
            base.Draw(gameTime);
  
            
        }

                                
        
    }
}