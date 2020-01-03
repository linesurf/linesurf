using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Linesurf.Framework.UI.Positioning;
namespace Linesurf.Framework.UI.Components
{
    public abstract class UIElement
    {
        protected View viewParent = default!;
        protected Rectangle elementRect;
        protected string xPosition = default!;
        protected string yPosition = default!;
        protected string width = default!;
        protected string height = default!;

        public Rectangle ElementRect => elementRect;

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
            int x, y, h, w, i;
            //xpos

            if (int.TryParse(xPosition, NumberStyles.Integer, CultureInfo.InvariantCulture, out i))
            {
                if (i < 0) x = viewParent.Width - i;
                else x = i;
            }
            else if (xPosition.EndsWith('%'))
            {
                var s = xPosition.Split('%')[0];
                i = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                if (i < 0 || i > 100) throw new FormatException("Percentage should be between 0% and 100%");
                x = (int) (viewParent.Width * (i / 100f));
            }
            else throw new FormatException("Invalid position format");
                

            //ypos
            if (int.TryParse(yPosition, NumberStyles.Integer, CultureInfo.InvariantCulture, out i))
            {
                if (i < 0) y = viewParent.Height - i;
                else y = i;
            }
            else if (yPosition.EndsWith('%'))
            {
                var s = yPosition.Split('%')[0];
                i = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                if (i < 0 || i > 100) throw new FormatException("Percentage should be between 0% and 100%");
                y = (int) (viewParent.Height * (i / 100f));
            }
            else throw new FormatException("Invalid position format");

            
            //width
            if (width == "min")
            {
                w = GetMinElementSize().Width;
            }
            else if (int.TryParse(width, NumberStyles.Integer, CultureInfo.InvariantCulture, out i))
            {
                w = i;
            }
            else if (width.EndsWith('%'))
            {
                var s = width.Split('%')[0];
                i = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                if (i < 0 || i > 100) throw new FormatException("Percentage should be between 0% and 100%");
                w = (int) (viewParent.Width * (i / 100f));
            }
            else throw new FormatException("Invalid size format");

            
            //height
            if (height == "min")
            {
                h = GetMinElementSize().Height;
            }
            else if (int.TryParse(width, NumberStyles.Integer, CultureInfo.InvariantCulture, out i))
            {
                h = i;
            }
            else if (width.EndsWith('%'))
            {
                var s = height.Split('%')[0];
                i = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                if (i < 0 || i > 100) throw new FormatException("Percentage should be between 0% and 100%");
                h = (int) (viewParent.Height * (i / 100f));
            }
            else throw new FormatException("Invalid size format");


            elementRect = new Rectangle(x,y,w,h);
        }

        protected abstract Rectangle GetMinElementSize();
    }
}