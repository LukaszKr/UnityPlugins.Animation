using System;
using System.Collections.Generic;
using DG.Tweening;

namespace ProceduralLevel.UnityPlugins.Animation
{
	public class Animator
	{
		private AnimationManager m_Manager;

		private readonly Stack<AAnimationPlayback> m_PendingAnimations = new Stack<AAnimationPlayback>();
		private readonly List<AAnimationPlayback> m_ActiveAnimations = new List<AAnimationPlayback>();

		public Animator(AnimationManager manager)
		{
			m_Manager = manager;
		}

		public void Append<TParameters>(IAnimation<TParameters> animation, TParameters parameters, bool blocking = false)
			where TParameters : class
		{
			AAnimationPlayback playback = animation.GetPlayback(parameters, blocking);
			Append(playback);
		}

		public void Append(AAnimationPlayback playback)
		{
			m_PendingAnimations.Push(playback);
			m_Manager.RegisterAnimation(playback);
			TryPlayNextAnimation();
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

		private bool TryPlayNextAnimation()
		{
			if(m_PendingAnimations.Count > 0 && !IsPlayingBlockingAnimation())
			{
				AAnimationPlayback playback = m_PendingAnimations.Pop();
				m_ActiveAnimations.Add(playback);
				playback.Play(OnAnimationFinishedHandler);
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
