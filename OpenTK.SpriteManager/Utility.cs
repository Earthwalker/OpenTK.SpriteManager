//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OpenTK.SpriteManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Utility functions.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="source">The source <see cref="Vector2"/>.</param>
        /// <param name="other">The other <see cref="Vector2"/>.</param>
        /// <returns>The distance.</returns>
        public static double Distance(this Vector2 source, Vector2 other)
        {
            // d = √ (x2-x1)^2 + (y2-y1)^2
            return Math.Sqrt(Math.Pow(other.X - source.X, 2) + Math.Pow(other.Y - source.Y, 2));
        }

        /// <summary>
        /// Returns the <see cref="Vector2"/> equivalent of the specified <see cref="Layout"/>.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        public static Vector2 ToVector2(this Layout layout)
        {
            switch (layout)
            {
                case Layout.TopLeft:
                    return Vector2.Zero;
                case Layout.TopCenter:
                    return new Vector2(.5f, 0);
                case Layout.TopRight:
                    return new Vector2(1, 0);
                case Layout.CenterLeft:
                    return new Vector2(0, .5f);
                case Layout.Center:
                    return new Vector2(.5f, .5f);
                case Layout.CenterRight:
                    return new Vector2(1, .5f);
                case Layout.BottomLeft:
                    return new Vector2(0, 1);
                case Layout.BottomCenter:
                    return new Vector2(.5f, 1);
                case Layout.BottomRight:
                    return Vector2.One;
                default:
                    return Vector2.Zero;
            }
        }
    }
}
