using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Emgu.CV;

namespace orangestems.otkinect
{
    class OpenTKExample 
    {
        [STAThread]
        public static void Main()
        {
            float viewOffs = 1.4f;
	    	Capture camera = null;
			int texture = 0;
			Bitmap imgFrame = null;

            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;

					Matrix4 project = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI * 0.5), 800.0f / 600.0f, 1f, 9999f);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref project);

					var colors = new int[4] { 0xff, 0xff00, 0xff0000, 0xff00fff};
					texture = GL.GenTexture();
					GL.BindTexture(TextureTarget.Texture2D, texture);
					GL.TexImage2D<int>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 2, 2, 0, PixelFormat.Rgb, 
						PixelType.UnsignedByte, colors);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

					camera = new Capture(Emgu.CV.CvEnum.CaptureType.ANY);
					camera.Start();

					camera.ImageGrabbed += (object imgSender, EventArgs imgE) => 
					{
						var frame = camera.RetrieveBgrFrame();
						imgFrame = frame.Bitmap;
					};
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
						if (camera != null) camera.Stop();
                        game.Exit();
                    }
                    //viewOffs += 0.01f;

					//if (camera.*
                };
 
                game.RenderFrame += (sender, e) =>
                {
					if (imgFrame != null) {
						var d = imgFrame.LockBits(
							new Rectangle(0, 0, imgFrame.Width, imgFrame.Height), 
							System.Drawing.Imaging.ImageLockMode.ReadOnly,
							System.Drawing.Imaging.PixelFormat.Format24bppRgb
						);
						GL.BindTexture(TextureTarget.Texture2D, texture);
						GL.TexImage2D(TextureTarget.Texture2D, 0, 
							PixelInternalFormat.Rgb, 
							imgFrame.Width, imgFrame.Height, 0, 
							PixelFormat.Bgr, PixelType.UnsignedByte, d.Scan0);

					imgFrame.UnlockBits(d);

						imgFrame = null;
					}

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
						
					GL.Enable(EnableCap.Texture2D);
					GL.BindTexture(TextureTarget.Texture2D, texture);
					GL.Begin(PrimitiveType.Quads);
					GL.Color4(1f,1f,1f,1f);
					GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 2);
					GL.TexCoord2(1f, 0); GL.Vertex3(0, 1, 2);
					GL.TexCoord2(1f, 1); GL.Vertex3(1, 1, 2);
					GL.TexCoord2(0, 1f); GL.Vertex3(1, 0, 2);
					GL.End();

					GL.Disable(EnableCap.Texture2D);
 
                    game.SwapBuffers();
                };
 
                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }
    }
}
