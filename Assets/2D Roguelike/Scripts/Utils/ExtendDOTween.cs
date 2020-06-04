using DG.Tweening;
using System;
namespace Roguelike2D.Utils
{
	public static class ExtendDOTween
	{
		// sequence
		public static void Wait(this Sequence seq, float waitTime, Action OnEnd) {
			seq.AppendInterval(waitTime);
			seq.OnComplete(() => { OnEnd.Invoke(); });
		}
	}
}
