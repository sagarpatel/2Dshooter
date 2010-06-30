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

    // Struct template for all particles

    public struct ParticleData
    {

        public float BirthTime;
        public float MaxAge;
        public Vector2 OrginalPosition;
        public Vector2 Accelaration;
        public Vector2 Direction;
        public Vector2 Position;
        public float Scaling;
        public Color ModColor;


    }


    // Textures of a given particle will be held seperately, outisde of the struct and only once.
    // The texture variable will be called directly when using SpriteBatch.Draw to draw the particle
    // This will avoid using up memory with the texture
    


    class ParticleEngine
    {
        // Contains all Particle realated functions, except Draw
        // ex: AddParticles(), AddExplosion, etc


        
    }
}
