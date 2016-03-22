//-----------------------------------------------------------------------
// <copyright file="Sprite.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OpenTK.SpriteManager
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using OpenTK.Graphics.OpenGL;

    /// <summary>
    /// Layout for origin.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Layout
    {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        Center,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    /// <summary>
    /// Represents a stored <see cref="Bitmap"/> image.
    /// </summary>
    public struct Sprite : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite" /> struct from a file.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="imageNumber">The image number.</param>
        [JsonConstructor]
        public Sprite(string name, bool transparent, Layout origin, Vector2 imageNumber = new Vector2())
        {
            Name = name;
            Transparent = transparent;
            Origin = origin;
            ImageNumber = Vector2.Max(Vector2.One, imageNumber);

            Id = 0;
            ImageIndex = Vector2.Zero;
            Size = Vector2.Zero;

            // load the sprite
            Load();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite" /> struct from a byte array.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="imageNumber">The image number.</param>
        public Sprite(Vector2 size, byte[] bytes, bool transparent, Layout origin, Vector2 imageNumber = new Vector2())
        {
            Contract.Requires(Size.X > 0 && Size.Y > 0);
            Contract.Requires(bytes.Length == (int)size.X * (transparent ? (int)size.Y * 4 : (int)size.Y * 3));

            Transparent = transparent;
            Origin = origin;
            ImageNumber = Vector2.Max(Vector2.One, imageNumber);
            ImageIndex = Vector2.Zero;

            // generate the texture id
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Id);

            // set the size
            Size = size;

            // create the texture
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                transparent ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb,
                (int)(Size.X * ImageNumber.X),
                (int)(Size.Y * ImageNumber.Y),
                0,
                transparent ? PixelFormat.Bgra : PixelFormat.Bgr,
                PixelType.UnsignedByte,
                bytes);

            Name = "Generated";

            // register the sprite
            SpriteManager.Register(this);

            // set the texture parameters
            SetParameters();
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonIgnore]
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the index of the image.
        /// </summary>
        /// <value>
        /// The index of the image.
        /// </value>
        [JsonIgnore]
        public Vector2 ImageIndex { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>The origin.</value>
        public Layout Origin { get; }

        /// <summary>
        /// Gets or sets the number of images contained in this sprite.
        /// </summary>
        /// <value>
        /// The number of images.
        /// </value>
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 ImageNumber { get; set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        [JsonIgnore]
        public Vector2 Size { get; private set; }

        /// <summary>
        /// Gets or set a value indicating whether this <see cref="Sprite"/> has transparency.
        /// </summary>
        /// <value><c>true</c> if transparent; otherwise, <c>false</c>.</value>
        public bool Transparent { get; set; }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Sprite source, Sprite other)
        {
            return !source.Equals(other);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Sprite source, Sprite other)
        {
            return source.Equals(other);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GL.DeleteTexture(Id);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Draws this instance at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void Draw(Vector2 position)
        {
            Draw(position, Size, Color.White);
        }

        /// <summary>
        /// Draws this instance with the specified parameters.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="color">The color.</param>
        public void Draw(Vector2 position, float scale, Color color)
        {
            Contract.Requires(scale > 0);

            Draw(position, Size * scale, color);
        }

        /// <summary>
        /// Draws this instance with the specified parameters.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        public void Draw(Vector2 position, Vector2 size, Color color)
        {
            Contract.Requires(size.X > 0 && Size.Y > 0);

            var drawPosition = position - (Origin.ToVector2() * size);

            if (color.A < 255 || Transparent)
                GL.Enable(EnableCap.Blend);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(color);

            GL.TexCoord2(0, 0);
            GL.Vertex2(drawPosition.X, drawPosition.Y);

            GL.TexCoord2(1, 0);
            GL.Vertex2(drawPosition.X + size.X, drawPosition.Y);

            GL.TexCoord2(1, 1);
            GL.Vertex2(drawPosition.X + size.X, drawPosition.Y + size.Y);

            GL.TexCoord2(0, 1);
            GL.Vertex2(drawPosition.X, drawPosition.Y + size.Y);

            GL.Color4(Color.White);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);

            GL.LoadIdentity();
        }

        /// <summary>
        /// Draws this instance repeated at the specified positions.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public void DrawRepeated(Vector2 start, Vector2 end)
        {
            DrawRepeated(start, end, Size, Color.White);
        }

        /// <summary>
        /// Draws this instance repeated with the specified parameters.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="color">The color.</param>
        public void DrawRepeated(Vector2 start, Vector2 end, float scale, Color color)
        {
            Contract.Requires(scale > 0);

            DrawRepeated(start, end, Size * scale, color);
        }

        /// <summary>
        /// Draws this instance repeated with the specified parameters.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        public void DrawRepeated(Vector2 start, Vector2 end, Vector2 size, Color color)
        {
            Contract.Requires(size.X > 0 && Size.Y > 0);

            // calculate position differences
            var diff = new Vector2(end.X - start.X, end.Y - start.Y);

            // calculate number of repeats by find the distance between a zero Vector2 and the
            // position difference
            var repeats = (int)Math.Round(Vector2.Zero.Distance(diff / size.X));
            //// int drawAngle = angle + (int)Math.Round(start.Angle(end));
            var step = repeats != 0 ? diff / repeats : Vector2.Zero;

            for (int i = 0; i <= repeats; i++)
                Draw(start + step * i, size, color);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Sprite && Equals((Sprite)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Sprite"/>, is equal to this instance.
        /// </summary>
        /// <param name="sprite">The <see cref="Sprite"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Sprite"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Sprite sprite)
        {
            return Id.Equals(sprite.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Id.ToString() + ':' + Name;
        }

        /// <summary>
        /// Loads the sprite.
        /// </summary>
        public void Load()
        {
            // ensure file exists
            if (!File.Exists(SpriteManager.Directory + Name))
                return;

            // check if this sprite was already loaded
            var id = SpriteManager.FindSprite(Name).Id;

            if (id != 0)
            {
                // set the id to the previously loaded one's
                Id = id;
                return;
            }

            // generate the texture id
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Id);

            using (var bitmap = new Bitmap(SpriteManager.Directory + Name))
            {
                // set the size
                Size = new Vector2(bitmap.Width / ImageNumber.X, bitmap.Height / ImageNumber.Y);

                // load the bitmap data
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                try
                {
                    // create the texture
                    GL.TexImage2D(
                        TextureTarget.Texture2D,
                        0,
                        PixelInternalFormat.Rgba,
                        bitmap.Width,
                        bitmap.Height,
                        0,
                        PixelFormat.Bgra,
                        PixelType.UnsignedByte,
                        bitmapData.Scan0);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }

            // register the sprite
            SpriteManager.Register(this);

            // set the texture parameters
            SetParameters();
        }

        /// <summary>
        /// Sets the texture parameters.
        /// </summary>
        private static void SetParameters()
        {
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);
        }
    }
}