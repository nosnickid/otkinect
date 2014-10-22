using System;
using OpenTK;

namespace orangestems.otkinect
{
    class FpsCamera
    {
        public Vector4 Position { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
        public Vector4 Up { get; set; }

        public FpsCamera()
        {
            this.Position = new Vector4(2, 2, 2, 0);
        }
    }
}
