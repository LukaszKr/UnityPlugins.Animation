namespace ProceduralLevel.UnityPlugins.Animation.Unity
{
	public class AnimationResourceLock
	{
		public readonly AAnimationPlayback Playback;
		public readonly bool Exclusive;

		public AnimationResourceLock(AAnimationPlayback playback, bool exclusive)
		{
			Playback = playback;
			Exclusive = exclusive;
		}
	}
}
