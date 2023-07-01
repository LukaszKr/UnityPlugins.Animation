using System;
using DG.Tweening;
using ProceduralLevel.Common.Event;

namespace ProceduralLevel.Animation.Unity
{
	public abstract class AAnimationPlayback
	{
		private Sequence m_Sequence;
		private EPlaybackStatus m_Status;
		private int m_Locked;

		public readonly bool Blocking = false;

		public readonly CustomEvent<AAnimationPlayback> OnFinished = new CustomEvent<AAnimationPlayback>();
		public readonly CustomEvent OnUnlocked = new CustomEvent();

		public bool IsLocked => m_Locked > 0;
		public bool IsActive => m_Status > EPlaybackStatus.Active;
		public bool IsFinished => m_Status == EPlaybackStatus.Finished;


		protected AAnimationPlayback(bool blocking)
		{
			Blocking = blocking;
		}

		internal void SetLocked(bool locked)
		{
			if(locked)
			{
				m_Locked++;
			}
			else
			{
				m_Locked--;
				if(m_Locked == 0)
				{
					OnUnlocked.Invoke();
				}
			}
		}

		internal void Play()
		{
			if(m_Status != EPlaybackStatus.Pending)
			{
				throw new InvalidOperationException();
			}

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
			if(newStatus == EPlaybackStatus.Finished)
			{
				OnFinished.Invoke(this);
				Cleanup();
			}
		}

		private void Cleanup()
		{
			OnUnlocked.RemoveAllListeners();
			OnFinished.RemoveAllListeners();
			m_Sequence = null;
		}

		#region Callbacks
		private void OnCompleteHandler()
		{
			SetStatus(EPlaybackStatus.Finished);
		}
		#endregion
	}
}
