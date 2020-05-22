using UnityEngine;

namespace Roguelike2D
{
	public interface ISoundManager
	{
		void PlaySingle(AudioClip clip);
		void RandomizeSfx(params AudioClip[] clips);
		void StopMusic();
	}
}