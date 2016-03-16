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
    using Epic.Vectors;
    using Newtonsoft.Json;
    using OpenTK.Graphics.OpenGL;

    /// <summary>
    /// Layout for origin.
    /// </summary>
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
        /// Initializes a new instance of the <see cref="Sprite"/> struct from a file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        public Sprite(string filename, bool transparent, Vector2<int> origin)
        {
            Name = filename;
            Transparent = transparent;
            Origin = origin;

            Id = 0;
            Size = Vector2.Create(0, 0);

            Load(filename);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite" /> struct from a file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        public Sprite(string filename, bool transparent, Layout origin)
        {
            Name = filename;
            Transparent = transparent;
            Origin = Vector2.Create(0, 0);

            Id = 0;
            Size = Vector2.Create(0, 0);

            // load the file
            Load(filename);

            // change origin according to the layout
            switch (origin)
            {
                case Layout.TopLeft:
                    Origin = Vector2.Create(0, 0);
                    break;
                case Layout.TopCenter:
                    Origin = Vector2.Create(Size.X / 2, 0);
                    break;
                case Layout.TopRight:
                    Origin = Vector2.Create(Size.X, 0);
                    break;
                case Layout.CenterLeft:
                    Origin = Vector2.Create(0, Size.Y / 2);
                    break;
                case Layout.Center:
                    Origin = Vector2.Create(Size.X / 2, Size.Y / 2);
                    break;
                case Layout.CenterRight:
                    Origin = Vector2.Create(Size.X, Size.Y / 2);
                    break;
                case Layout.BottomLeft:
                    Origin = Vector2.Create(0, Size.Y);
                    break;
                case Layout.BottomCenter:
                    Origin = Vector2.Create(Size.X / 2, Size.Y);
                    break;
                case Layout.BottomRight:
                    Origin = Vector2.Create(Size.X, Size.Y);
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> struct from a byte array.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="transparent">if set to <c>true</c> uses transparency.</param>
        /// <param name="origin">The origin.</param>
        public Sprite(Vector2<int> size, byte[] bytes, bool transparent, Vector2<int> origin = null)
        {
            Contract.Requires(Size.X > 0 && Size.Y > 0);
            Contract.Requires(bytes.Length == size.X * (transparent ? size.Y * 4 : size.Y * 3));

            Transparent = transparent;

            Origin = origin ?? Vector2.Create(0, 0);

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
                Size.X,
                Size.Y,
                0,
                transparent ? PixelFormat.Bgra : PixelFormat.Bgr,
                PixelType.UnsignedByte,
                bytes);

            Name = "Generated";

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
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the origin.
        /// </summary>
        /// <value>The origin.</value>
        [JsonIgnore]
        public Vector2<int> Origin { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        [JsonIgnore]
        public Vector2<int> Size { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Sprite"/> has transparency.
        /// </summary>
        /// <value><c>true</c> if transparent; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool Transparent { get; }

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
        public void Draw(Vector2<int> position)
        {
            Draw(position, Size, Color.White);
        }

        /// <summary>
        /// Draws this instance with the specified parameters.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="color">The color.</param>
        public void Draw(Vector2<int> position, float scale, Color color)
        {
            Contract.Requires(scale > 0);

            Draw(position, Size.Multiply(scale), color);
        }

        /// <summary>
        /// Draws this instance with the specified parameters.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        public void Draw(Vector2<int> position, Vector2<int> size, Color color)
        {
            Contract.Requires(size.X > 0 && Size.Y > 0);

            var drawPosition = position - (Origin * Vector2.Create(size.X / Size.X, size.Y / Size.Y));

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
        public void DrawRepeated(Vector2<int> start, Vector2<int> end)
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
        public void DrawRepeated(Vector2<int> start, Vector2<int> end, float scale, Color color)
        {
            Contract.Requires(scale > 0);

            DrawRepeated(start, end, Size.Multiply(scale), color);
        }

        /// <summary>
        /// Draws this instance repeated with the specified parameters.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        public void DrawRepeated(Vector2<int> start, Vector2<int> end, Vector2<int> size, Color color)
        {
            Contract.Requires(size.X > 0 && Size.Y > 0);

            // calculate position differences
            var diff = Vector2.Create(end.X - start.X, end.Y - start.Y);

            // calculate number of repeats by find the distance between a zero Vector2 and the
            // position difference
            var repeats = (int)Math.Round(Vector2.Create(0, 0).Distance(diff / size.X));
            //// int drawAngle = angle + (int)Math.Round(start.Angle(end));
            var step = repeats != 0 ? diff / repeats : Vector2.Create(0, 0);

            for (int i = 0; i <= repeats; i++)
                Draw(start + step.Multiply(i), size, color);
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
        /// <param name="texture">The <see cref="Sprite"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Sprite"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Sprite texture)
        {
            return Id.Equals(texture.Id);
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
        /// Loads the sprite from the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void Load(string filename)
        {
            Name = filename;

            // generate the texture id
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Id);

            using (var bitmap = new Bitmap(SpriteManager.Directory + filename))
            {
                // set the size
                Size = Vector2.Create(bitmap.Width, bitmap.Height);

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
                        Size.X,
                        Size.Y,
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