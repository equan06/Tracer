using System;
using System.Numerics;

namespace Tracer
{
    class Sphere : Hittable
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        
        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public override bool Hit(Ray r, float tMin, float tMax, HitRecord record) 
        {
            // Some simplifications applied (instead of using b, use b/2)
            Vector3 rayToCenter = r.Origin - Center;
            float a = Vector3.Dot(r.Direction, r.Direction);
            float halfb = Vector3.Dot(r.Direction, rayToCenter);
            float c = Vector3.Dot(rayToCenter, rayToCenter) - Radius * Radius;
            float discriminant = (halfb * halfb - 4 * a * c);

            if (discriminant < 0)
                return false;
            else
            {
                float sqrtd = (float)Math.Sqrt(discriminant);
                float root = (-halfb - sqrtd) / a;
                if (root < tMin || root > tMax)
                {
                    root = (-halfb + sqrtd) / a;
                    if (root < tMin || root > tMax)
                        return false;
                }

                record.t = root;
                record.point = r.At(record.t);
                record.normal = (record.point - Center) / Radius;
                return true;
            }
        }
    }
}
