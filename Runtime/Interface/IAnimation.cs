using DG.Tweening;

namespace ProceduralLevel.UnityPlugins.Animation
{
	public interface IAnimation<TParameters>
		where TParameters : class
	{
		AAnimationPlayback GetPlayback(TParameters parameters, bool blocking);
		Sequence Play(TParameters parameters);
	}
}
