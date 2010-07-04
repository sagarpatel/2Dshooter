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

        public float ParticleScale = 0.5f; // Used in AddExplosionParticle()
        public float ParticleAcceleration = 3.0f; // Used in AddExplosionParticle()
        public float ExplosionSize = 30f;
        public float ParticleMaxAge = 750.0f;
        public int MaxParticles = 80; // U

        Random random = new Random(); // Will be used later for random generation
        
        Texture2D Explosion1Sprite;
               
        SpriteBatch spriteBatch;


        public ParticleEngine(SpriteBatch SB, Texture2D EXP1Tex)
        {
            Particle1List = new List<ParticleData>() ;

            Explosion1Sprite = EXP1Tex;

            spriteBatch = SB;
        }


        public void AddExplosionParticle(List<ParticleData> PL, Vector2 ExplosionPosition, float ExplosionSize, float MaxAge, GameTime gametime)
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
            particle.Accelaration = ParticleAcceleration * particle.Direction;

            particle.Direction = Displacement * 2.0f;
            particle.Accelaration = -particle.Direction;


            PL.Add(particle);


        }


        public void AddExplosion(List<ParticleData> PL, int MaxParticles, Vector2 ExplosionPosition, float ExplosionSize, float MaxAge, GameTime gametime)
        {
            for (int i = 0; i < MaxParticles; i++)
            {
                AddExplosionParticle(PL, ExplosionPosition, ExplosionSize, MaxAge, gametime);

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
                                 new Vector2(256,256),
                                 particle.Scaling,
                                 SpriteEffects.None,
                                 1);

            }
        }


        public void UpdateParticles(List<ParticleData> PL, GameTime gameTime)
        {

            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;

            for (int i = PL.Count - 1; i >= 0; i--)
            {
                ParticleData particle = PL[i];
                float timeAlive = now - particle.BirthTime;
                particle.NowAge = timeAlive;

                if (particle.NowAge > particle.MaxAge)
                {
                    PL.RemoveAt(i);
                }
                else
                {
                    float relativeAge = timeAlive / particle.MaxAge;
                    particle.Position = 0.5f * particle.Accelaration * relativeAge * relativeAge + particle.Direction * relativeAge + particle.OrginalPosition;

                    float inverseAge = 1.0f - relativeAge;
                    particle.ModColor = new Color(new Vector4(inverseAge, inverseAge+0.5f, inverseAge, inverseAge-0.1f));

                    Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                    float distance = positionFromCenter.Length();
                    particle.Scaling = (20.0f + distance) / 200.0f;
                    PL[i] = particle;
                }
            }
        }

         

    }
}
