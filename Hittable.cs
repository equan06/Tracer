using System;
using System.Numerics;

namespace Tracer
{
	public struct HitRecord
	{
		public Vector3 point;
		public Vector3 normal;
		public float t;
	}

	public abstract class Hittable
	{
		public abstract bool Hit(Ray r, float tMin, float tMax, HitRecord rec);
	}
}