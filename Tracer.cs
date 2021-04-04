using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tracer
{

    class Tracer
    {
        public const float infinity = int.MaxValue;
        public const float pi = (float)Math.PI;

        public float DegreesToRadians(float deg)
        {
            return deg * pi / 180;
        }

        static void Main(string[] args)
        {
            const float AspectRatio = 16f / 9;
            const int ImageWidth = 400;
            const int ImageHeight = (int)(ImageWidth / AspectRatio);

            // Dimensions of the viewport
            // This is basically the plane that we send our rays to
            float viewportHeight = 2.0f;
            float viewportWidth = viewportHeight * AspectRatio;
            float focalLength = 1.0f;

            Vector3 origin = new Vector3(0, 0, 0);
            Vector3 horizontal = new Vector3(viewportWidth, 0, 0); // x 
            Vector3 vertical = new Vector3(0, viewportHeight, 0); // y 
            // Create the corner of the viewport
            Vector3 lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, focalLength);

            HittableList world = new HittableList();
            world.Add(new Sphere(new Vector3(0, 0, -1), 0.5f));
            world.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f));

            Console.WriteLine($"P3\n{ImageWidth} {ImageHeight}\n255");

            for (int j = ImageHeight - 1; j >= 0; j--)
            {
                Console.Error.WriteLine("\rScanlines remaining: " + j);
                for (int i = 0; i < ImageWidth; i++)
                {
                    float u = (float)i / (ImageWidth - 1);
                    float v = (float)j / (ImageHeight - 1);
                    // Draw a ray from the origin to the viewport
                    Ray r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
                    // Compute the color at the intersection between the ray and the viewport
                    WriteLineColor(RayColor(r, world));
                }
            }

            Console.Error.WriteLine("Done");
        }

        /// <summary>
        /// Assume that v has values in [0, 1]. Writes an RGB value scaled by v. 
        /// </summary>
        /// <param name="v"></param>
        public static void WriteLineColor(Vector3 v)
        {
            Console.WriteLine($"{(int)(255.999 * v.X)} {(int)(255.999 * v.Y)} {(int)(255.999 * v.Z)}");
        }

        public static Vector3 RayColor(Ray r, HittableList world)
        {
            HitRecord rec = world.Hit(r, 0, infinity);
            if (rec.isHit)
            {
                // Calculate the surface normal of the closest object, and scale to [0,1]
                return 0.5f * (rec.normal + new Vector3(1, 1, 1));
            }

            Vector3 unitDirection = Vector3.Normalize(r.Direction);
            // Scale y from [-1, 1] to [0, 1]
            float t = 0.5f * (1 + unitDirection.Y);
            // Linear interp. from white (255, 255, 255) to blue (128, 180, 255) based on y
            return (1 - t) * new Vector3(1, 1, 1) + t * new Vector3(0.5f, 0.7f, 1.0f);
        }


        public static float HitSphere(Vector3 center, float radius, Ray r)
        {
            // Some simplifications applied (instead of using b, use b/2)
            Vector3 rayToCenter = r.Origin - center;
            float a = Vector3.Dot(r.Direction, r.Direction);
            float halfb = Vector3.Dot(r.Direction, rayToCenter);
            float c = Vector3.Dot(rayToCenter, rayToCenter) - radius * radius;
            float discriminant = (halfb * halfb - 4 * a * c);
            if (discriminant < 0)
                return -1;
            else
            {
                return (-halfb - (float)Math.Sqrt(discriminant)) / a;
            }
        }
    }
}
