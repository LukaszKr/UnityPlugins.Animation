using DG.Tweening;

namespace ProceduralLevel.Animation.Unity
{
	public interface IAnimation<TParameters>
		where TParameters : class
	{
		AAnimationPlayback GetPlayback(TParameters parameters, bool blocking);
		Sequence Play(TParameters parameters);
	}
}
