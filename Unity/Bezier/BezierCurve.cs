using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ProceduralLevel.UnityPlugins.Animation.Unity
{
	[Serializable]
	public class BezierCurve
	{
		private readonly List<Vector3> m_Buffer = new List<Vector3>();

		public List<Vector3> ControlPoints = new List<Vector3>();

		public BezierCurve Add(Vector3 controlPoint)
		{
			ControlPoints.Add(controlPoint);
			return this;
		}

		public Tween Tween(Action<Vector3> setter, float duration)
		{
			float t = 0f;
			return DOTween.To(() => t,
			(newT) =>
			{
				t = newT;
				Vector3 position = Evaluate(t);
				setter(position);
			}, 1f, duration);
		}

		public Vector3 Evaluate(float t)
		{
			if(ControlPoints.Count == 0)
			{
				throw new NotSupportedException();
			}
			m_Buffer.Clear();
			m_Buffer.AddRange(ControlPoints);
			return InterpolateLayer(t);
		}

		private Vector3 InterpolateLayer(float t)
		{
			int count = m_Buffer.Count;
			if(count == 1)
			{
				return m_Buffer[0];
			}
			int lastElement = count-1;
			for(int x = 0; x < lastElement; ++x)
			{
				Vector3 a = m_Buffer[x];
				Vector3 b = m_Buffer[x+1];
				m_Buffer[x] = Interpolate(a, b, t);
			}
			m_Buffer.RemoveAt(lastElement);
			return InterpolateLayer(t);
		}

		private Vector3 Interpolate(Vector3 a, Vector3 b, float t)
		{
			Vector3 vectorDelta = b-a;
			return a+vectorDelta*t;
		}
	}
}
