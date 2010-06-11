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
    class GameObject
    {
        public Texture2D sprite;

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;

        public float rotation;

        public float scale; 
        
        public Vector2 center;
        
        public bool alive;

        public float mass;

        public bool IsPlayer;

        public float acceleration_clamp;
        public float velocity_clamp;

        public float friction;

        public int HP;

        
        public GameObject(Texture2D loadedTexture)
        {
            sprite = loadedTexture;

            position = Vector2.Zero;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;

      
            rotation = 0.0f;
            
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            
            alive = false;
            IsPlayer = false;

            mass = 0;
        }


    }
}
