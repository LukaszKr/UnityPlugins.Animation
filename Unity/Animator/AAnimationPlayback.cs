using System;
using DG.Tweening;
using ProceduralLevel.Common.Event;

namespace ProceduralLevel.UnityPlugins.Animation.Unity
{
	public abstract class AAnimationPlayback
	{
		private Sequence m_Sequence;
		private EPlaybackStatus m_Status;
		private Action<AAnimationPlayback> m_OnFinishedCallback;

		public readonly bool Blocking = false;

		public readonly CustomEvent<AAnimationPlayback> OnStarted = new CustomEvent<AAnimationPlayback>();
		public readonly CustomEvent<AAnimationPlayback> OnFinished = new CustomEvent<AAnimationPlayback>();

		public bool IsActive => m_Status > EPlaybackStatus.Active;
		public bool IsFinished => m_Status == EPlaybackStatus.Finished;

		protected AAnimationPlayback(bool blocking)
		{
			Blocking = blocking;
		}

		internal void Play(Action<AAnimationPlayback> onFinishedCallback)
		{
			if(m_Status != EPlaybackStatus.Pending)
			{
				throw new InvalidOperationException();
			}

			m_OnFinishedCallback = onFinishedCallback;

			SetStatus(EPlaybackStatus.Active);
			m_Sequence = OnPlay();
			m_Sequence.OnComplete(OnCompleteHandler);
		}

		internal void Abort()
		{
			if(m_Status != EPlaybackStatus.Active)
			{
				throw new InvalidOperationException();
			}
			m_Sequence.Kill(false);
			m_Sequence = null;
		}

		protected abstract Sequence OnPlay();

		private void SetStatus(EPlaybackStatus newStatus)
		{
			if(m_Status >= newStatus)
			{
				throw new InvalidOperationException();
			}
			m_Status = newStatus;
			switch(newStatus)
			{
				case EPlaybackStatus.Active:
					OnStarted.Invoke(this);
					break;
				case EPlaybackStatus.Finished:
					OnFinished.Invoke(this);
					Cleanup();
					break;
			}
		}

		private void Cleanup()
		{
			OnFinished.RemoveAllListeners();
			OnStarted.RemoveAllListeners();
			m_OnFinishedCallback = null;
			m_Sequence = null;
		}

		#region Callbacks
		private void OnCompleteHandler()
		{
			m_OnFinishedCallback(this);
			SetStatus(EPlaybackStatus.Finished);
		}
		#endregion
	}
}
