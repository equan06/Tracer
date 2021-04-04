using System;
using System.Numerics;
using System.Collections.Generic;

namespace Tracer
{
	public struct HitRecord
	{
		public Vector3 point;
		public Vector3 normal;
		public float t;
		public bool isHit;
	}

	public abstract class Hittable
	{
		public abstract HitRecord Hit(Ray r, float tMin, float tMax);
	}

	public class HittableList
    {
		public List<Hittable> objects;

		public HittableList()
        {
			objects = new List<Hittable>();
        }

		public void Clear()
        {
			objects.Clear();
        }

		public void Add(Hittable obj)
        {
			objects.Add(obj);
        }

		public HitRecord Hit(Ray r, float tMin, float tMax)
        {
			HitRecord resultRecord = new HitRecord() ;
			float closest = tMax;
			foreach (Hittable obj in objects)
            {
				HitRecord rec = obj.Hit(r, tMin, closest);
				if (rec.isHit)
                {
					closest = rec.t;
					resultRecord = rec;
                }
            }
			return resultRecord;
        }
    }
}