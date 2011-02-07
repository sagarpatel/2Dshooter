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
    class ActionScreen : GameScreen
    {
        GraphicsDeviceManager graphics;

        Texture2D image;
        Rectangle imageRectangle;

        KeyboardState keyboardState;
        KeyboardState keyboardState_before = Keyboard.GetState();

        GamePadState gamePadState ;
        GamePadState gamePadState_before = GamePad.GetState(PlayerIndex.One);
       




       // Video myVideoFile;
       // VideoPlayer videoPlayer;

        float time_before = 0.0f;
        float time_now = 0.0f;
        float time_diff = 0.0f;
        float time_stretch = 0.01f;
        float G = 1.0f;

        GameObject player1;
        int player1_score = 0;
        bool player1_exploded = false;

        GameObject[] player1_weapon1;
        const int max_player1_weapon1 = 10;
        const float player1_weapon1_initial_velocity_X = 40.0f;


        GameObject[] player1_weapon2;
        const int max_player1_weapon2 = 100;
        const float player1_weapon2_initial_velocity_X = 20.0f;
        float player1_weapon2_maxage = 10000;

        GameObject[] enemies1;
        const int max_enemies1 = 10;
        int min_enemies1_velocity = 0;
        int max_enemies1_velocity = 0;

        GameObject[] enemies2;
        const int max_enemies2 = 20;
        const int enemies2_perspawn = 2;
        const float enemies2_velocity_scalar = 10.0f;


        GameObject Feynman;


        Random random = new Random();




        SpriteFont font;


        Rectangle window_frame;

        int Window_Height = 720;
        int Window_Width = 1280;

        int player1_initial_position_X = 300;
        int player1_initial_position_Y = 300;

        Song BGM;
        SoundEffect zubat_scream;
        SoundEffect golbat_scream;


        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        //GameObject planet;
        GameObject ball;

        
        // Create instance of Particle Engine

        ParticleEngine PE1;
        ParticleEngine PE2;
        ParticleEngine PE3;

        // Shaders are created here, will be sent to PE but can be used here too.
                


        ParticleEngine.ShaderStruct Shaders;
        



        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image, GraphicsDeviceManager Graphics): base(game, spriteBatch)
        {

            this.graphics = Graphics;

            this.image = image;
            imageRectangle = new Rectangle(
                                            0,
                                            0,
                                            Game.Window.ClientBounds.Width,
                                            Game.Window.ClientBounds.Height);

           // videoPlayer = new VideoPlayer();
           // myVideoFile = game.Content.Load<Video>("Videos\\F1v4");

            

            player1 = new GameObject(game.Content.Load<Texture2D>("Sprites\\120-staryu"));
            player1.position = new Vector2(player1_initial_position_X, player1_initial_position_Y);
            player1.alive = true;


            player1_weapon1 = new GameObject[max_player1_weapon1];
            for (int i = 0; i < max_player1_weapon1; i++)
            {
                player1_weapon1[i] = new GameObject(game.Content.Load<Texture2D>("Sprites\\bubblemod_ss"));

            }


            player1_weapon2 = new GameObject[max_player1_weapon2];
            for (int i = 0; i < max_player1_weapon2; i++)
            {
                player1_weapon2[i] = new GameObject(game.Content.Load<Texture2D>("Sprites\\pokeball_color1_ss"));

            }

            enemies1 = new GameObject[max_enemies1];
            for (int i = 0; i < max_enemies1; i++)
            {
                enemies1[i] = new GameObject(game.Content.Load<Texture2D>("Sprites\\042-golbat_ss"));
            }


            enemies2 = new GameObject[max_enemies2];
            for (int i = 0; i < max_enemies2; i++)
            {
                enemies2[i] = new GameObject(game.Content.Load<Texture2D>("Sprites\\041-zubat_ss"));
            }


            ball = new GameObject(game.Content.Load<Texture2D>("Sprites\\gravity_weapon_ss"));

            Set_Values();

            font = game.Content.Load<SpriteFont>("Fonts\\SpriteFont1");

            window_frame = new Rectangle(
                0,
                0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height
                );


            BGM = game.Content.Load<Song>("Audio\\128-battle-vs-gym-leader-");
            
            zubat_scream = game.Content.Load<SoundEffect>("Audio\\zubat_scream");
            golbat_scream = game.Content.Load<SoundEffect>("Audio\\golbat_scream");

         //   myVideoFile = game.Content.Load<Video>("Videos\\F1v4");

            // TODO: use this.Content to load your game content here

            Shaders.Blur = game.Content.Load<Effect>("Shaders\\blur");
            Shaders.Grayscale = game.Content.Load<Effect>("Shaders\\grayscale");
            Shaders.Pulse_Simple = game.Content.Load<Effect>("Shaders\\pulse_simple");
            Shaders.Pulse_Blur_Time_Trig = game.Content.Load<Effect>("Shaders\\pulse_blur_time_trig");
            Shaders.Pulse_Blur_Var_Linear = game.Content.Load<Effect>("Shaders\\pulse_blur_var_linear");
            Shaders.Weapon2_Timefade = game.Content.Load<Effect>("Shaders\\weapon2_timefade");
            

            PE1 = new ParticleEngine(graphics, spriteBatch, Shaders, game.Content.Load<Texture2D>("Sprites\\red_small16_frame32_ss"), 2);
            PE2 = new ParticleEngine(graphics, spriteBatch, Shaders, game.Content.Load<Texture2D>("Sprites\\green_small16_frame32_ss"), 2);
            PE3 = new ParticleEngine(graphics, spriteBatch, Shaders, game.Content.Load<Texture2D>("Sprites\\green_small16_frame32_ss"), 1);//"Sprites\\explosion1mod2b"), 1);

            // Create animerted objecct
            //Giving it temporary sprite here, will update to video fram later
          //  Feynman = new GameObject(game.Content.Load<Texture2D>("Sprites\\120-staryu"));
                        
       
        }



        public override void Initialize()
        {

            
            base.Initialize();
        }





        public override void Update(GameTime gameTime)
        {
            // To check FPS
            //elapsedTime += gameTime.ElapsedGameTime;

            //if (elapsedTime > TimeSpan.FromSeconds(1))
            //{
            //    elapsedTime -= TimeSpan.FromSeconds(1);
            //    frameRate = frameCounter;
            //    frameCounter = 0;
            //}

            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(BGM);
            }

          
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);


          // videoPlayer.Play(myVideoFile);

            Player1Health();

            Player1Death(gameTime);

            Update_Walls();

            Controls(gameTime);

            Collisions(gameTime);

            Update_Weapons(gameTime);

            time_now = (float)gameTime.TotalGameTime.TotalMilliseconds;

            Physics(time_before, time_now);



            time_before = (float)gameTime.TotalGameTime.TotalMilliseconds;


            Spawn_enemies(1, 0f, 0f);
            

            //planet.position.X = 600;
            //planet.position.Y = 300;
            

            PE1.UpdateParticles(PE1.ParticleArray, gameTime, (player1.position + player1.center));
            PE2.UpdateParticles(PE2.ParticleArray, gameTime, (player1.position + player1.center));
            PE3.UpdateParticles(PE3.ParticleArray, gameTime, new Vector2(0,0));


           // Feynman.sprite = videoPlayer.GetTexture();
            //Feynman.position = new Vector2(10*(float)Math.Sin(gameTime.TotalGameTime.Milliseconds), 10*(float)Math.Sin(gameTime.TotalGameTime.Milliseconds));
            //if (gameTime.TotalGameTime.Seconds % 5 == 0)
            //{
            //    Feynman.position = new Vector2(random.Next(1200), random.Next(720));
            //}
            // TODO: Add your update logic here

            base.Update(gameTime);
            keyboardState_before = keyboardState;
            gamePadState_before = gamePadState;
        }



        



        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Draw(image, imageRectangle, Color.White);
            

            // For FPS
            frameCounter++;
            string fps = string.Format("FPS: {0}", frameRate);


            

            this.graphics.GraphicsDevice.Clear(Color.Black);


            //Draw particles from te Particle Enigne

            PE1.DrawExplosion(PE1.ParticleArray, spriteBatch, gameTime, player1);
            PE2.DrawExplosion(PE2.ParticleArray, spriteBatch, gameTime, player1);
            PE3.DrawExplosion(PE3.ParticleArray, spriteBatch, gameTime, player1);





            // Draw other objects


            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(ball.sprite, ball.position, Color.White);


            foreach (GameObject shot in player1_weapon1)
            {
                if (shot.alive)
                {
                    spriteBatch.Draw(shot.sprite, shot.position, Color.White);
                }

            }



            

            foreach (GameObject enemy in enemies1)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite, enemy.position, Color.White);
                }
            }

            foreach (GameObject enemy in enemies2)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite, enemy.position, Color.White);
                }
            }


            

            //spriteBatch.DrawString(
            //    font,
            //    "Position: " + ball.position.ToString() + "           time_before: " + time_before.ToString(),
            //    new Vector2(10, 10),
            //    Color.Yellow
            //    );


            //spriteBatch.DrawString(
            //    font,
            //    fps,
            //    new Vector2(600, 10),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //    font,
            //    "Velocity: " + ball.velocity.ToString() + "                  time_now: " + time_now.ToString(),
            //    new Vector2(10, 30),
            //    Color.Yellow
            //    );



            //spriteBatch.DrawString(
            //    font,
            //    "time diff" + time_diff.ToString(),
            //    new Vector2(300, 50),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //    font,
            //    "Acceleration: " + ball.acceleration.ToString(),
            //    new Vector2(10, 50),
            //    Color.Yellow
            //    );




            spriteBatch.DrawString(
                font,
                "Player1 Life " + player1.HP.ToString(),
                new Vector2(20, 90),
                Color.Yellow
                );


            spriteBatch.DrawString(
                font,
                "Player1 Score " + player1_score.ToString(),
                new Vector2(20, 110),
                Color.Yellow
                );


            //spriteBatch.DrawString(
            //    font,
            //    "PE1 ParticleCounter:  " + PE1.ParticleCounter.ToString(),
            //    new Vector2(500, 30),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //    font,
            //    "PE2 ParticleCounter:  " + PE2.ParticleCounter.ToString(),
            //    new Vector2(500, 50),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //    font,
            //    "PE1 ParticlesCreated:  " + PE1.ParticlesCreated.ToString(),
            //    new Vector2(750, 30),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //    font,
            //    "PE2 ParticlesCreated:  " + PE2.ParticlesCreated.ToString(),
            //    new Vector2(750, 50),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //    font,
            //    "PE1 ParticlesKilled:  " + PE1.ParticlesKilled.ToString(),
            //    new Vector2(1000, 30),
            //    Color.Yellow
            //    );

            //spriteBatch.DrawString(
            //     font,
            //     "PE2 ParticlesKilled:  " + PE2.ParticlesKilled.ToString(),
            //     new Vector2(1000, 50),
            //     Color.Yellow
            //     );

            //spriteBatch.DrawString(
            //     font,
            //     "PE1 ParticlesOverwritten:  " + PE1.ParticlesOverwritten.ToString(),
            //     new Vector2(800, 80),
            //     Color.Yellow
            //     );

            //spriteBatch.DrawString(
            //     font,
            //     "PE2 ParticlesOverwritten:  " + PE2.ParticlesOverwritten.ToString(),
            //     new Vector2(800, 100),
            //     Color.Yellow
            //     );


            

            spriteBatch.End();


            // Draw video


            //spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);

            //Shaders.Pulse_Blur_Time_Trig.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds * 2);



            //Shaders.Pulse_Blur_Time_Trig.Begin();
            //Shaders.Pulse_Blur_Time_Trig.CurrentTechnique.Passes[0].Begin();


            //spriteBatch.Draw(Feynman.sprite, Feynman.position, Color.White);

            //Shaders.Pulse_Blur_Time_Trig.CurrentTechnique.Passes[0].End();
            //Shaders.Pulse_Blur_Time_Trig.End();

            //spriteBatch.End();




            //Draw weapon2


            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            
           

            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    //Shaders.Weapon2_Timefade.Parameters["fade_percentage"].SetValue(shot.FadePercentage);

                    //Shaders.Weapon2_Timefade.Begin();
                    //Shaders.Weapon2_Timefade.CurrentTechnique.Passes[0].Begin();
                    
                    
                    
                    spriteBatch.Draw(shot.sprite, shot.position, new Color(255,255,255,shot.FadePercentage*255*0.01f));

                    //Shaders.Weapon2_Timefade.CurrentTechnique.Passes[0].End();
                    //Shaders.Weapon2_Timefade.End();

                    


                }

            }


            




            spriteBatch.End();





            //Draw player1
            

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            Shaders.Pulse_Simple.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds/2);
            Shaders.Pulse_Simple.Begin();
            Shaders.Pulse_Simple.CurrentTechnique.Passes[0].Begin();

            spriteBatch.Draw(player1.sprite, player1.position, Color.White);


            Shaders.Pulse_Simple.CurrentTechnique.Passes[0].End();
            Shaders.Pulse_Simple.End();


         
            
            
            spriteBatch.End();

            




            base.Draw(gameTime);
            

        }











        // Extra functions
        


        private void Controls(GameTime gameTime)
        {


            
            // CONTROLS


           // KeyboardState keyboardState = Keyboard.GetState();
           // GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);


            // Xbox controller controls


            if (gamePadState.IsConnected)
            {

                Vector2 LeftStick = gamePadState.ThumbSticks.Left;
                Vector2 RightStick = gamePadState.ThumbSticks.Right;

                // Invert y-axis
                LeftStick.Y = -LeftStick.Y;
                RightStick.Y = -RightStick.Y;


                //Player1 Controls

                // Kill accleration if oposite direction 

                if (LeftStick.X / player1.acceleration.X < 0)
                {
                    player1.acceleration.X = 0;
                }

                if (LeftStick.Y / player1.acceleration.Y < 0)
                {
                    player1.acceleration.Y = 0;
                }


                // Apply accelration

                player1.acceleration.X += player1.acceleration_increment * LeftStick.X;
                player1.acceleration.Y += player1.acceleration_increment * LeftStick.Y;

                // Kill accleration if stick not used

                if (LeftStick.X == 0)
                {
                    player1.acceleration.X = 0;
                }

                if (LeftStick.Y == 0)
                {
                    player1.acceleration.Y = 0;
                }


                // Gravity well (ball) controls

                // Kill accleration if oposite direction 

                if (RightStick.X / ball.acceleration.X < 0)
                {
                    ball.acceleration.X = 0;
                }

                if (RightStick.Y / ball.acceleration.Y < 0)
                {
                    ball.acceleration.Y = 0;
                }


                // Apply accelration

                ball.acceleration.X += ball.acceleration_increment * RightStick.X;
                ball.acceleration.Y += ball.acceleration_increment * RightStick.Y;

                // Kill accleration if stick not used

                if (RightStick.X == 0)
                {
                    ball.acceleration.X = 0;
                }

                if (RightStick.Y == 0)
                {
                    ball.acceleration.Y = 0;
                }


                // Teleport ball in front of player

                if (gamePadState.Buttons.X == ButtonState.Pressed )//&& gamePadState_before.Buttons.X == ButtonState.Released)
                {
                    ball.position = player1.position;
                    ball.position.X = player1.position.X + 100f;
                }


                if (gamePadState.Triggers.Right > 0)
                {
                    Fire_Weapons("2", gameTime);
                    //GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                }


                // RESET POSITION
                if ( gamePadState.DPad.Down == ButtonState.Pressed && gamePadState_before.DPad.Down == ButtonState.Released )
                {
                    player1.position.X = player1_initial_position_X;
                    player1.position.Y = player1_initial_position_Y;

                    for (int i = 0; i < max_player1_weapon2; i++)
                    {
                        if (player1_weapon2[i].alive)
                        {
                            player1_weapon2[i].alive = false;

                        }

                    }


                    for (int i = 0; i < max_enemies1; i++)
                    {
                        if (enemies1[i].alive)
                        {
                            enemies1[i].alive = false;

                        }
                    }
                }             
            }
         


      
            
            // Keyboard controls


            if (keyboardState.IsKeyDown(Keys.A))
            {
                player1.acceleration.X -= player1.acceleration_increment;

                if (player1.acceleration.X > 0)
                {
                    player1.acceleration.X = 0;
                }


            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                player1.acceleration.X += player1.acceleration_increment;

                if (player1.acceleration.X < 0)
                {
                    player1.acceleration.X = 0;
                }

            }


            if (keyboardState.IsKeyDown(Keys.W))
            {
                player1.acceleration.Y -= player1.acceleration_increment;

                if (player1.acceleration.Y > 0)
                {
                    player1.acceleration.Y = 0;
                }

            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                player1.acceleration.Y += player1.acceleration_increment;

                if (player1.acceleration.Y < 0)
                {
                    player1.acceleration.Y = 0;
                }
            }


            // Kill acceleration if not pressed

            if (keyboardState.IsKeyUp(Keys.A) && keyboardState_before.IsKeyDown(Keys.A))
            {
                player1.acceleration.X = 0;
            }
            if (keyboardState.IsKeyUp(Keys.D) && keyboardState_before.IsKeyDown(Keys.D))
            {
                player1.acceleration.X = 0;
            }
            if (keyboardState.IsKeyUp(Keys.W) && keyboardState_before.IsKeyDown(Keys.W))
            {
                player1.acceleration.Y = 0;
            }
            if (keyboardState.IsKeyUp(Keys.S) && keyboardState_before.IsKeyDown(Keys.S))
            {
                player1.acceleration.Y = 0;
            }



            // Gravity well controls (ball)

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                ball.acceleration.X -= ball.acceleration_increment;

                if (ball.acceleration.X > 0)
                {
                    ball.acceleration.X = 0;
                }


            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                ball.acceleration.X += ball.acceleration_increment;

                if (ball.acceleration.X < 0)
                {
                    ball.acceleration.X = 0;
                }

            }


            if (keyboardState.IsKeyDown(Keys.Up))
            {
                ball.acceleration.Y -= ball.acceleration_increment;

                if (ball.acceleration.Y > 0)
                {
                    ball.acceleration.Y = 0;
                }

            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                ball.acceleration.Y += ball.acceleration_increment;

                if (ball.acceleration.Y < 0)
                {
                    ball.acceleration.Y = 0;
                }
            }


            // Kill acceleration if not pressed

            if (keyboardState.IsKeyUp(Keys.Left) && keyboardState_before.IsKeyDown(Keys.Left))
            {
                ball.acceleration.X = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Right) && keyboardState_before.IsKeyDown(Keys.Right))
            {
                ball.acceleration.X = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Up) && keyboardState_before.IsKeyDown(Keys.Up))
            {
                ball.acceleration.Y = 0;
            }
            if (keyboardState.IsKeyUp(Keys.Down) && keyboardState_before.IsKeyDown(Keys.Down))
            {
                ball.acceleration.Y = 0;
            }


            // Teleport ball in front of player

            if (keyboardState.IsKeyDown(Keys.E) && keyboardState_before.IsKeyUp(Keys.E))
            {
                ball.position = player1.position;
                ball.position.X = player1.position.X + 100f;
            }





            // RESET POSITION
            if (keyboardState.IsKeyDown(Keys.R))
            {
                player1.position.X = player1_initial_position_X;
                player1.position.Y = player1_initial_position_Y;

                for (int i = 0; i < max_player1_weapon2; i++)
                {
                    if (player1_weapon2[i].alive)
                    {
                        player1_weapon2[i].alive = false;

                    }

                }



                for (int i = 0; i < max_enemies1; i++)
                {
                    if (enemies1[i].alive)
                    {
                        enemies1[i].alive = false;

                    }

                }



            }

            // WEAPONS


            if (keyboardState.IsKeyDown(Keys.Space) && keyboardState_before.IsKeyUp(Keys.Space))
            {
                Fire_Weapons("1",gameTime);
            }

            if (keyboardState.IsKeyDown(Keys.G))//&& keyboardState_before.IsKeyUp(Keys.G))
            {
                Fire_Weapons("2",gameTime);
            }


       


        }


        private void Physics(float time_before, float time_now)
        {


            time_diff = time_now - time_before;
            time_diff = time_diff * time_stretch;



            /// Gravity relative to weapon2

            for (int i = 0; i < max_player1_weapon2; i++)
            {
                if (player1_weapon2[i].alive)
                {

                    Gravity(ball, player1_weapon2[i]);


                }

            }





            // Update velocity and position of player

            Update_PVA(player1);
            Update_PVA(ball);


            // Update velocity and position of weapon1
            foreach (GameObject shot in player1_weapon1)
            {
                if (shot.alive)
                {
                    Update_PVA(shot);
                }
            }


            // Update velocity and position of weapon2
            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    Update_PVA(shot);
                }
            }


            Update_enemies(1);
            Update_enemies(2);




        }



        private void Fire_Weapons(string weapon_id, GameTime gameTime)
        {

            //float Weapon1_velocity = 10.0f;
            //float Weapon2_velocity = 50.0f;
            

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
                            shot.BirhtTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

                            GamePad.SetVibration(PlayerIndex.One, 0.15f, 0.15f);

                            //continue; // Use this for one shot per button press
                            return;
                        }
                        else
                        {
                            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                        }

                        
                    }
                    break;


                default: break;

            }


        }


        private void Update_Weapons(GameTime gameTime)
        {

            foreach (GameObject shot in player1_weapon1)
            {
                if (shot.alive)
                {


                    if (!window_frame.Contains(new Point((int)shot.position.X, (int)shot.position.Y)))
                    {
                        shot.alive = false;
                        shot.velocity = new Vector2(player1_weapon1_initial_velocity_X, 0);
                        shot.acceleration = new Vector2(0, 0);

                        continue;
                    }

                }

                else
                {
                    shot.velocity = new Vector2(player1_weapon1_initial_velocity_X, 0);
                    shot.acceleration = new Vector2(0, 0);
                }
            }


            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {

                    shot.NowAge = (float)gameTime.TotalGameTime.TotalMilliseconds - shot.BirhtTime;
                    shot.FadePercentage = 1 - (shot.NowAge / player1_weapon2_maxage);
                    if (shot.NowAge > player1_weapon2_maxage)
                    {
                        shot.alive = false;
                        shot.velocity = new Vector2(player1_weapon2_initial_velocity_X, 0);
                        shot.acceleration = new Vector2(0, 0);
                        
                        continue;
                    }

                    //if (shot.position.X > Window_Width)
                    //{
                    //    shot.alive = false;
                    //    shot.velocity = new Vector2(player1_weapon2_initial_velocity_X, 0);
                    //    shot.acceleration = new Vector2(0, 0);

                    //    continue;
                    //}
                }

                else
                {
                    shot.velocity = new Vector2(player1_weapon2_initial_velocity_X, 0);
                    shot.acceleration = new Vector2(0, 0);
                }
            }

        }




        private void Gravity(GameObject m1, GameObject m2)
        {
            // Calculates and applies appropriate force -> accelration to both masses relative to each other
            //// F = G*m1*m2 / r^2      
            /// F= ma, a = F/m


            double F = 0;
            float r = Vector2.Distance((m1.position + m1.center), (m2.position + m2.center));

            //r = r / 5;

            if (r < 10)
            {
                r = 10;
            }

            F = (G * m1.mass * m2.mass) / Math.Pow(r, 2);



            Vector2 delta = (m2.position + m2.center) - (m1.position + m1.center);
            Vector2 delta_normalized = Vector2.Normalize(delta);



            // Scale caclulated Force int 2d Vector by theta using matrix


            Vector2 Applied_Force = delta_normalized * (int)F;


            // Apply Force to m1 using a = F/m
            Vector2 force_acceleration_m1 = Applied_Force / m1.mass;
            if (m1.IsGravityWell == false)
            {
                m1.acceleration += force_acceleration_m1;
            }

            // Apply Force to m2 using a = F/m
            Vector2 force_acceleration_m2 = Applied_Force / m2.mass;
            if (m2.IsGravityWell == false)
            {
                m2.acceleration -= force_acceleration_m2;
            }


        }





        private void Update_PVA(GameObject m1)
        {


            if (m1.IsPlayer == false)
            {
                m1.acceleration.X = MathHelper.Clamp(m1.acceleration.X, -m1.acceleration_clamp, m1.acceleration_clamp);
                m1.acceleration.Y = MathHelper.Clamp(m1.acceleration.Y, -m1.acceleration_clamp, m1.acceleration_clamp);
            }


            if (m1.IsPlayer == false)
            {
                m1.velocity.X = MathHelper.Clamp(m1.velocity.X, -m1.velocity_clamp, m1.velocity_clamp);
                m1.velocity.Y = MathHelper.Clamp(m1.velocity.Y, -m1.velocity_clamp, m1.velocity_clamp);
            }


            m1.velocity += m1.acceleration * (time_diff);
            m1.position += m1.velocity * (time_diff);


            // Friction
            m1.velocity.X = m1.velocity.X * (1 - m1.friction);
            m1.velocity.Y = m1.velocity.Y * (1 - m1.friction);



            if (Math.Abs(m1.velocity.X) < 1)
            {
                m1.velocity.X = 0;
            }

            if (Math.Abs(m1.velocity.Y) < 1)
            {
                m1.velocity.Y = 0;
            }




        }


        //Used for oboros top/bot
        private void Check_Borders(GameObject m1)
        {

            if (m1.position.Y > Window_Height)
            {
                m1.position.Y = 0;
            }

            if (m1.position.Y < 0)
            {
                m1.position.Y = Window_Height;
            }


        }


        private void Wall_Bounce(GameObject m1)
        {
            

            if (m1.position.X < 0)
            {
                m1.velocity.X = -m1.velocity.X;
            }

            if (m1.position.Y < 0 || m1.position.Y > Window_Height)
            {
                m1.velocity.Y = -m1.velocity.Y;
            }



        }


        private void Update_Walls()
        {
            //Check_Borders(player1);

            Wall_Bounce(player1);

            //foreach (GameObject shot in player1_weapon2)
            //{
            //    //Check_Borders(shot);
            //    Wall_Bounce(shot);
            //}
        }


        private void Spawn_enemies(int enemyID, float spawnX, float spawnY)
        {

            switch (enemyID)
            {

                case 1:


                    for (int i = 0; i < max_enemies1; i++)
                    {
                        if (enemies1[i].alive == false)
                        {
                            enemies1[i].alive = true;
                            enemies1[i].position = new Vector2(Window_Width + 500, MathHelper.Lerp(0, Window_Height, (float)random.NextDouble()));
                            enemies1[i].velocity = new Vector2(-(float)random.Next(min_enemies1_velocity, max_enemies1_velocity), 0f);
                        }


                    }
                    break;



                case 2:



                    for (int i = 1; i < max_enemies2; i++)
                    {

                        if (enemies2[i].alive == false)
                        {
                            enemies2[i].alive = true;
                            enemies2[i].position = new Vector2(spawnX, spawnY);
                            break;
                        }


                    }
                    break;




                default: break;

            }


        }



        private void Update_enemies(int enemyID)
        {

            switch (enemyID)
            {

                case 1:
                    {
                        //Update enemies1 PVA
                        foreach (GameObject enemy in enemies1)
                        {
                            if (enemy.alive)
                            {

                                Update_PVA(enemy);

                                if (enemy.position.X < 0)
                                {
                                    enemy.alive = false;
                                    player1_score -= 2;

                                }
                            }
                        }


                    }
                    break;



                case 2:
                    {

                        foreach (GameObject enemy in enemies2)
                        {
                            if (enemy.alive)
                            {

                                Vector2 delta = player1.position - enemy.position;
                                Vector2 norm = Vector2.Normalize(delta);
                                enemy.velocity = norm * enemies2_velocity_scalar;


                                Update_PVA(enemy);

                                if (enemy.position.X < 0 || enemy.position.X > Window_Width || enemy.position.Y < 0 || enemy.position.Y > Window_Height)
                                {
                                    enemy.alive = false;
                                }
                            }
                        }
                    }
                    break;


                default: break;

            }


        }


        private bool Check_Collision(GameObject m1, GameObject m2)
        {

            Rectangle m1Rect = new Rectangle(
                                            (int)m1.position.X + (int)m1.center.X,
                                            (int)m1.position.Y + (int)m1.center.Y,
                                            m1.sprite.Width,
                                            m1.sprite.Height);

            Rectangle m2Rect = new Rectangle(
                                            (int)m2.position.X + (int)m2.center.X,
                                            (int)m2.position.Y + (int)m2.center.Y,
                                            m2.sprite.Width,
                                            m2.sprite.Height);


            if (m1Rect.Intersects(m2Rect))
            {
                return true;
            }

            else
                return false;


        }

        private void Collisions(GameTime gameTime)
        {
            //Check for enemies1 collisions AND apply consequences

            foreach (GameObject enemy in enemies1)
            {
                if (enemy.alive)
                {
                    //Check player collision

                    if (Check_Collision(enemy, player1))
                    {
                        player1.HP -= 1;
                    }
                }

            }


            foreach (GameObject enemy in enemies2)
            {
                if (enemy.alive)
                {
                    //Check player collision

                    if (Check_Collision(enemy, player1))
                    {
                        player1.HP -= 1;
                    }
                }

            }

            //Check player1_weapon1 collision

            foreach (GameObject shot in player1_weapon1)
            {
                if (shot.alive)
                {
                    foreach (GameObject enemy in enemies1)
                    {
                        if (enemy.alive)
                        {

                            if (Check_Collision(enemy, shot))
                            {
                                player1_score += 1;
                                enemy.alive = false;
                                shot.alive = false;


                                SoundEffectInstance GS = golbat_scream.CreateInstance();
                                GS.Pitch = 0f;
                                GS.Volume = 0.5f;
                                GS.Play();


                                PE1.AddExplosion(PE1.ParticleArray, PE1.MaxParticles, enemy.position, PE1.ExplosionSize, gameTime, shot.velocity);
                                Spawn_enemies(2, shot.position.X + shot.center.X + 3 * shot.velocity.X, shot.position.Y + shot.center.Y + 3 * shot.velocity.Y);
                            }
                        }
                    }


                    foreach (GameObject enemy in enemies2)
                    {
                        if (enemy.alive)
                        {


                            if (Check_Collision(enemy, shot))
                            {
                                player1_score += 1;
                                enemy.alive = false;
                                shot.alive = false;

                                SoundEffectInstance ZS = zubat_scream.CreateInstance();
                                ZS.Pitch = 0f;
                                ZS.Volume = 0.5f;
                                ZS.Play();

                                PE2.AddExplosion(PE2.ParticleArray, PE2.MaxParticles, enemy.position, PE2.ExplosionSize, gameTime, shot.velocity);

                            }
                        }
                    }

                }
            }


            //Check player1_weapon2 collision

            foreach (GameObject shot in player1_weapon2)
            {
                if (shot.alive)
                {
                    foreach (GameObject enemy in enemies1)
                    {
                        if (enemy.alive)
                        {

                            if (Check_Collision(enemy, shot))
                            {
                                player1_score += 1;
                                enemy.alive = false;
                                shot.alive = false;

                                SoundEffectInstance GS = golbat_scream.CreateInstance();
                                GS.Pitch = 0f;
                                GS.Volume = 0.5f;
                                GS.Play();

                                PE1.AddExplosion(PE1.ParticleArray, PE1.MaxParticles, enemy.position, PE1.ExplosionSize, gameTime, shot.velocity);

                                Spawn_enemies(2, shot.position.X + shot.center.X + 0.5f * shot.velocity.X, shot.position.Y + shot.center.Y + 0.5f * shot.velocity.Y);
                            }
                        }
                    }

                    foreach (GameObject enemy in enemies2)
                    {
                        if (enemy.alive)
                        {

                            if (Check_Collision(enemy, shot))
                            {
                                player1_score += 1;
                                enemy.alive = false;
                                shot.alive = false;

                                SoundEffectInstance ZS = zubat_scream.CreateInstance();
                                ZS.Pitch = 0f;
                                ZS.Volume = 0.5f;
                                ZS.Play();

                                PE2.AddExplosion(PE2.ParticleArray, PE2.MaxParticles, enemy.position, PE2.ExplosionSize, gameTime, shot.velocity);

                            }
                        }
                    }


                }
            }


        }


        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && keyboardState_before.IsKeyDown(theKey);
        }




        private void Player1Health()
        {
            if (player1.HP <= 0)
            {
                player1.alive = false;
            }
        }


                //Handles Plyer1 death
        private void Player1Death(GameTime gameTime)
        {

            if (player1.alive == false && player1_exploded == false )//&& player1.HP == 0)
            {
                PE3.AddExplosion(PE3.ParticleArray, PE3.MaxParticles, (player1.position + player1.center), PE3.ExplosionSize, gameTime, new Vector2(0,0));
                player1_exploded = true;
                //game.Exit();
            }

        }



        private void Set_Values()
        {

            time_stretch = 0.01f;
            G = 100f;

            player1.scale = 1.0f;
            player1.mass = 1;
            player1.IsPlayer = true;
            player1.acceleration_clamp = 100.0f;
            player1.friction = 0.025f;
            player1.HP = 100;
            player1.acceleration_increment = 1;
            

            for (int i = 0; i < max_player1_weapon1; i++)
            {

                player1_weapon1[i].friction = 0.0f;
                player1_weapon1[i].velocity = new Vector2(player1_weapon1_initial_velocity_X, 0);
                player1_weapon1[i].velocity_clamp = 50.0f;

            }



            for (int i = 0; i < max_player1_weapon2; i++)
            {

                player1_weapon2[i].mass = 1f;
                player1_weapon2[i].acceleration_clamp = 50.0f;
                player1_weapon2[i].velocity = new Vector2(player1_weapon2_initial_velocity_X, 0);
                player1_weapon2[i].velocity_clamp = 2000.0f;
                player1_weapon2[i].friction = 0.02f;

            }
            player1_weapon2_maxage = 5000;



            foreach (GameObject enemy in enemies1)
            {
                enemy.friction = 0;
                enemy.velocity_clamp = 100;
            }
            min_enemies1_velocity = 20;
            max_enemies1_velocity = 60;


            foreach (GameObject enemy in enemies2)
            {
                enemy.alive = false;
                enemy.friction = 0;
                enemy.velocity.X = 10f;
                enemy.velocity.Y = 10f;
                enemy.velocity_clamp = 100f;
                enemy.acceleration_clamp = 100f;
            }


            ball.position.X = 450;
            ball.position.Y = 350;
            ball.mass = 100000.0f;
            ball.acceleration_clamp = 11000000.75f;
            ball.velocity.Y = 0;
            ball.velocity.X = 0f;
            ball.velocity_clamp = 1000f;
            ball.acceleration_clamp = 1000f;
            ball.acceleration_increment = 2;
            ball.friction = 0.05f;
            ball.IsGravityWell = true;


        }



    }
}
