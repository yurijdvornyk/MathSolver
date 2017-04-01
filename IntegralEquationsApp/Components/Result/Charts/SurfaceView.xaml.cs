using IntegralEquationsApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace IntegralEquationsApp.Components.Result.Charts
{
    /// <summary>
    /// Interaction logic for SurfaceView.xaml
    /// </summary>
    public partial class SurfaceView : UserControl
    {
        public double[,] Data { get; private set; }

        //private const string TEMP_IMAGE_FILE = "texture.png";
        private const double cameraVerticalChange = 0.1;
        private const double cameraHorizontalChange = 0.1;
        private const double cameraScalingChange = 0.1;
        private double cameraVerticalPosition;
        private double cameraHorizontalPosition;
        private double cameraDistance;

        private Dictionary<Point3D, int> pointDictionary;
        private Model3DGroup mainModel3Dgroup;
        private PerspectiveCamera camera;
        private int xmin;
        private int xmax;
        private int dx;
        private int zmin;
        private int zmax;
        private int dz;
        private double textureScaleX, textureScaleZ;
        private WriteableBitmap surfaceImage;

        public SurfaceView()
        {
            InitializeComponent();
            cameraVerticalPosition = Math.PI / 6.0;
            cameraHorizontalPosition = Math.PI / 6.0;
            cameraDistance = 17.0;
        }

        public void Update(double[,] data)
        {
            mainModel3Dgroup = new Model3DGroup();
            pointDictionary = new Dictionary<Point3D, int>();
            SetData(data);
            Show3dSurface();
        }

        public void SetData(double[,] data)
        {
            Data = data;
            xmin = 0;
            xmax = data.GetUpperBound(0);
            dx = 1;
            zmin = 0;
            zmax = data.GetUpperBound(1);
            dz = 1;
            textureScaleX = (xmax - xmin);
            textureScaleZ = (zmax - zmin);
        }

        public void Show3dSurface()
        {
            camera = new PerspectiveCamera();
            camera.FieldOfView = 60;
            viewport.Camera = camera;
            positionCamera();
            initLights();
            сreateAltitudeMap(Data);
            defineModel(mainModel3Dgroup, Data);
            ModelVisual3D modelVisual = new ModelVisual3D();
            modelVisual.Content = mainModel3Dgroup;
            viewport.Children.Add(modelVisual);
        }

        public void OnKeyDown(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    cameraVerticalPosition += cameraVerticalChange;
                    if (cameraVerticalPosition > Math.PI / 2.0) cameraVerticalPosition = Math.PI / 2.0;
                    break;
                case Key.Down:
                    cameraVerticalPosition -= cameraVerticalChange;
                    if (cameraVerticalPosition < -Math.PI / 2.0) cameraVerticalPosition = -Math.PI / 2.0;
                    break;
                case Key.Left:
                    cameraHorizontalPosition += cameraHorizontalChange;
                    break;
                case Key.Right:
                    cameraHorizontalPosition -= cameraHorizontalChange;
                    break;
                case Key.Add:
                    cameraDistance -= cameraScalingChange;
                    if (cameraDistance < cameraScalingChange) cameraDistance = cameraScalingChange;
                    break;
                case Key.OemPlus:
                    cameraDistance -= cameraScalingChange;
                    if (cameraDistance < cameraScalingChange) cameraDistance = cameraScalingChange;
                    break;
                case Key.Subtract:
                    cameraDistance += cameraScalingChange;
                    break;
                case Key.OemMinus:
                    cameraDistance += cameraScalingChange;
                    break;
            }
            positionCamera();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data != null)
            {
                Show3dSurface();
            }
        }

        private void initLights()
        {
            AmbientLight ambientLight = new AmbientLight(Colors.Gray);
            DirectionalLight directionalLight =
                new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));
            mainModel3Dgroup.Children.Add(ambientLight);
            mainModel3Dgroup.Children.Add(directionalLight);
        }

        private void сreateAltitudeMap(double[,] data)
        {
            int xWidth = data.GetUpperBound(0) + 1;
            int zWidth = data.GetUpperBound(1) + 1;
            double dx = (xmax - xmin) / xWidth;
            double dz = (zmax - zmin) / zWidth;
            IEnumerable<double> values = from double value in data select value;
            double ymin = values.Min();
            double ymax = values.Max();
            BitmapPixelCreator bitmapPixelCreator = new BitmapPixelCreator(xWidth, zWidth);
            for (int ix = 0; ix < xWidth; ix++)
            {
                for (int iz = 0; iz < zWidth; iz++)
                {
                    byte red;
                    byte green;
                    byte blue;
                    MapRainbowColor(data[ix, iz], ymin, ymax, out red, out green, out blue);
                    bitmapPixelCreator.SetPixel(ix, iz, red, green, blue, 255);
                }
            }
            surfaceImage = bitmapPixelCreator.MakeBitmap(96, 96);
            //SurfaceUtils.SaveWritableBitmapToFile(writableBitmap, TEMP_IMAGE_FILE);
        }

        private void MapRainbowColor(double value, double minValue, double maxValue, out byte red, out byte green, out byte blue)
        {
            int intValue = (int)(1023 * (value - minValue) / (maxValue - minValue));
            if (intValue < 256)
            {
                red = 255;
                green = (byte)intValue;
                blue = 0;
            }
            else if (intValue < 512)
            {
                intValue -= 256;
                red = (byte)(255 - intValue);
                green = 255;
                blue = 0;
            }
            else if (intValue < 768)
            {
                intValue -= 512;
                red = 0;
                green = 255;
                blue = (byte)intValue;
            }
            else
            {
                intValue -= 768;
                red = 0;
                green = (byte)(255 - intValue);
                blue = 255;
            }
        }
        
        private void defineModel(Model3DGroup modelGroup, double[,] values)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            float offsetX = xmax / 2f;
            float offsetZ = zmax / 2f;
            for (int x = xmin; x <= xmax - dx; x += dx)
            {
                for (int z = zmin; z <= zmax - dz; z += dx)
                {
                    Point3D p00 = new Point3D(x - offsetX, values[x, z], z - offsetZ);
                    Point3D p10 = new Point3D(x - offsetX + dx, values[x + dx, z], z - offsetZ);
                    Point3D p01 = new Point3D(x - offsetX, values[x, z + dz], z - offsetZ + dz);
                    Point3D p11 = new Point3D(x - offsetX + dx, values[x + dx, z + dz], z - offsetZ + dz);
                    addTriangle(mesh, p00, p01, p11);
                    addTriangle(mesh, p00, p11, p10);
                }
            }
            ImageBrush textureBrush = new ImageBrush();
            textureBrush.ImageSource = surfaceImage;
                //new BitmapImage(new Uri(TEMP_IMAGE_FILE, UriKind.Relative));
            DiffuseMaterial surfaceMaterial = new DiffuseMaterial(textureBrush);
            GeometryModel3D surfaceModel = new GeometryModel3D(mesh, surfaceMaterial);
            surfaceModel.BackMaterial = surfaceMaterial;
            modelGroup.Children.Add(surfaceModel);
        }

        private void addTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            int index1 = addPoint(mesh.Positions, mesh.TextureCoordinates, point1);
            int index2 = addPoint(mesh.Positions, mesh.TextureCoordinates, point2);
            int index3 = addPoint(mesh.Positions, mesh.TextureCoordinates, point3);
            mesh.TriangleIndices.Add(index1);
            mesh.TriangleIndices.Add(index2);
            mesh.TriangleIndices.Add(index3);
        }

        private int addPoint(Point3DCollection points, PointCollection textureCoords, Point3D point)
        {
            if (pointDictionary.ContainsKey(point))
            {
                return pointDictionary[point];
            }
            points.Add(point);
            pointDictionary.Add(point, points.Count - 1);
            textureCoords.Add(new Point((point.X - xmin) * textureScaleX, (point.Z - zmin) * textureScaleZ));
            return points.Count - 1;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.Key);
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            OnKeyDown(Key.Up);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            OnKeyDown(Key.Down);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            OnKeyDown(Key.Left);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            OnKeyDown(Key.Right);
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            OnKeyDown(Key.Add);
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            OnKeyDown(Key.Subtract);
        }

        private void positionCamera()
        {
            double y = cameraDistance * Math.Sin(cameraVerticalPosition);
            double hyp = cameraDistance * Math.Cos(cameraVerticalPosition);
            double x = hyp * Math.Cos(cameraHorizontalPosition);
            double z = hyp * Math.Sin(cameraHorizontalPosition);
            camera.Position = new Point3D(x, y, z);
            camera.LookDirection = new Vector3D(-x, -y, -z);
            camera.UpDirection = new Vector3D(0, 1, 0);
        }
    }
}
