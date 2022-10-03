namespace CourseWork
{
    using System;
    using System.Drawing;
    using System.IO;

    public class BusinessLogic
    {
        /// <summary>
        /// Інформація про зображення
        /// </summary>
        public ImageInformation ImageInformation { get; private set; }

        /// <summary>
        /// ImAbsDiff
        /// </summary>
        /// <param name="x">Зображення1</param>
        /// <param name="y">Зображення2</param>
        /// <param name="z">Результуюче зображення</param>
        public void ImAbsDiff(Image x, Image y, out Image z)
        {
            // якщо розміри чи формат різний
            if (x.Width != y.Width && x.Height != y.Height && x.RawFormat.Guid != y.RawFormat.Guid)
            {
                throw new Exception($"Зображення {nameof(x)} та {nameof(y)} повинні бути одного розміру та формату.");
            }

            // отримуємо бітові карти зображень
            var result = new Bitmap(x.Width, x.Height);
            using (var xbitmap = new Bitmap(x))
            using (var ybitmap = new Bitmap(y))
            {
                // йдемо по циклу
                for (int i = 0; i < result.Height; i++)
                {
                    for (int j = 0; j < result.Width; j++)
                    {
                        // отримуємо кольори
                        var xcolor = xbitmap.GetPixel(j, i);
                        var ycolor = ybitmap.GetPixel(j, i);

                        // віднімаємо
                        var resultColor = this.SubtractColor(xcolor, ycolor);

                        // ставимо рузультуючий колір
                        result.SetPixel(j, i, resultColor);
                    }
                }
            }

            z = result;
        }

        public void AddImageInformation(string name, string fileName, Image image)
        {
            this.ImageInformation = new ImageInformation
                                        {
                                            Name = name,
                                            Path = fileName,
                                            Format = image.RawFormat,
                                            Width = image.Width,
                                            Height = image.Height,
                                            VerticalResolution = image.VerticalResolution / 2.54f,
                                            HorizontalResolution = image.HorizontalResolution / 2.54f,
                                            PhysicalWidth =
                                                (float)(image.Width / (image.HorizontalResolution
                                                                       / 2.54)),
                                            PhysicalHeight =
                                                (float)(image.Height
                                                        / (image.VerticalResolution / 2.54)),
                                            PixelFormat = image.PixelFormat,
                                            Transparent = new Bitmap(image).GetPixel(1, 1).A,
                                            BitPerPixel = new FileInfo(fileName).Length
                                                        / (image.Width * image.Height)
            };
        }

        /// <summary>
        /// Функція віднімання зображень
        /// </summary>
        /// <param name="color1">1 колір</param>
        /// <param name="color2">2 колір</param>
        /// <returns>Результат</returns>
        private Color SubtractColor(Color color1, Color color2)
        {
            // віднімаємо пікселі
            int r = color1.R - color2.R;
            int g = color1.G - color2.G;
            int b = color1.B - color2.B;

            // нормалізуємо значення
            this.NormalizeColorComponents(ref r, ref g, ref b);

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Функція нормалізації компонентів пікселя (значення не повинно виходити за межі [0; 255])
        /// </summary>
        /// <param name="r">R компонент</param>
        /// <param name="g">G компонент</param>
        /// <param name="b">B компонент</param>
        private void NormalizeColorComponents(ref int r, ref int g, ref int b)
        {
            if (r < 0) r = 0;
            if (r >= 256) r = 255;
            if (g < 0) g = 0;
            if (g >= 256) g = 255;
            if (b < 0) b = 0;
            if (b >= 256) b = 255;
        }
    }
}
