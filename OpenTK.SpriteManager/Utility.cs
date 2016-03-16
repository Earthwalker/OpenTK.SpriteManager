//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OpenTK.SpriteManager
{
    using System;
    using Epic.Vectors;

    /// <summary>
    /// Utility methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="source">The source <see cref="Vector2"/>.</param>
        /// <param name="other">The other <see cref="Vector2"/>.</param>
        /// <returns>The distance.</returns>
        public static double Distance(this Vector2<int> source, Vector2<int> other)
        {
            // d = √ (x2-x1)^2 + (y2-y1)^2
            return Math.Sqrt(Math.Pow(other.X - source.X, 2) + Math.Pow(other.Y - source.Y, 2));
        }

        /// <summary>
        /// Multiplies the specified Vector2s.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result.</returns>
        public static Vector2<int> Multiply(this Vector2<int> source, int value)
        {
            return Vector2.Create(source.X * value, source.Y * value);
        }

        /// <summary>
        /// Multiplies the specified Vector2s.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result.</returns>
        public static Vector2<int> Multiply(this Vector2<int> source, float value)
        {
            return Vector2.Create((int)(source.X * value), (int)(source.Y * value));
        }
    }
}