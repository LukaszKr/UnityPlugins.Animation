using System.Collections.Generic;

namespace ProceduralLevel.UnityPlugins.Animation.Unity
{
	public class AnimationResource
	{
		private Queue<AnimationResourceLock> m_Pending = new Queue<AnimationResourceLock>();
		private List<AnimationResourceLock> m_Active = new List<AnimationResourceLock>();

		public void AddRequest(AAnimationPlayback playback, bool exclusive)
		{
			AnimationResourceLock resourceLock = new AnimationResourceLock(playback, exclusive);
			if(IsExclusiveAnimationActive())
			{
				m_Pending.Enqueue(resourceLock);
				resourceLock.Playback.SetLocked(true);
			}
			else
			{
				PushToActive(resourceLock);
			}
		}

		private void TryPlayNext()
		{
			if(IsExclusiveAnimationActive())
			{
				return;
			}

			while(m_Pending.Count > 0)
			{
				AnimationResourceLock pendingLock = m_Pending.Dequeue();
				pendingLock.Playback.SetLocked(false);
				PushToActive(pendingLock);
				if(pendingLock.Exclusive)
				{
					break;
				}
			}
		}

		private void PushToActive(AnimationResourceLock resourceLock)
		{
			m_Active.Add(resourceLock);
			resourceLock.Playback.OnFinished.AddListener(OnActiveAnimationFinishedHandler);
		}

		private bool IsExclusiveAnimationActive()
		{
			int count = m_Active.Count;
			for(int x = 0; x < count; ++x)
			{
				AnimationResourceLock targetLock = m_Active[x];
				if(targetLock.Exclusive)
				{
					return true;
				}
			}
			return false;
		}

		private void OnActiveAnimationFinishedHandler(AAnimationPlayback playback)
		{
			int count = m_Active.Count;
			for(int x = 0; x < count; ++x)
			{
				AnimationResourceLock resourceLock = m_Active[x];
				if(resourceLock.Playback == playback)
				{
					m_Active.RemoveAt(x);
					break;
				}
			}
			TryPlayNext();
		}
	}
}
