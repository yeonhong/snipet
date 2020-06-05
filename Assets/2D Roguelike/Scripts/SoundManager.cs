using UnityEngine;

namespace Roguelike2D
{
	public class SoundManager : MonoBehaviour, ISoundManager
	{
		[SerializeField] private AudioSource efxSource = null;
		[SerializeField] private AudioSource musicSource = null;
		[SerializeField] private float lowPitchRange = .95f;
		[SerializeField] private float highPitchRange = 1.05f;

		public static SoundManager Instance { get; private set; }

		private void Awake() {
			if (Instance == null) {
				Instance = this;
			}
			else if (Instance != this) {
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}

		public void PlaySingle(AudioClip clip) {
			efxSource.clip = clip;
			efxSource.Play();
		}

		public void RandomizeSfx(params AudioClip[] clips) {
			int randomIndex = Random.Range(0, clips.Length);
			float randomPitch = Random.Range(lowPitchRange, highPitchRange);
			efxSource.pitch = randomPitch;
			efxSource.clip = clips[randomIndex];
			efxSource.Play();
		}

		public void StopMusic() {
			musicSource.Stop();
		}
	}
}
