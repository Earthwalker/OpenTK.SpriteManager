//-----------------------------------------------------------------------
// <copyright file="SpriteManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OpenTK.SpriteManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages the <see cref="Sprite"/> s used by the application.
    /// </summary>
    public static class SpriteManager
    {
        /// <summary>
        /// The sprites.
        /// </summary>
        private static readonly List<Sprite> sprites = new List<Sprite>();

        /// <summary>
        /// Gets the sprite directory.
        /// </summary>
        /// <value>The sprite directory.</value>
        public static string Directory { get; } = Environment.CurrentDirectory + "\\Sprites\\";

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public static void Dispose()
        {
            foreach (var sprite in sprites)
                sprite.Dispose();
        }

        /// <summary>
        /// Finds the sprite with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The sprite.</returns>
        public static Sprite FindSprite(int id)
        {
            return sprites.Find(t => t.Id == id);
        }

        /// <summary>
        /// Finds the sprite with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The sprite.</returns>
        public static Sprite FindSprite(string name)
        {
            return sprites.Find(s => s.Name == name);
        }

        /// <summary>
        /// Registers the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public static void Register(Sprite sprite)
        {
            if (!sprites.Contains(sprite))
                sprites.Add(sprite);
        }

        /// <summary>
        /// Unregisters the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public static void Unregister(Sprite sprite)
        {
            // remove the sprite from the collection
            sprites.Remove(sprite);

            // dispose of the sprite
            sprite.Dispose();
        }
    }
}