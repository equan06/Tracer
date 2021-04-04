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

        public override HitRecord Hit(Ray r, float tMin, float tMax) 
        {
            HitRecord rec = new HitRecord();
            // Some simplifications applied (instead of using b, use b/2)
            Vector3 rayToCenter = r.Origin - Center;
            float a = Vector3.Dot(r.Direction, r.Direction);
            float halfb = Vector3.Dot(r.Direction, rayToCenter);
            float c = Vector3.Dot(rayToCenter, rayToCenter) - Radius * Radius;
            float discriminant = (halfb * halfb - a * c);

            if (discriminant < 0)
            {
                rec.isHit = false;
                return rec;
            }
            else
            {
                float sqrtd = (float)Math.Sqrt(discriminant);
                float root = (-halfb - sqrtd) / a;
                if (root < tMin || root > tMax)
                {
                    root = (-halfb + sqrtd) / a;
                    if (root < tMin || root > tMax)
                    {
                        rec.isHit = false;
                        return rec;
                    }
                }
                rec.isHit = true;
                rec.t = root;
                rec.point = r.At(rec.t);
                rec.normal = (rec.point - Center) / Radius;
                return rec;
            }
        }
    }
}
