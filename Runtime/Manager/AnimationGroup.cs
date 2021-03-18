using System.Threading.Tasks;

namespace ProceduralLevel.UnityPlugins.Animation
{
	public class AnimationGroup
	{
		private int m_AnimationCount;

		public AnimationGroup()
		{
		}

		internal void RegisterAnimation(AAnimationPlayback playback)
		{
			++m_AnimationCount;
			playback.OnFinished.AddListener(OnFinishedHandler);
		}

		internal Task IsFinishedAsync()
		{
			return Task.Run(AwaitFinished);
		}

		private async Task AwaitFinished()
		{
			while(m_AnimationCount > 0)
			{
				await Task.Delay(1);
			}
		}

		private void OnFinishedHandler(AAnimationPlayback playback)
		{
			--m_AnimationCount;
		}
	}
}
