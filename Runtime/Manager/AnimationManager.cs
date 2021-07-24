using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProceduralLevel.UnityPlugins.Animation
{
	public class AnimationManager
	{
		private readonly Stack<AnimationGroup> m_ActiveGroups = new Stack<AnimationGroup>();
		private AnimationGroup m_CurrentGroup = null;

		internal void RegisterAnimation(AAnimationPlayback playback)
		{
			if(m_CurrentGroup != null)
			{
				m_CurrentGroup.RegisterAnimation(playback);
			}
		}

		public void StartGroup()
		{
			AnimationGroup group = new AnimationGroup();
			m_ActiveGroups.Push(group);
			m_CurrentGroup = group;
		}

		public void EndGroup()
		{
			PopGroup();
		}

		public async Task EndGroupAsync()
		{
			AnimationGroup group = PopGroup();
			await group.IsFinishedAsync();
		}

		private AnimationGroup PopGroup()
		{
			AnimationGroup group = m_ActiveGroups.Pop();
			if(m_ActiveGroups.Count > 0)
			{
				m_CurrentGroup = m_ActiveGroups.Peek();
			}
			else
			{
				m_CurrentGroup = null;
			}
			return group;
		}
	}
}
