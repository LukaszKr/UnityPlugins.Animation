using System.Diagnostics;
using DG.Tweening;

namespace ProceduralLevel.Animation.Unity
{
	public class AnimationPlayback<TParameters> : AAnimationPlayback
		where TParameters : class
	{
		private readonly IAnimation<TParameters> m_Animation;
		private readonly TParameters m_Parameters;

		[DebuggerStepThrough]
		public AnimationPlayback(IAnimation<TParameters> animation, TParameters parameters, bool blocking = false)
			: base(blocking)
		{
			m_Animation = animation;
			m_Parameters = parameters;
		}

		protected override Sequence OnPlay()
		{
			return m_Animation.Play(m_Parameters);
		}
	}
}
