//-----------------------------------------------------------------------
// <copyright file="SpriteManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OpenTK.SpriteManager
{
    using System;
    using System.Collections.Generic;
    using Epic.Vectors;

    /// <summary>
    /// Manages the <see cref="Sprite"/> s used by the application.
    /// </summary>
    public class SpriteManager : IDisposable
    {
        /// <summary>
        /// The sprites.
        /// </summary>
        private readonly List<Sprite> sprites = new List<Sprite>();

        /// <summary>
        /// Gets the sprite directory.
        /// </summary>
        /// <value>The sprite directory.</value>
        public static string Directory { get; } = Environment.CurrentDirectory + "\\Textures\\";

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        public void Dispose(bool managed)
        {
            if (managed)
            {
                foreach (var sprite in sprites)
                    sprite.Dispose();
            }
        }

        /// <summary>
        /// Loads a new <see cref="Sprite"/> from the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        /// <returns>The new <see cref="Sprite"/>.</returns>
        public Sprite Load(string filename, bool transparent, Vector2<int> origin = null)
        {
            // check if this sprite was already loaded
            var sprite = FindSprite(Directory + filename);

            if (sprite != default(Sprite))
                return sprite;

            // load the sprite
            sprite = new Sprite(filename, transparent, origin);

            // register the new sprite
            Register(sprite);

            return sprite;
        }

        /// <summary>
        /// Loads a new <see cref="Sprite"/> from the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        /// <returns>The new <see cref="Sprite"/>.</returns>
        public Sprite Load(string filename, bool transparent, Layout origin = Layout.TopLeft)
        {
            // check if this sprite was already loaded
            var sprite = FindSprite(Directory + filename);

            if (sprite != default(Sprite))
                return sprite;

            // load the sprite
            sprite = new Sprite(filename, transparent, origin);

            // register the new sprite
            Register(sprite);

            return sprite;
        }

        /// <summary>
        /// Finds the sprite with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The sprite.</returns>
        public Sprite FindSprite(int id)
        {
            return sprites.Find(t => t.Id == id);
        }

        /// <summary>
        /// Finds the sprite with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The sprite.</returns>
        public Sprite FindSprite(string name)
        {
            return sprites.Find(t => t.Name == name);
        }

        /// <summary>
        /// Registers the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public void Register(Sprite sprite)
        {
            if (!sprites.Contains(sprite))
                sprites.Add(sprite);
        }

        /// <summary>
        /// Unregisters the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="dispose">if set to <c>true</c> dispose the sprite.</param>
        public void Unregister(Sprite sprite, bool dispose = true)
        {
            sprites.Remove(sprite);

            if (dispose)
                sprite.Dispose();
        }
    }
}