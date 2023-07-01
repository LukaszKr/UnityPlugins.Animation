using UnityEngine;

namespace ProceduralLevel.Animation.Unity
{
	public static class BezierCurveFactory
	{
		public static BezierCurve Create(Vector3 start, Vector3 end, bool rightSide, float curveModifier, float hillOffset = 0.5f)
		{
			Vector3 direction = (end-start);
			Vector3 perpendicular = new Vector3(-direction.y, direction.x, direction.z).normalized*start.magnitude*curveModifier;
			Vector3 midPoint = (start+end)*hillOffset;
			Vector3 offset = midPoint+(rightSide? -perpendicular: perpendicular);
			return new BezierCurve()
				.Add(start)
				.Add(offset)
				.Add(end);
		}
	}
}
