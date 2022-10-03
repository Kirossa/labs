namespace CourseWork
{
    using System.Drawing.Imaging;

    /// <summary>
    /// Клас для зберігання інформації про зображення
    /// </summary>
    public class ImageInformation
    {
        /// <summary>
        /// Ім'я зображення
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Шлях до зображення
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Формат зображення
        /// </summary>
        public ImageFormat Format { get; set; }

        /// <summary>
        /// Ширина зображення
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Висота зображення
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Вертикальне розширення зображення в точка/см
        /// </summary>
        public float VerticalResolution { get; set; }

        /// <summary>
        /// Горизонтальне розширення зображення в точка/см
        /// </summary>
        public float HorizontalResolution { get; set; }

        /// <summary>
        /// Фізична ширина зображення в см
        /// </summary>
        public float PhysicalWidth { get; set; }

        /// <summary>
        /// Фізична висота зображення в см
        /// </summary>
        public float PhysicalHeight { get; set; }

        /// <summary>
        /// Формат пікселів зображенняі
        /// </summary>
        public PixelFormat PixelFormat { get; set; }

        /// <summary>
        /// Біт прозорості зображення
        /// </summary>
        public byte Transparent { get; set; }

        /// <summary>
        /// Кількість біт на піксель
        /// </summary>
        public decimal BitPerPixel { get; set; }

        /// <summary>
        /// Пустий конструктор
        /// </summary>
        public ImageInformation()
        {
        }

        // конструктор з параметрами
        public ImageInformation(
            string name,
            string path,
            ImageFormat format,
            int width,
            int height,
            float verticalResolution,
            float horizontalResolution,
            float physicalWidth,
            float physicalHeight,
            PixelFormat pixelFormat,
            byte transparent,
            decimal bitPerPixel)
        {
            this.Name = name;
            this.Path = path;
            this.Format = format;
            this.Width = width;
            this.Height = height;
            this.VerticalResolution = verticalResolution;
            this.HorizontalResolution = horizontalResolution;
            this.PhysicalWidth = physicalWidth;
            this.PhysicalHeight = physicalHeight;
            this.PixelFormat = pixelFormat;
            this.Transparent = transparent;
            this.BitPerPixel = bitPerPixel;
        }

        /// <summary>
        /// Перевантаження методу ToString для зручного виводу інформації про зображення
        /// </summary>
        /// <returns>Рядок з інформацією про зображення</returns>
        public override string ToString() =>
                $"Ім'я файлу: {this.Name}, \n" +
                $"Шлях до файлу: {this.Path}, \n" +
                $"Формат файлу: {this.ImageFormatToString(this.Format)}, \n" +
                $"Ширина: {this.Width} пікселів, \n" +
                $"Висота: {this.Height} пікселів, \n" +
                $"Вертикальне розширення: {this.VerticalResolution} точок/см, \n" +
                $"Горизонтальне розширення: {this.HorizontalResolution} точок/см, \n" +
                $"Фізична ширина: {this.PhysicalWidth} см, \n" +
                $"Фізична висота: {this.PhysicalHeight} см, \n" +
                $"Формат пікселів: {this.PixelFormatToString(this.PixelFormat)}, \n" +
                $"Формат прозорісті: {this.Transparent}, \n" +
                $"Кількість біт на піксель: {this.BitPerPixel}.";

        /// <summary>
        /// Переведення формату зображення в зручний вигляд
        /// </summary>
        /// <param name="imageFormat">Формат зображення</param>
        /// <returns>Строкове представлення формату</returns>
        private string ImageFormatToString(ImageFormat imageFormat)
        {
            var format = string.Empty;

            if (ImageFormat.Bmp.Equals(imageFormat))
            {
                format = "bmp";
            }

            if (ImageFormat.Emf.Equals(imageFormat))
            {
                format = "emf";
            }

            if (ImageFormat.Gif.Equals(imageFormat))
            {
                format = "gif";
            }

            if (ImageFormat.Icon.Equals(imageFormat))
            {
                format = "icon";
            }

            if (ImageFormat.Jpeg.Equals(imageFormat))
            {
                format = "jpg";
            }

            if (ImageFormat.Png.Equals(imageFormat))
            {
                format = "png";
            }

            if (ImageFormat.Tiff.Equals(imageFormat))
            {
                format = "tiff";
            }

            if (ImageFormat.Wmf.Equals(imageFormat))
            {
                format = "wmf";
            }

            if (format.Length == 0)
            {
                format = "Unknown";
            }

            return format;
        }

        /// <summary>
        /// Переведення формату пікселів в зручний вигляд
        /// </summary>
        /// <param name="pixelFormat">Формат пікселів</param>
        /// <returns>Строкове представлення формату пікселів</returns>
        private string PixelFormatToString(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppArgb1555:
                    return "16bpp ARGB";

                case PixelFormat.Format16bppGrayScale:
                    return "16bpp Gray";

                case PixelFormat.Format16bppRgb555:
                    return "16bpp RGB";

                case PixelFormat.Format16bppRgb565:
                    return "16bpp RGB";

                case PixelFormat.Format1bppIndexed:
                    return "1bpp Indexed";

                case PixelFormat.Format24bppRgb:
                    return "24bpp RGB";

                case PixelFormat.Format32bppArgb:
                    return "32bpp ARGB";

                case PixelFormat.Format32bppPArgb:
                    return "32bpp Premultiplied ARGB";

                case PixelFormat.Format32bppRgb:
                    return "32bpp RGB";

                case PixelFormat.Format48bppRgb:
                    return "48bpp RGB";

                case PixelFormat.Format4bppIndexed:
                    return "4bpp Indexed";

                case PixelFormat.Format64bppArgb:
                    return "64bpp ARGB";

                case PixelFormat.Format64bppPArgb:
                    return "64bpp Premultiplied ARGB";

                case PixelFormat.Format8bppIndexed:
                    return "8bpp Indexed";

                default:
                    return pixelFormat.ToString();
            }
        }
    }
}