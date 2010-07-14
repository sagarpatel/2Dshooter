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

    // Contains all Particle realated functions
    // ex: AddParticles(), AddExplosion, etc



    class ParticleEngine
    {

    

        // Struct template for all particles

        // Textures of a given particle will be held seperately, outisde of the struct and only once.
        // The texture variable will be called directly when using SpriteBatch.Draw to draw the particle
        // This will avoid using up memory with the texture
    


        public struct ParticleData
        {
            public float BirthTime;
            
            public float NowAge;
            public Vector2 OrginalPosition;
            public Vector2 Accelaration;
            public Vector2 Direction;
            public Vector2 Position;
            public float Scaling;
            public Color ModColor;
            public bool IsHoming;
            public bool IsAlive;

        }


        
        public ParticleData [] ParticleArray;
        public int ParticleArraySize;

        public int ParticleCounter =0;

        public float ParticleScale = 0.0f; // Used in AddExplosionParticle()
        public float ParticleScaleFactor1 = 0f;
        public float ParticleScaleFactor2 = 0f;
        public float ParticleVectorX = 64;
        public float ParticleVectorY = 64;
        public float ParticleInitialVelocityScale = 1.0f;
        public float ParticleAcceleration = 1f; // Used in AddExplosionParticle()
        public float ParticleToPlayerAccelerationScale = 1.0f;
        public float ExplosionSize = 40f;
        public float ParticleMaxAge = 350.0f;
        public float ParticleAgeOffset = 1000f;
        public int MaxParticles = 20; // U

        public int ParticleID = 0;

        Random random = new Random(); // Will be used later for random generation
        
        Texture2D Explosion1Sprite;
               
        SpriteBatch spriteBatch;


        public ParticleEngine(SpriteBatch SB, Texture2D EXP1Tex, int PID)
        {
            

            Explosion1Sprite = EXP1Tex;

            spriteBatch = SB;

            ParticleID = PID;

            switch (ParticleID)
            {

                case 1:
                    ParticleScale = 50.0f; // Used in AddExplosionParticle()
                    ParticleScaleFactor1 = 10f;
                    ParticleScaleFactor2 = 20f;
                    ParticleVectorX = 64;
                    ParticleVectorY = 64;
                    ParticleAcceleration = 1f; // Used in AddExplosionParticle()
                    ExplosionSize = 40f;
                    ParticleMaxAge = 350.0f;
                    MaxParticles = 20;
                    ParticleArraySize = 1000;
                    ParticleArray = new ParticleData[ParticleArraySize];

                    break;

                case 2:
                    ParticleScale = 0.10f;
                    
                    ParticleVectorX = 0 ;
                    ParticleVectorY = 0;
                    ParticleInitialVelocityScale = 3.0f;
                    ParticleAcceleration = 0.0f; // Used in AddExplosionParticle()
                    ParticleToPlayerAccelerationScale = 300.0f;
                    ExplosionSize = 100f;
                    ParticleMaxAge = 500f;
                    ParticleAgeOffset = 10000f;
                    MaxParticles = 1000;
                    ParticleArraySize = 10000;
                    ParticleArray = new ParticleData[ParticleArraySize];
                    break;


                default: break;
            }

        }





            
        public void AddExplosionParticle1(ParticleData[] PA, Vector2 ExplosionPosition, float ExplosionSize, GameTime gametime)
        {

            ParticleData particle = new ParticleData();
            
            particle.OrginalPosition = ExplosionPosition;
            particle.Position = particle.OrginalPosition;

            particle.BirthTime = (float)gametime.TotalGameTime.TotalMilliseconds;
            
            particle.Scaling = ParticleScale;
            particle.ModColor = Color.White;

            float ParticleDistance = (float)random.NextDouble() * ExplosionSize;
            Vector2 Displacement = new Vector2(ParticleDistance);

            float angle = MathHelper.ToRadians(random.Next(360));
            Displacement = Vector2.Transform(Displacement, Matrix.CreateRotationZ(angle));

            particle.Direction = ParticleInitialVelocityScale * Displacement;
            particle.Accelaration = -ParticleAcceleration * particle.Direction;

            particle.IsAlive = true;
            if (ParticleCounter == ParticleArraySize - 1)
            {
                ParticleCounter = 0;
            }
            PA[ParticleCounter] = particle;
            ParticleCounter++;

        }


        public void AddExplosionParticle2(ParticleData[] PA, Vector2 ExplosionPosition, float ExplosionSize, GameTime gametime, Vector2 ImpactVelocity)
        {
            int ConeWidth = 75;

            ParticleData particle = new ParticleData();

            particle.IsHoming = false;
            particle.OrginalPosition = ExplosionPosition;
            particle.Position = particle.OrginalPosition;

            particle.BirthTime = (float)gametime.TotalGameTime.TotalMilliseconds;
            
            //particle.Scaling = ParticleScale;
            particle.ModColor = Color.White;

            float ParticleDistance = (float)random.NextDouble() * ExplosionSize;
            Vector2 Displacement = new Vector2(ParticleDistance,0);


            Vector2 IV = ImpactVelocity;

            Vector2 IVnorm = Vector2.Normalize(IV);
            Vector2 CartesianIVnorm = new Vector2(0, 0);

            float angle = (float)Math.Atan( (IVnorm.Y / IVnorm.X));
            angle = MathHelper.ToDegrees(angle);



            float tanangle = Math.Abs(angle);
            float angleRangeMin=0;
            float angleRangeMax=0;
            

            int quadrant = FindQuadrant(IVnorm);

            switch (quadrant)
            {
                case 1:
                    CartesianIVnorm = new Vector2(Math.Abs(IVnorm.X), Math.Abs(IVnorm.Y));
                    angle = (float)Math.Atan((CartesianIVnorm.Y / CartesianIVnorm.X));
                    angle = MathHelper.ToDegrees(angle);
                    angleRangeMin = angle - ConeWidth;
                    angleRangeMax = angle + ConeWidth;
                    break;

                case 2:
                    CartesianIVnorm = new Vector2(-Math.Abs(IVnorm.X), Math.Abs(IVnorm.Y));
                    angle = (float)Math.Atan((CartesianIVnorm.X / CartesianIVnorm.Y));
                    angle = Math.Abs(MathHelper.ToDegrees(angle));
                    angleRangeMin = angle + 90 - ConeWidth;
                    angleRangeMax = angle + 90 + ConeWidth;
                    break;

                case 3:
                    CartesianIVnorm = new Vector2(-Math.Abs(IVnorm.X), -Math.Abs(IVnorm.Y));
                    angle = (float)Math.Atan((CartesianIVnorm.Y / CartesianIVnorm.X));
                    angle = MathHelper.ToDegrees(angle);
                    angleRangeMin = angle + 180 - ConeWidth;
                    angleRangeMax = angle + 180 + ConeWidth;
                    break;

                case 4:
                    CartesianIVnorm = new Vector2(Math.Abs(IVnorm.X), -Math.Abs(IVnorm.Y));
                    angle = (float)Math.Atan((CartesianIVnorm.X / CartesianIVnorm.Y));
                    angle = Math.Abs(MathHelper.ToDegrees(angle));
                    angleRangeMin = angle + 270 - ConeWidth;
                    angleRangeMax = angle + 270 + ConeWidth;
                    break;

                case 0 :
                    if (IVnorm.X == 1 && IVnorm.Y == 0)
                    {
                        angleRangeMin = -ConeWidth;
                        angleRangeMax = ConeWidth;
                    }
                    else if (IVnorm.X == 0 && IVnorm.Y == -1)
                    {
                        angleRangeMin = 90 - ConeWidth;
                        angleRangeMax = 90 + ConeWidth;
                    }
                    else if (IVnorm.X == -1 && IVnorm.Y == 0)
                    {
                        angleRangeMin = 180 - ConeWidth;
                        angleRangeMax = 180 + ConeWidth;
                    }
                    else if (IVnorm.X == 0 && IVnorm.Y == 1)
                    {
                        angleRangeMin = 270 - ConeWidth;
                        angleRangeMax = 270 + ConeWidth;
                    }

                    break;
                                        
                default: break;
                    
            }


            

            // Generate radom angle between Min and Max, then feed to isplacment code below
            float radMin = 1000*MathHelper.ToRadians(angleRangeMin);
            float radMax = 1000*MathHelper.ToRadians(angleRangeMax);
            float randomangle = random.Next((int)radMin, (int)radMax);
            randomangle = randomangle / 1000;
            //randomangle = MathHelper.ToRadians(randomangle);

            

            //For now, Displacement the usual-> relative to ExplosionSize, later can change to be magnitude of IV by using Vector2.Lengt

            Displacement = Vector2.Transform(Displacement, Matrix.CreateRotationZ(-randomangle));

            particle.Direction = ParticleInitialVelocityScale * Displacement;
            particle.Accelaration = -ParticleAcceleration * particle.Direction;


            particle.IsAlive = true;

            if (ParticleCounter == ParticleArraySize - 1)
            {
                ParticleCounter = 0;
            }
            PA[ParticleCounter] = particle;
            ParticleCounter++;


        }



        public void AddExplosion(ParticleData[] PA, int MaxParticles, Vector2 ExplosionPosition, float ExplosionSize, GameTime gametime, Vector2 ImpactVelocity)
        {
            switch (ParticleID)
            {
                case 1:

                    for (int i = 0; i < MaxParticles; i++)
                    {
                        AddExplosionParticle1(PA, ExplosionPosition, ExplosionSize, gametime);
                    }
                    break;

                case 2:

                    for (int i = 0; i < MaxParticles; i++)
                    {
                        AddExplosionParticle2(PA, ExplosionPosition, ExplosionSize, gametime, ImpactVelocity);
                    }
                    break;


                default: break;
            }
                    

        }


         

        public void DrawExplosion(ParticleData[] PA, SpriteBatch spriteBatch)
        {


            for (int i = 0; i < ParticleArraySize; i++)
            {
                ParticleData particle = PA[i];
                if (particle.IsAlive==true)
                {
                    spriteBatch.Draw(Explosion1Sprite,
                                     particle.Position,
                                     null,
                                     particle.ModColor,
                                     i,
                                     new Vector2(ParticleVectorX, ParticleVectorY),
                                     particle.Scaling,
                                     SpriteEffects.None,
                                     1);
                }

            }
            
        }


        public void UpdateParticles(ParticleData[] PA, GameTime gameTime, Vector2 P1positon)
        {


            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;


            switch (ParticleID)
            {
                case 1:

                    for (int i =0; i < ParticleArraySize ; i++)
                    {
                        ParticleData particle = PA[i];
                        float timeAlive = now - particle.BirthTime;
                        particle.NowAge = timeAlive;

                        if (particle.NowAge > ParticleMaxAge || particle.ModColor.A < 0.1f)
                        {
                            particle.IsAlive = false;
                            particle.Accelaration = new Vector2(0,0);
                            particle.Direction = new Vector2(0,0);
                            particle.IsHoming = false;
                            
                        }
                        else
                        {
                            float relativeAge = timeAlive / ParticleMaxAge;
                            particle.Position = 0.5f * particle.Accelaration * relativeAge * relativeAge + particle.Direction * relativeAge + particle.OrginalPosition;

                            float inverseAge = 1.0f - relativeAge;
                            particle.ModColor = new Color(new Vector4(inverseAge, inverseAge + 0.5f, inverseAge, inverseAge - 0.15f));

                            Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                            float distance = positionFromCenter.Length();
                            particle.Scaling = (ParticleScaleFactor1 + distance) / ParticleScaleFactor2;
                            PA[i] = particle;
                        }
                    }
                    break;



                case 2:

                    for (int i = 0; i < ParticleArraySize; i++)
                    {
                        ParticleData particle = PA[i];
                        

                        if (particle.IsAlive == true)
                        {
                            float timeAlive = now - particle.BirthTime;
                            particle.NowAge = timeAlive;

                            if (particle.NowAge > ParticleMaxAge+ ParticleAgeOffset)
                            {
                                particle.IsAlive = false;
                                particle.Accelaration = new Vector2(0, 0);
                                particle.Direction = new Vector2(0, 0);
                                particle.BirthTime = 0;
                                particle.IsHoming = false;
                                PA[i] = particle;
                                continue;

                            }
                            else
                            {
                                float distori = Vector2.Distance(particle.OrginalPosition, particle.Position);
                                float distplayer = Vector2.Distance(P1positon, particle.Position);

                                if (Math.Abs(distori) > 100 || Math.Abs(distplayer) < 200)
                                {
                                    particle.IsHoming = true;
                                    particle.Accelaration = ParticleToPlayerAccelerationScale * Vector2.Normalize(P1positon - particle.Position);
                                    
                                }



                                float relativeAge = timeAlive / ParticleMaxAge;


                                particle.Position = 0.5f * particle.Accelaration * relativeAge * relativeAge + particle.Direction * relativeAge + particle.OrginalPosition;

                                float inverseAge = 1.0f - relativeAge;
                                //particle.ModColor = new Color(new Vector4(inverseAge, inverseAge , inverseAge, inverseAge ));

                                //Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                                //float distance = positionFromCenter.Length();
                                particle.Scaling = ParticleScale;// (ParticleScaleFactor1 + distance) / ParticleScaleFactor2;
                                PA[i] = particle;
                                
                            }
                        }
                    }
                    break;




                default: break;
            }
        }
        


        public int FindQuadrant(Vector2 input)
        {

            if (input.X > 0 && input.Y > 0)
            {
                return 4;
            }
            else if (input.X < 0 && input.Y > 0)
            {
                return 3;
            }
            else if (input.X < 0 && input.Y < 0)
            {
                return 2;
            }
            else if (input.X > 0 && input.Y < 0)
            {
                return 1;
            }

            else
                return 0;
                            
        }
        
         

    }
}
