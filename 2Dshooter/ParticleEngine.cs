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
            public float MaxAge;
            public float NowAge;
            public Vector2 OrginalPosition;
            public Vector2 Accelaration;
            public Vector2 Direction;
            public Vector2 Position;
            public float Scaling;
            public Color ModColor;

        }


        public List<ParticleData> Particle1List;

        public float ParticleScale = 0.0f; // Used in AddExplosionParticle()
        public float ParticleScaleFactor1 = 0f;
        public float ParticleScaleFactor2 = 0f;
        public float ParticleVectorX = 64;
        public float ParticleVectorY = 64;
        public float ParticleAcceleration = 1f; // Used in AddExplosionParticle()
        public float ExplosionSize = 40f;
        public float ParticleMaxAge = 350.0f;
        public int MaxParticles = 20; // U

        public int ParticleID = 0;

        Random random = new Random(); // Will be used later for random generation
        
        Texture2D Explosion1Sprite;
               
        SpriteBatch spriteBatch;


        public ParticleEngine(SpriteBatch SB, Texture2D EXP1Tex, int PID)
        {
            Particle1List = new List<ParticleData>();

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
                    MaxParticles = 20; // U

                    break;

                case 2:
                    ParticleScale = 0.0f; // Used in AddExplosionParticle()
                    ParticleScaleFactor1 = 10f;
                    ParticleScaleFactor2 = 50f;
                    ParticleVectorX = 0 ;
                    ParticleVectorY = 0;
                    ParticleAcceleration = 0.0f; // Used in AddExplosionParticle()
                    ExplosionSize = 200f;
                    ParticleMaxAge = 600.0f;
                    MaxParticles = 500; // U

                    break;


                default: break;
            }

        }





            
        public void AddExplosionParticle1(List<ParticleData> PL, Vector2 ExplosionPosition, float ExplosionSize, float MaxAge, GameTime gametime)
        {

            ParticleData particle = new ParticleData();

            particle.OrginalPosition = ExplosionPosition;
            particle.Position = particle.OrginalPosition;

            particle.BirthTime = (float)gametime.TotalGameTime.TotalMilliseconds;
            particle.MaxAge = MaxAge;
            particle.Scaling = ParticleScale;
            particle.ModColor = Color.White;

            float ParticleDistance = (float)random.NextDouble() * ExplosionSize;
            Vector2 Displacement = new Vector2(ParticleDistance);

            float angle = MathHelper.ToRadians(random.Next(360));
            Displacement = Vector2.Transform(Displacement, Matrix.CreateRotationZ(angle));

            particle.Direction = Displacement;
            particle.Accelaration = -ParticleAcceleration * particle.Direction;
            
            PL.Add(particle);
            

        }


        public void AddExplosionParticle2(List<ParticleData> PL, Vector2 ExplosionPosition, float ExplosionSize, float MaxAge, GameTime gametime, Vector2 ImpactVelocity)
        {

            ParticleData particle = new ParticleData();

            particle.OrginalPosition = ExplosionPosition;
            particle.Position = particle.OrginalPosition;

            particle.BirthTime = (float)gametime.TotalGameTime.TotalMilliseconds;
            particle.MaxAge = MaxAge;
            //particle.Scaling = ParticleScale;
            particle.ModColor = Color.White;

            float ParticleDistance = (float)random.NextDouble() * ExplosionSize;
            Vector2 Displacement = new Vector2(ParticleDistance,0);


            Vector2 IV = ImpactVelocity;

            Vector2 IVnorm = Vector2.Normalize(IV);

            float angle = (float)Math.Atan( (IVnorm.Y / IVnorm.X));
            angle = MathHelper.ToDegrees(angle);



            float tanangle = Math.Abs(angle);
            float angleRangeMin=0;
            float angleRangeMax=0;
            

            int quadrant = FindQuadrant(IVnorm);

            switch (quadrant)
            {
                case 1:
                    //angle = angle;
                    angleRangeMin = 0;
                    angleRangeMax = tanangle + tanangle;
                    if (IVnorm.X == 1)
                    {
                        angleRangeMin = 45;
                        angleRangeMax = 360 - 45;
                    }
                    
                    break;

                case 2:
                    angle = 180 - angle;
                    angleRangeMin = angle - tanangle;
                    angleRangeMax = 180;
                    break;

                case 3:
                    angle = 180 + angle;
                    angleRangeMin = 180;
                    angleRangeMax = angle + tanangle;
                    break;

                case 4:
                    angle = 360 - angle;
                    angleRangeMin = angle - tanangle;
                    angleRangeMax = 360;
                    break;
                    

                default:
                    if (angle == 0 || angle == 360)
                    {
                        angleRangeMin = 45;
                        angleRangeMax = 315;
                    }
                    else if (angle == 90)
                    {
                        angleRangeMin = 45;
                        angleRangeMax = 135;
                    }
                    else if (angle == 180)
                    {
                        angleRangeMin = 135;
                        angleRangeMax = 225;
                    }
                    else if (angle == 270)
                    {
                        angleRangeMin = 225;
                        angleRangeMax = 315;
                    }



                    break;
            }


            // Generate radom angle between Min and Max, then feed to isplacment code below
            float randomangle = random.Next((int)angleRangeMin, (int)angleRangeMax);
            randomangle = MathHelper.ToRadians(randomangle);

            if(angle == 0)
            {
                randomangle = (2*(float)random.Next(785398)/1000000);
                randomangle = randomangle - (float)0.785398;

            }


            //For now, Displacement the usual-> relative to ExplosionSize, later can change to be magnitude of IV by using Vector2.Distance

            Displacement = Vector2.Transform(Displacement, Matrix.CreateRotationZ(randomangle));

            particle.Direction = Displacement;
            particle.Accelaration = -ParticleAcceleration * particle.Direction;

            PL.Add(particle);


        }



        public void AddExplosion(List<ParticleData> PL, int MaxParticles, Vector2 ExplosionPosition, float ExplosionSize, float MaxAge, GameTime gametime, Vector2 ImpactVelocity)
        {
            switch (ParticleID)
            {
                case 1:

                    for (int i = 0; i < MaxParticles; i++)
                    {
                        AddExplosionParticle1(PL, ExplosionPosition, ExplosionSize, MaxAge, gametime);
                    }
                    break;

                case 2:

                    for (int i = 0; i < MaxParticles; i++)
                    {
                        AddExplosionParticle2(PL, ExplosionPosition, ExplosionSize, MaxAge, gametime, ImpactVelocity);
                    }
                    break;


                default: break;
            }
                    

        }


         

        public void DrawExplosion(List<ParticleData> PL, SpriteBatch spriteBatch)
        {


            for (int i = 0; i < PL.Count; i++)
            {
                ParticleData particle = PL[i];
                spriteBatch.Draw(Explosion1Sprite,
                                 particle.Position,
                                 null,
                                 particle.ModColor,
                                 i,
                                 new Vector2(ParticleVectorX,ParticleVectorY),
                                 particle.Scaling,
                                 SpriteEffects.None,
                                 1);

            }
            
        }


        public void UpdateParticles(List<ParticleData> PL, GameTime gameTime)
        {


            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;


            switch (ParticleID)
            {
                case 1:

                    for (int i = PL.Count - 1; i >= 0; i--)
                    {
                        ParticleData particle = PL[i];
                        float timeAlive = now - particle.BirthTime;
                        particle.NowAge = timeAlive;

                        if (particle.NowAge > particle.MaxAge || particle.ModColor.A < 0.1f)
                        {
                            PL.RemoveAt(i);
                        }
                        else
                        {
                            float relativeAge = timeAlive / particle.MaxAge;
                            particle.Position = 0.5f * particle.Accelaration * relativeAge * relativeAge + particle.Direction * relativeAge + particle.OrginalPosition;

                            float inverseAge = 1.0f - relativeAge;
                            particle.ModColor = new Color(new Vector4(inverseAge, inverseAge + 0.5f, inverseAge, inverseAge - 0.15f));

                            Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                            float distance = positionFromCenter.Length();
                            particle.Scaling = (ParticleScaleFactor1 + distance) / ParticleScaleFactor2;
                            PL[i] = particle;
                        }
                    }
                    break;



                case 2:

                    for (int i = PL.Count - 1; i >= 0; i--)
                    {
                        ParticleData particle = PL[i];
                        float timeAlive = now - particle.BirthTime;
                        particle.NowAge = timeAlive;

                        if (particle.NowAge > particle.MaxAge || particle.ModColor.A < 0.1f)
                        {
                            PL.RemoveAt(i);
                        }
                        else
                        {
                            float relativeAge = timeAlive / particle.MaxAge;
                            particle.Position = 0.5f * particle.Accelaration * relativeAge * relativeAge + particle.Direction * relativeAge + particle.OrginalPosition;

                            float inverseAge = 1.0f - relativeAge;
                            particle.ModColor = new Color(new Vector4(inverseAge, inverseAge + 0.5f, inverseAge, inverseAge - 0.15f));

                            //Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                            //float distance = positionFromCenter.Length();
                            particle.Scaling = 0.5f;// (ParticleScaleFactor1 + distance) / ParticleScaleFactor2;
                            PL[i] = particle;
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
