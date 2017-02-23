using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Squares
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    class Rectangle
    {
        private Color color;
        private double[] coordinates;

        public Rectangle(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, float r, float g, float b)
        {
            color = new Color();
            color.ScR = r;
            color.ScG = g;
            color.ScB = b;

            coordinates = new double[8];
            coordinates[0] = x1;
            coordinates[1] = y1;
            coordinates[2] = x2;
            coordinates[3] = y2;
            coordinates[4] = x3;
            coordinates[5] = y3;
            coordinates[6] = x4;
            coordinates[7] = y4;
        }

        public double[] Coordinates => coordinates;

        public Color Color => color;
    }
    public partial class MainWindow
    {
        List<Rectangle> rectangles = new List<Rectangle>();
        private SharpGL.OpenGL gl;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            gl = args.OpenGL;
            gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            foreach (var rect in rectangles)
            {
                DrawRectangle(gl, rect);
            }
            gl.Flush();
        }

       private void DrawRectangle(SharpGL.OpenGL gl, Rectangle rect)
        {
            gl.Begin(SharpGL.OpenGL.GL_POLYGON);
            gl.Color(rect.Color.ScR, rect.Color.ScG, rect.Color.ScB);
            double helpminY = rect.Coordinates[1];
            double helpmaxX = rect.Coordinates[0];
            double helpminX = rect.Coordinates[0];

            for (int i = 0; i < 7; i += 2)
            {
                gl.Vertex(rect.Coordinates[i], rect.Coordinates[i + 1]);
                if (rect.Coordinates[i + 1] < helpminY)
                {
                    helpminY = rect.Coordinates[i + 1];
                }
                if (rect.Coordinates[i] < helpminX)
                {
                    helpminX = rect.Coordinates[i];
                }
                if (rect.Coordinates[i] > helpmaxX)
                {
                    helpmaxX = rect.Coordinates[i];
                }
            }

            gl.End();
            double textPosY;
            if (helpminY < 0)
            {
                textPosY = (1 + helpminY) * 100 * Height / 2 / 100;
            }
            else
            {
                textPosY = helpminY * 100 * Height / 2 / 100 + Height / 2.0;
            }
            double textPosX;
            if (helpminX < 0)
            {
                textPosX = (1 + helpminX) * 100 * Width / 2 / 100;
            }
            else
            {
                textPosX = helpminX * 100 * Width / 2 / 100 + Width / 2.0;
            }
            gl.DrawText((int)(textPosX), (int)(textPosY), 1, 1, 1, "courier new", 14, rect.Color.ToString());
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            args.OpenGL.Ortho(0, Width, 0, Height, -1.0, 1.0);
        }

        private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            var counter = rectangles.Count;
            if (counter != 0)
            {
                rectangles.Remove(rectangles.Last());
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            var r = Convert.ToSingle(rand.NextDouble());
            var g = Convert.ToSingle(rand.NextDouble());
            var b = Convert.ToSingle(rand.NextDouble());
            var point = rand.NextDouble() * (rand.NextDouble() > 0.5 ? 1 : -1);

            var rect = new Rectangle(point, point, point, -point, -point, -point, -point, point, r, g, b);

            rectangles.Add(rect);

        }
    }
}
