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
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] menuItems;
        int selectedIndex;
        Color normal = Color.White;
        Color hilite = Color.Gold;

        KeyboardState keyboardState;
        KeyboardState keyboardState_before;
        GamePadState gamepadState;
        GamePadState gamepadState_before;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Vector2 position;
        float width = 0f;
        float height = 0f;

        //bool MenuIsDrawn = true;
        
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {

            selectedIndex = value;
            if (selectedIndex < 0)
                selectedIndex = 0;
            if (selectedIndex >= menuItems.Length)
                selectedIndex = menuItems.Length - 1;
            }
        }

        public MenuComponent(Game game,SpriteBatch spriteBatch,SpriteFont spriteFont,string[] menuItems) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.menuItems = menuItems;
            MeasureMenu();
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(item);
                if (size.X > width)
                width = size.X;
                height += spriteFont.LineSpacing + 5;
            }
            position = new Vector2(
                (Game.Window.ClientBounds.Width - width) / 2,
                (Game.Window.ClientBounds.Height - height) / 2);
        }



        public override void Initialize()
        {
            base.Initialize();
        }


        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
            keyboardState_before.IsKeyDown(theKey);
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);


            if (CheckKey(Keys.Down) || ((gamepadState.ThumbSticks.Left.Y > 0.0f) && (gamepadState_before.ThumbSticks.Left.Y == 0.0f)) )
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                    selectedIndex = 0;
            }
            if (CheckKey(Keys.Up) || ((gamepadState.ThumbSticks.Left.Y < 0.0f) && (gamepadState_before.ThumbSticks.Left.Y == 0.0f)))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                selectedIndex = menuItems.Length - 1;
            }

            //if (selectedIndex == 0 && CheckKey(Keys.Enter))
            //{
            //    MenuIsDrawn = false;
            //}


            base.Update(gameTime);

            keyboardState_before = keyboardState;
            gamepadState_before = gamepadState;
        }

        public override void Draw(GameTime gameTime)
        {
            //if (MenuIsDrawn)
            //{
                
                //base.Draw(gameTime);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

                Vector2 location = position;
                Color tint;
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedIndex)
                        tint = hilite;
                    else
                        tint = normal;
                    spriteBatch.DrawString(
                    spriteFont,
                    menuItems[i],
                    location,
                    tint);
                    location.Y += spriteFont.LineSpacing + 5;
                }

                spriteBatch.End();

            //}

            


        }



    }
}