using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

namespace ProceduralLevel.Animation.Unity
{
	public abstract class AAnimationObject<TParameters> : ScriptableObject, IAnimation<TParameters>
		where TParameters : class
	{
		[DebuggerStepThrough]
		public AAnimationPlayback GetPlayback(TParameters parameters, bool blocking)
		{
			return new AnimationPlayback<TParameters>(this, parameters, blocking);
		}

		public Sequence Play(TParameters parameters)
		{
			Sequence sequence = DOTween.Sequence();
			OnPlay(parameters, sequence);
			return sequence;
		}

		protected abstract void OnPlay(TParameters parameters, Sequence sequence);
	}
}
