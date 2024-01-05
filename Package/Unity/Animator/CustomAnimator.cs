using System;
using System.Collections.Generic;
using DG.Tweening;

namespace ProceduralLevel.Animation.Unity
{
	public class CustomAnimator
	{
		private readonly Queue<AAnimationPlayback> m_PendingAnimations = new Queue<AAnimationPlayback>();
		private readonly List<AAnimationPlayback> m_ActiveAnimations = new List<AAnimationPlayback>();

		public AAnimationPlayback Append<TParameters>(IAnimation<TParameters> animation, TParameters parameters, bool blocking = false)
			where TParameters : class
		{
			AAnimationPlayback playback = animation.GetPlayback(parameters, blocking);
			m_PendingAnimations.Enqueue(playback);
			return playback;
		}

		public void Abort()
		{
			m_PendingAnimations.Clear();
			int count = m_ActiveAnimations.Count;
			for(int x = 0; x < count; ++x)
			{
				AAnimationPlayback playback = m_ActiveAnimations[x];
				playback.Abort();
			}
			m_ActiveAnimations.Clear();
		}

		public bool TryPlayNextAnimation()
		{
			if(m_PendingAnimations.Count > 0 && !IsPlayingBlockingAnimation())
			{
				AAnimationPlayback playback = m_PendingAnimations.Peek();
				if(playback.IsLocked)
				{
					playback.OnUnlocked.AddListener(OnPlaybackUnlockedHandler);
					return false;
				}
				m_PendingAnimations.Dequeue();
				m_ActiveAnimations.Add(playback);
				playback.OnFinished.AddListener(OnAnimationFinishedHandler);
				playback.Play();
				if(playback.IsFinished)
				{
					TryPlayNextAnimation();
				}
			}
			return false;
		}

		private bool IsPlayingBlockingAnimation()
		{
			int count = m_ActiveAnimations.Count;
			for(int x = count-1; x >= 0; --x)
			{
				AAnimationPlayback playback = m_ActiveAnimations[x];
				if(playback.Blocking)
				{
					return true;
				}
			}
			return false;
		}

		#region Animations
		public void Delay(float delay, bool blocking = false)
		{
			DelayAnimationParameters parameters = new DelayAnimationParameters(delay);
			Append(DelayAnimation.Instance, parameters, blocking);
		}

		public void Callback(TweenCallback callback, bool blocking = false)
		{
			CallbackAnimationParameters parameters = new CallbackAnimationParameters(callback);
			Append(CallbackAnimation.Instance, parameters, blocking);
		}
		#endregion

		#region Callbacks
		private void OnPlaybackUnlockedHandler()
		{
			TryPlayNextAnimation();
		}

		private void OnAnimationFinishedHandler(AAnimationPlayback playback)
		{
			if(!m_ActiveAnimations.Remove(playback))
			{
				throw new InvalidOperationException();
			}
			TryPlayNextAnimation();
		}
		#endregion
	}
}
