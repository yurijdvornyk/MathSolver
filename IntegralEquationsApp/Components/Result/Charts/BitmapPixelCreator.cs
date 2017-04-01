using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IntegralEquationsApp.Components.Result.Charts
{
    public class BitmapPixelCreator
    {
        private int width;
        private int height;
        private byte[] pixels;
        private int stride;
        public BitmapPixelCreator(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new byte[width * height * 4];
            stride = width * 4;
        }

        public void GetPixel(int x, int y, out byte red, out byte green, out byte blue, out byte alpha)
        {
            int index = y * stride + x * 4;
            blue = pixels[index++];
            green = pixels[index++];
            red = pixels[index++];
            alpha = pixels[index];
        }

        public byte GetBlue(int x, int y)
        {
            return pixels[y * stride + x * 4];
        }

        public byte GetGreen(int x, int y)
        {
            return pixels[y * stride + x * 4 + 1];
        }

        public byte GetRed(int x, int y)
        {
            return pixels[y * stride + x * 4 + 2];
        }

        public byte GetAlpha(int x, int y)
        {
            return pixels[y * stride + x * 4 + 3];
        }

        public void SetPixel(int x, int y, byte red, byte green, byte blue, byte alpha)
        {
            int index = y * stride + x * 4;
            pixels[index++] = blue;
            pixels[index++] = green;
            pixels[index++] = red;
            pixels[index++] = alpha;
        }

        public void SetBlue(int x, int y, byte blue)
        {
            pixels[y * stride + x * 4] = blue;
        }

        public void SetGreen(int x, int y, byte green)
        {
            pixels[y * stride + x * 4 + 1] = green;
        }

        public void SetRed(int x, int y, byte red)
        {
            pixels[y * stride + x * 4 + 2] = red;
        }

        public void SetAlpha(int x, int y, byte alpha)
        {
            pixels[y * stride + x * 4 + 3] = alpha;
        }

        public void SetColor(byte red, byte green, byte blue, byte alpha)
        {
            int num_bytes = width * height * 4;
            int index = 0;
            while (index < num_bytes)
            {
                pixels[index++] = blue;
                pixels[index++] = green;
                pixels[index++] = red;
                pixels[index++] = alpha;
            }
        }

        public void SetColor(byte red, byte green, byte blue)
        {
            SetColor(red, green, blue, 255);
        }

        public WriteableBitmap MakeBitmap(double dpiX, double dpiY)
        {
            WriteableBitmap wbitmap = new WriteableBitmap(width, height, dpiX, dpiY, PixelFormats.Bgra32, null);
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            wbitmap.WritePixels(rect, pixels, stride, 0);
            return wbitmap;
        }
    }
}
