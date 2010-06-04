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

        float time_before = 0.0f;
        float time_now = 0.0f;

        GameObject player1;
        
        GameObject [] player1_weapon1;
        const int max_player1_weapon1 = 10;

        GameObject[] player1_weapon2;
        const int max_player1_weapon2 = 5;

        const int player1_weapon2_mass = 100;

        SpriteFont font;

        KeyboardState keyboardState_before = Keyboard.GetState();


        Rectangle window_frame;

        int Window_Height = 720;
        int Window_Width = 1280;

        int player1_initial_position_X ;
        int player1_initial_position_Y ;

        Song BGM;


        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public double theta_test;

        GameObject planet;
        GameObject ball;





        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            graphics.PreferredBackBufferHeight = Window_Height;
            graphics.PreferredBackBufferWidth = Window_Width;
            
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "2D Experiment";

            player1_initial_position_X = 120;
            player1_initial_position_Y = 120;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player1 = new GameObject(Content.Load<Texture2D>("Sprites\\120-staryu"));
            player1.position = new Vector2(player1_initial_position_X, player1_initial_position_Y);
            player1.scale = 1.0f;

            player1_weapon1 = new GameObject[max_player1_weapon1];
            for (int i = 0; i < max_player1_weapon1; i++)
            {
                player1_weapon1[i] = new GameObject(Content.Load<Texture2D>("Sprites\\bubblemod"));

            }


            player1_weapon2 = new GameObject[max_player1_weapon2];
            for (int i = 0; i < max_player1_weapon2; i++)
            {
                player1_weapon2[i] = new GameObject(Content.Load<Texture2D>("Sprites\\gravity_weapon"));
                player1_weapon2[i].mass = player1_weapon2_mass;

            }


            planet = new GameObject(Content.Load<Texture2D>("Sprites\\planet"));
            planet.position.X = 600;
            planet.position.Y = 300;
            planet.mass = 100;
           

            ball = new GameObject(Content.Load<Texture2D>("Sprites\\pokeball1"));
            ball.position.X = 550;
            ball.position.Y = 320;
            ball.mass = 100;
                  



            font = Content.Load<SpriteFont>("Fonts\\SpriteFont1");

            window_frame = new Rectangle(
                0,
                0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height
                );


            BGM = Content.Load<Song>("Audio\\130-cycling");
            //MediaPlayer.Play(BGM);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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



            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            
            Controls();
            Update_Weapons();

            time_now = (float)gameTime.TotalGameTime.TotalMilliseconds;

            Physics(time_before, time_now );


            time_before  = (float)gameTime.TotalGameTime.TotalMilliseconds;


            
            


            keyboardState_before = Keyboard.GetState();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // For FPS
            frameCounter++;
            string fps = string.Format("FPS: {0}", frameRate);




            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            spriteBatch.Draw(player1.sprite, player1.position, Color.White);


            spriteBatch.Draw(planet.sprite, planet.position, Color.White);
            spriteBatch.Draw(ball.sprite, ball.position, Color.White);
            

            foreach (GameObject shot in player1_weapon1)
            {
                if (shot.alive)
                {
                    spriteBatch.Draw(shot.sprite, shot.position, Color.White);
                }

            }


            
            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    spriteBatch.Draw(shot.sprite, shot.position, Color.White);
                }

            }




            spriteBatch.DrawString(
                font,
                "Position: " + player1.position.ToString() + "           time_before: " + time_before.ToString(),
                new Vector2(10, 10),
                Color.Yellow
                );

            spriteBatch.DrawString(
                font,
                "Velocity: " + player1.velocity.ToString() + "                  time_now: " + time_now.ToString(),
                new Vector2(10, 30),
                Color.Yellow
                );

            spriteBatch.DrawString(
                font,
                "Acceleration: " + player1.acceleration.ToString(),
                new Vector2(10, 50),
                Color.Yellow
                );




            spriteBatch.DrawString(
                font,
                "Ball Position: " + ball.position.ToString(),
                new Vector2(10,70),
                Color.Yellow
                );

            spriteBatch.DrawString(
                font,
                "Ball Velocity: " + ball.velocity.ToString(),
                new Vector2(10, 90),
                Color.Yellow
                );

            spriteBatch.DrawString(
                font,
                "Ball Acceleration: " + ball.acceleration.ToString(),
                new Vector2(10, 110),
                Color.Yellow
                );


            spriteBatch.DrawString(
                font,
                "Planet Position: " + planet.position.ToString(),
                new Vector2(10, 130),
                Color.Yellow
                );

            spriteBatch.DrawString(
                font,
                "Planet Velocity: " + planet.velocity.ToString(),
                new Vector2(10, 150),
                Color.Yellow
                );

            spriteBatch.DrawString(
                font,
                "Planet Acceleration: " + planet.acceleration.ToString(),
                new Vector2(10, 170),
                Color.Yellow
                );

         



            spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }




        // Extra functions




        private void Controls()
        {


            // CONTROLS


            KeyboardState keyboardState = Keyboard.GetState();


            int acceleration_increment = 1;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player1.acceleration.X -= acceleration_increment;

                if (player1.acceleration.X > 0)
                {
                    player1.acceleration.X = 0;
                }
                
                
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                player1.acceleration.X += acceleration_increment;

                if (player1.acceleration.X < 0)
                {
                    player1.acceleration.X = 0;
                }

            }


            if (keyboardState.IsKeyDown(Keys.Up))
            {
                player1.acceleration.Y -= acceleration_increment;

                if (player1.acceleration.Y > 0)
                {
                    player1.acceleration.Y = 0;
                }

            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                player1.acceleration.Y += acceleration_increment;

                if (player1.acceleration.Y < 0)
                {
                    player1.acceleration.Y = 0;
                }
            }


            // Kill acceleration if not pressed

            if (keyboardState.IsKeyUp(Keys.Left) && keyboardState_before.IsKeyDown(Keys.Left))
            {
                player1.acceleration.X = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Right) && keyboardState_before.IsKeyDown(Keys.Right))
            {
                player1.acceleration.X = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Up) && keyboardState_before.IsKeyDown(Keys.Up))
            {
                player1.acceleration.Y = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Down) && keyboardState_before.IsKeyDown(Keys.Down))
            {
                player1.acceleration.Y = 0;
            }


            // RESET POSITION
            if (keyboardState.IsKeyDown(Keys.R))
            {
                player1.position.X = player1_initial_position_X;
                player1.position.Y = player1_initial_position_Y;

            }

            // WEAPONS


            if (keyboardState.IsKeyDown(Keys.Space) && keyboardState_before.IsKeyUp(Keys.Space))
            {
                Fire_Weapons("1");
            }

            if (keyboardState.IsKeyDown(Keys.G) && keyboardState_before.IsKeyUp(Keys.G))
            {
                Fire_Weapons("2");
            }

            

        }


        private void Physics(float time_before, float time_now)
        {
            float time_diff = time_now - time_before;

            // Update velocity and position of player
            player1.velocity += player1.acceleration * (time_diff*0.01f);
            player1.position += player1.velocity * (time_diff*0.01f);



            // Friction on player

            float friction = 0.025f;  // in percentage

            player1.velocity.X = player1.velocity.X * (1 - friction);

            player1.velocity.Y = player1.velocity.Y * (1 - friction);

            if (Math.Abs(player1.velocity.X) < 1)
            {
                player1.velocity.X = 0;
            }

            if (Math.Abs(player1.velocity.Y) < 1)
            {
                player1.velocity.Y = 0;
            }



            // Weapon related physics

                /// weapon2 friction

            float friction_weapon2 = 0.01f;

            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    //if (Math.Abs(shot.velocity.X) > 4)
                    //{
                    //    shot.velocity.X = shot.velocity.X * (1 - friction);
                    //}

                    //if (Math.Abs(shot.velocity.Y) > 4)
                    //{
                    //    shot.velocity.Y = shot.velocity.Y * (1 - friction);
                    //}

                    shot.velocity.X = shot.velocity.X * (1 - friction_weapon2);
                    shot.velocity.Y = shot.velocity.Y * (1 - friction_weapon2);

                    if (Math.Abs(shot.velocity.X) < 1)
                    {
                        shot.velocity.X = 0;
                    }

                    if (Math.Abs(shot.velocity.Y) < 1)
                    {
                        shot.velocity.Y = 0;
                    }


                }
            }


                /// Gravity relative to weapon2
            
            
            for (int i = 0; i < max_player1_weapon2; i++)
            {
                if(player1_weapon2[i].alive)
                {

                    // check distance between active weapon2 sprites
                    for (int j = 0; j < max_player1_weapon2; j++)
                    {
                        if(player1_weapon2[j].alive && i!=j)
                        {

                            Gravity( player1_weapon2[i],  player1_weapon2[j]);
                            
                        }
                        
                    }

                    
                }
         
            }



            // Update velocity and position of weapon2
            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    shot.velocity += shot.acceleration * (time_diff * 0.01f);
                    shot.position += shot.velocity * (time_diff * 0.01f);                    

                }
            }



           

            // planet and ball physics
            Gravity(ball, planet);

            planet.velocity += planet.acceleration * (time_diff * 0.01f);
            planet.position += planet.velocity * (time_diff * 0.01f);

            ball.velocity += ball.acceleration * (time_diff * 0.01f);
            ball.position += ball.velocity * (time_diff * 0.01f);                    



            

        }



        private void Fire_Weapons(string weapon_id)
        {

            float Weapon1_velocity = 10.0f;
            float Weapon2_velocity = 5.0f;
                     
            
            switch (weapon_id)
            {

                case "1":

                    foreach (GameObject shot in player1_weapon1)
                    {
                        if (!shot.alive)
                        {
                            shot.alive = true;
                            shot.position = player1.position;
                            shot.position.Y -= 4;
                            shot.velocity = new Vector2(Weapon1_velocity, 0);

                            return;
                        }
                    }
                    break;


                case "2":

                    foreach (GameObject shot in player1_weapon2)
                    {
                        if (!shot.alive)
                        {
                            shot.alive = true;
                            shot.position = player1.position;
                            shot.position.Y += 4;
                            shot.velocity = new Vector2(Weapon2_velocity, 0);

                            return;
                        }
                    }
                    break;


                default: break;

            }


        }


        private void Update_Weapons()
        {
                        
            foreach (GameObject shot in player1_weapon1)
            {
                if (shot.alive)
                {
                    shot.position += shot.velocity;

                    if (!window_frame.Contains(new Point((int)shot.position.X, (int)shot.position.Y)))
                    {
                        shot.alive = false;
                        shot.acceleration.X = 0;
                        shot.acceleration.Y = 0;
                        continue;
                    }
                }
            }

            
            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    shot.position += shot.velocity;

                    if (!window_frame.Contains(new Point((int)shot.position.X, (int)shot.position.Y)))
                    {
                        shot.alive = false;
                        shot.acceleration.X = 0;
                        shot.acceleration.Y = 0;
                        continue;
                    }
                }
            }
                      
        }




        private void Gravity(GameObject m1, GameObject m2)
        {
            // Calculates and applies appropriate force -> accelration to both masses relative to each other
            //// F = G*m1*m2 / r^2      
            /// F= ma, a = F/m

            const float G = 2.0f; // Gravitational constant
            double F = 0;
            float r = Vector2.Distance( (m1.position+m1.center), (m2.position+m2.center));

            if (r < 1)
            {
                r = 1;
            }

            F = (G * m1.mass * m2.mass) / Math.Pow(r, 2);

            Vector2 delta = (m2.position + m2.center) - (m1.position + m1.center);
            Vector2 delta_normalized = Vector2.Normalize(delta);
     
            

            // Scale caclulated Force int 2d Vector by theta using matrix

            
            Vector2 Applied_Force = delta_normalized * (int)F;
                        
            
            // Apply Force to m1 using a = F/m
            Vector2 force_acceleration_m1 = Applied_Force / m1.mass;
            m1.acceleration += force_acceleration_m1;

            
            // Apply Force to m2 using a = F/m
            Vector2 force_acceleration_m2 = Applied_Force / m2.mass;
            m2.acceleration -= force_acceleration_m2;
            
            // Clamp aceleration values

            float acceleration_clamp = 0.5f;

            m1.acceleration.X = MathHelper.Clamp(m1.acceleration.X, -acceleration_clamp, acceleration_clamp);
            m1.acceleration.Y = MathHelper.Clamp(m1.acceleration.Y, -acceleration_clamp, acceleration_clamp);

            m2.acceleration.X = MathHelper.Clamp(m2.acceleration.X, -acceleration_clamp, acceleration_clamp);
            m2.acceleration.Y = MathHelper.Clamp(m2.acceleration.Y, -acceleration_clamp, acceleration_clamp);



        }


                   
        
    }
}
