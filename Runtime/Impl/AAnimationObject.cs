using DG.Tweening;
using UnityEngine;

namespace ProceduralLevel.UnityPlugins.Animation
{
	public abstract class AAnimationObject<TParameters>: ScriptableObject, IAnimation<TParameters>
		where TParameters : class
	{
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
