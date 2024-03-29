﻿using DG.Tweening;

namespace ProceduralLevel.Animation.Unity
{
	internal class CallbackAnimation : AAnimation<CallbackAnimationParameters>
	{
		public static readonly CallbackAnimation Instance = new CallbackAnimation();

		protected override void OnPlay(CallbackAnimationParameters parameters, Sequence sequence)
		{
			sequence.AppendCallback(parameters.Callback);
		}
	}
}
