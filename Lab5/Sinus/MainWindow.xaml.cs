using SharpGL;
using System;
using System.Windows;
using SharpGL.SceneGraph.Assets;

namespace TriangleLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Texture globeTexture = new Texture();
        float rotateGlobe = 0f;
        private float speed = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = args.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(1, 1, 1, 1);

            gl.LoadIdentity();

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_REPLACE);
            gl.Disable(OpenGL.GL_LIGHTING);

            var globe = gl.NewQuadric();

            gl.Translate(0f, 0f, -3.5f);
            gl.Rotate(-90.0f, 1.0f, 0.0f, 0.3f);

            if ((bool)checkBoxMoving.IsChecked)
                gl.Rotate(rotateGlobe, 0, 0, 1);

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new float[] { 0f, 0f, 0f, 1f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new float[] { 1.0f, 1.0f, 0.8f });
            globeTexture.Bind(gl);

            gl.QuadricTexture(globe, 1);
            gl.QuadricDrawStyle(globe, OpenGL.GLU_FILL);
            gl.QuadricNormals(globe, OpenGL.GLU_SMOOTH);
            gl.QuadricOrientation(globe, 100020);
            gl.Sphere(globe, 1, 200, 20);

            gl.LoadIdentity();

            gl.Flush();
            try
            {
                speed = float.Parse(textBoxSpeed.Text);
            }
            catch (Exception)
            {
                speed = 1;
            }
            rotateGlobe += speed;
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            globeTexture.Create(gl, "ukraine.gif");
            gl.Ortho(0, Width, 0, Height, -1, -1);
            gl.ShadeModel(OpenGL.GL_SMOOTH);

            gl.Enable(OpenGL.GL_LIGHT0);

            gl.Enable(OpenGL.GL_DEPTH_TEST);
        }
    }
}
