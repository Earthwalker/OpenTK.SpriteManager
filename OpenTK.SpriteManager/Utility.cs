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
    }
}
