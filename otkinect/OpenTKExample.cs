using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace orangestems.otkinect
{
    class OpenTKExample 
    {
        [STAThread]
        public static void Main()
        {
            float viewOffs = 0;

            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;

                    Matrix4 project = Matrix4.Perspective(90f, 800.0f / 600.0f, 1f, 9999f);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref project);
                };
 
                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };
 
                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                    viewOffs += 0.01f;
                };
 
                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    Vector3 pos = new Vector3(2 * (float)Math.Sin(viewOffs), 2f, 2 * (float)Math.Cos(viewOffs));
                    Matrix4 lookat = Matrix4.LookAt(pos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref lookat);
 
                    GL.Begin(PrimitiveType.Triangles);
 
                    GL.Color4(Color4.MidnightBlue);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.Color4(Color4.SpringGreen);
                    GL.Vertex2(0.0f, -1.0f);
                    GL.Color4(Color4.Ivory);
                    GL.Vertex2(1.0f, 1.0f);
 
                    GL.End();

                    GL.Begin(PrimitiveType.Lines);

                    GL.Color4(1.0, 0, 0, 1);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(1, 0, 0);
                    GL.Color4(0, 1.0, 0, 1);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0, 1, 0);
                    GL.Color4(0, 0, 1.0, 1);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0, 0, 1);

                    GL.End();
 
                    game.SwapBuffers();
                };
 
                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }
    }
}
