using DG.Tweening;

namespace ProceduralLevel.Animation.Unity
{
	internal class DelayAnimation : AAnimation<DelayAnimationParameters>
	{
		public static readonly DelayAnimation Instance = new DelayAnimation();

		protected override void OnPlay(DelayAnimationParameters parameters, Sequence sequence)
		{
			sequence.AppendInterval(parameters.Delay);
		}
	}
}
