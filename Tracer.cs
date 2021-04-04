using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tracer
{
    class Tracer
    {
        public const float infinity = int.MaxValue;
        public const float pi = (float)Math.PI;
        public Random random = new Random();

        public float DegreesToRadians(float deg)
        {
            return deg * pi / 180;
        }

        public float RandUnit()
        {
            return (float)random.NextDouble();
        }

        public float RandFloat(float a, float b)
        {
            return a + (b - a) * (float)random.NextDouble();
        }

        public Vector3 RandUnitVec()
        {
            return new Vector3(RandUnit(), RandUnit(), RandUnit());
        }

        public Vector3 RandFloatVec(float a, float b)
        {
            return new Vector3(RandFloat(a, b), RandFloat(a, b), RandFloat(a, b));
        }

        public Vector3 RandVecInUnitSphere()
        {
            // Note: while you could do this, the distribution will be different than the below sampling scheme.
            // This samples within a unit cube, and then normalizes (to be on the sphere)
            //return Vector3.Normalize(RandFloatVec(-1, 1));

            // This samples within a unit sphere and then normalizes
            while (true)
            {
                Vector3 p = RandFloatVec(-1, 1);
                if (p.LengthSquared() >= 1) continue;
                return Vector3.Normalize(p);
            }
        }

        public float Clamp(float x, float min, float max)
        {
            return x < min ? min : (x > max ? max : x);
        }

        public Vector3 ClampVector(Vector3 v, float min, float max)
        {
            return new Vector3(Clamp(v.X, min, max), Clamp(v.Y, min, max), Clamp(v.Z, min, max));
        }

        /// <summary>
        /// Assume that v has values in [0, 1]. Writes an RGB value scaled by v. 
        /// </summary>
        /// <param name="v"></param>
        public void WriteLineColor(Vector3 v)
        {
            Console.WriteLine($"{(int)(255.999 * v.X)} {(int)(255.999 * v.Y)} {(int)(255.999 * v.Z)}");
        }

        public Vector3 RayColor(Ray r, HittableList world, int depth)
        {
            if (depth <= 0) return new Vector3(0, 0, 0);
            // Note: using .001 instead of 0 removes rays that are close to t=0 as a result of floating point approximations
            HitRecord rec = world.Hit(r, 0.001f, infinity);
            if (rec.isHit)
            {
                Vector3 target = rec.normal + RandVecInUnitSphere();
                // Calculate the surface normal of the closest object, and send it in a random direction starting from the intersection point 
                return 0.5f * RayColor(new Ray(rec.point, target), world, depth - 1);
            }

            Vector3 unitDirection = Vector3.Normalize(r.Direction);
            // Scale y from [-1, 1] to [0, 1]
            float t = 0.5f * (1 + unitDirection.Y);
            // Linear interp. from white (255, 255, 255) to blue (128, 180, 255) based on y
            return (1 - t) * new Vector3(1, 1, 1) + t * new Vector3(0.5f, 0.7f, 1.0f);
        }

        public void CreateImage()
        {
            const float AspectRatio = 16f / 9;
            const int ImageWidth = 400;
            const int ImageHeight = (int)(ImageWidth / AspectRatio);
            const int samplesPerPixel = 100;
            const int maxDepth = 50;

            Camera cam = new Camera();
            HittableList world = new HittableList();
            world.Add(new Sphere(new Vector3(0, 0, -1), 0.5f));
            world.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f));

            Console.WriteLine($"P3\n{ImageWidth} {ImageHeight}\n255");
            for (int j = ImageHeight - 1; j >= 0; j--)
            {
                Console.Error.WriteLine("\rScanlines remaining: " + j);
                for (int i = 0; i < ImageWidth; i++)
                {
                    // Sample the pixel color by averaging over nearby sampled rays
                    Vector3 pixelColor = new Vector3(0, 0, 0);
                    for (int s = 0; s < samplesPerPixel; s++)
                    {
                        float u = (float)(i + RandUnit()) / (ImageWidth - 1);
                        float v = (float)(j + RandUnit()) / (ImageHeight - 1);
                        pixelColor += RayColor(cam.GetRay(u, v), world, maxDepth);
                    }
                    WriteLineColor(ClampVector(Vector3.SquareRoot(pixelColor / samplesPerPixel), 0, 0.999f));
                }
            }
            Console.Error.WriteLine("Done");
        }

        static void Main(string[] args)
        {
            Tracer t = new Tracer();
            t.CreateImage();
        }
    }
}
