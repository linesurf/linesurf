﻿using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Linesurf.Framework.UI.Organization;
namespace Linesurf.Framework.UI.Components
{
    public abstract class UIElement
    {
        protected UIContainer parent = default!;
        protected Rectangle elementRect;
        protected string xPosition = default!;
        protected string yPosition = default!;
        protected string width = default!;
        protected string height = default!;

        public Rectangle ElementRect
        {
            get => elementRect;
        }

        public AnchorPoint AnchorPoint;

        public string XPosition
        {
            get => xPosition;
            set
            {
                xPosition = value;
                ParseRectangle();
            }
        }

        public string YPosition
        {
            get => yPosition;
            set
            {
                yPosition = value;
                ParseRectangle();
            }
        }

        public string Width
        {
            get => width;
            set
            {
                width = value;
                ParseRectangle();
            }
        }

        public string Height
        {
            get => height;
            set
            {
                height = value;
                ParseRectangle();
            }
        }

        protected unsafe void ParseRectangle()
        {
            int x, y, height = 0, width = 0, i;
            //xpos

            if (int.TryParse(xPosition, NumberStyles.Integer, CultureInfo.InvariantCulture, out i))
            {
                if (i < 0) x = parent.ElementRect.Width - i;
                else x = i;
            }
            else if (xPosition.EndsWith('%'))
            {
                var s = xPosition.Split('%')[0];
                i = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                if (i < 0 || i > 100) throw new FormatException("Percentage should be between 0% and 100%");
                x = (int) (parent.ElementRect.Width * (i / 100f));
            }

            //ypos
            if (int.TryParse(yPosition, NumberStyles.Integer, CultureInfo.InvariantCulture, out i))
            {
                if (i < 0) y = parent.ElementRect.Height - i;
                else y = i;
            }
            else if (yPosition.EndsWith('%'))
            {
                var s = yPosition.Split('%')[0];
                i = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                if (i < 0 || i > 100) throw new FormatException("Percentage should be between 0% and 100%");
                y = (int) (parent.ElementRect.Height * (i / 100f));
            }

            //need to implement size Lol
            _ = &width;
            _ = &height;
        }
    }
}