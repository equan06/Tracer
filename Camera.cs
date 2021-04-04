using System;
using System.Numerics;

namespace Tracer
{
    class Camera
    {
        private Vector3 horizontal;
        private Vector3 vertical;
        private Vector3 origin;
        private Vector3 lowerLeftCorner;

        public Camera()
        {
            const float AspectRatio = 16f / 9;

            // Dimensions of the viewport
            // This is basically the plane that we send our rays to
            float viewportHeight = 2.0f;
            float viewportWidth = viewportHeight * AspectRatio;
            float focalLength = 1.0f;

            origin = new Vector3(0, 0, 0);
            horizontal = new Vector3(viewportWidth, 0, 0); // x 
            vertical = new Vector3(0, viewportHeight, 0); // y 
            // Create the corner of the viewport
            lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, focalLength);
        }

        public Ray GetRay(float u, float v)
        {
            return new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
        }
    }
}
