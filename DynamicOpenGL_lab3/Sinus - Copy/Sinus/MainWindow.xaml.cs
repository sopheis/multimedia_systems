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
        private double a = 1;
        private double b = 0;
        private double rotateAngle = 0;
        private double move_x = 0.0;
        private double x = 0.0;
        private double y = 0.0;
        private double z = 0.0;
        private string sign = "+";
        private bool toRotate = false;
        private bool toMove = false;

        public MainWindow()
        {
            InitializeComponent();
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
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(1, 1, 1, 1);

            gl.LoadIdentity();

            gl.MatrixMode((SharpGL.Enumerations.MatrixMode) MatrixMode.Modelview);
            gl.LoadIdentity();

            gl.PushMatrix();
            try
            {
                a = Convert.ToDouble(textBox.Text);
            }
            catch (Exception)
            {
                a = 1.0;
            }
                b = 0;

            if (rectangles.Count != 0)
            {
                x = -(rectangles[0].Coordinates[0] + rectangles[0].Coordinates[2] + rectangles[0].Coordinates[4]) / 3.0;
                y = -(rectangles[0].Coordinates[1] + rectangles[0].Coordinates[3] + rectangles[0].Coordinates[5]) / 3.0;
            }
            if (toMove)
            {
                double move_y = a*Math.Pow(move_x, 2);
                gl.Translate(move_x, move_y, z);
            }

            if (toRotate)
            {
                gl.Translate(-x, -y, -z);
                //gl.Rotate(rotateAngle, 0, 0, 1);
                gl.Scale(2, 2, 2);
                gl.Translate(x, y, z);
            }

            foreach (var tr in rectangles)
            {
                DrawRectangle(gl, tr);
            }

            gl.PopMatrix();

            gl.Flush();

            if (sign == "+")
            {
                move_x += 0.1f;
            }
            if (sign == "-")
            {
                move_x -= 0.1f;
            }

            if (move_x > 1.0)
            {
                sign = "-";
            }
            if (move_x < -1.0)
            {
                sign = "+";
            }

            rotateAngle += 3;
        }

    private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            gl = args.OpenGL;
            gl.MatrixMode((SharpGL.Enumerations.MatrixMode) MatrixMode.Projection);
            gl.LoadIdentity();
            gl.Ortho(0, Width, 0, Height, -1, 1);
        }   

    private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGLControl_OpenGLDraw(sender, args);
        }

    private void button_Click(object sender, RoutedEventArgs e)
        {
            if (rectangles.Count == 1)
            {
                return;
            }
            var rand = new Random();
            var r = Convert.ToSingle(rand.NextDouble());
            var g = Convert.ToSingle(rand.NextDouble());
            var b = Convert.ToSingle(rand.NextDouble());
            var point = 0.1; //rand.NextDouble() * (rand.NextDouble() > 0.5 ? 1 : -1);

            var rect = new Rectangle(point, point, point, -point, -point, -point, -point, point, r, g, b);

            rectangles.Add(rect);
        }

    private void button1_Click(object sender, RoutedEventArgs e)
        {
            int n = rectangles.Count;
            if (n != 0)
            {
                rectangles.Remove(rectangles.Last());
            }
        }

        private void checkBoxMoving_Checked(object sender, RoutedEventArgs e)
        {
            toMove = !toMove;
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            if (button2.Content.ToString() == "Збільшити")
                button2.Content = "Зменшити";
            else if (button2.Content.ToString() == "Зменшити")
                button2.Content = "Збільшити";
            toRotate = !toRotate;
        }
    } 
    }
