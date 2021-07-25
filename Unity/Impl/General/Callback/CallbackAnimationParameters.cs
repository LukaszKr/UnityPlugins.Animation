using DG.Tweening;

namespace ProceduralLevel.UnityPlugins.Animation.Unity
{
	internal class CallbackAnimationParameters
	{
		public readonly TweenCallback Callback;

		public CallbackAnimationParameters(TweenCallback callback)
		{
			Callback = callback;
		}
	}
}
