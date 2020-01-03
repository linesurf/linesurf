﻿using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace Linesurf.Framework.UI.Components
{
    public abstract class View
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        ImmutableArray<UIElement> elements;
        
        public int Height => graphicsDevice.Viewport.Height;
        public int Width => graphicsDevice.Viewport.Width;


        protected View(SpriteBatch sb, GraphicsDevice gd)
        {
            spriteBatch = sb;
            graphicsDevice = gd;
            
            //register elements automagically

            var me = typeof(Program).Assembly.DefinedTypes
                .First(info => info.AssemblyQualifiedName == this.GetType().AssemblyQualifiedName);

            foreach (var field in me.DeclaredFields)
            {
                if (field.GetValue(null) is UIElement e)
                {
                    elements.Add(e);
                }    
            }
            
        }
        
    }
}