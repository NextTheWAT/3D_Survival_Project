using UnityEngine;
using UnityEngine.Audio;
using Utils.Attribute;

namespace Utils.Management
{
    [DisallowMultipleComponent]
    public class SoundManager : SingletonGameObject<SoundManager>
    {
        #region SERIALIZABLE FIELD API

        [Header("Master Audio Mixer")]
        [Alias("Master")]
        [SerializeField] private AudioMixer masterAudioMixer;

        [Header("Audio Mixer Group")]
        [Alias("Background")]
        [SerializeField] private AudioMixerGroup backgroundAudioMixerGroup;
        [Alias("Effect")]
        [SerializeField] private AudioMixerGroup effectAudioMixerGroup;

        [Header("Audio Source Group")]
        [Alias("Background")]
        [SerializeField] private AudioSource backgroundAudioSource;
        [Alias("Effect")]
        [SerializeField] private AudioSource effectAudioSource;

        #endregion

        private void PlayBackgroundAudioSource_Internal(AudioClip clip)
        {
            backgroundAudioSource.clip = clip;
            backgroundAudioSource.Play();

            Debug.Log($"Play <b>{clip.name}</b>");
        }

        private void StopBackgroundAudioSource_Internal()
        {
            if (backgroundAudioSource.isPlaying)
            {
                backgroundAudioSource.Stop();
            }
        }

        private void PlayEffectAudioSource_Internal(AudioClip clip)
        {
            if (clip is null)
            {
                return;
            }

            effectAudioSource.PlayOneShot(clip);
        }

        private void SetBackgroundAudioSourceVolume_Internal(float volume)
        {
            masterAudioMixer.SetFloat(backgroundAudioMixerGroup.name, volume);
        }

        private void SetEffectAudioSourceVolume_Internal(float volume)
        {
            masterAudioMixer.SetFloat(effectAudioMixerGroup.name, volume);
        }

        private bool IsBackgroundAudioSourcePlaying_Internal(string name)
        {
            if (backgroundAudioSource is null)
            {
                return false;
            }

            return backgroundAudioSource.isPlaying && backgroundAudioSource.clip.name == name;
        }

        #region STATIC METHOD API

        /// <summary>
        /// Play the background audio source with the given audio clip.
        /// </summary>
        /// <param name="clip"> Background audio clip. </param>
        public static void PlayBackgroundAudioSource(AudioClip clip)
        {
            Instance.PlayBackgroundAudioSource_Internal(clip);
        }

        /// <summary>
        /// Stop the background audio source that is currently playing.
        /// </summary>
        public static void StopBackgroundAudioSource()
        {
            Instance.StopBackgroundAudioSource_Internal();
        }

        /// <summary>
        /// Play the effect audio source with the given audio clip.
        /// </summary>
        public static void PlayEffectAudioSource(AudioClip clip)
        {
            Instance.PlayEffectAudioSource_Internal(clip);
        }

        /// <summary>
        /// Set the volume of the background audio source.
        /// </summary>
        /// <param name="value"> Volume to set for audio source. </param>
        public static void SetBackgroundAudioSourceVolume(float volume)
        {
            Instance.SetBackgroundAudioSourceVolume_Internal(volume);
        }
        
        /// <summary>
        /// Set the volume of the effect audio source.
        /// </summary>
        /// <param name="value"> Volume to set for audio source. </param>
        public static void SetEffectAudioSourceVolume(float volume)
        {
            Instance.SetEffectAudioSourceVolume_Internal(volume);
        }

        /// <param name="name"> Name of audio clip. </param>
        /// <returns> Whether the audio clip for the given name is playing from the audio source. </returns>
        public static bool IsBackgroundAudioSourcePlaying(string name)
        {
            return Instance.IsBackgroundAudioSourcePlaying_Internal(name);
        }

        #endregion

        #region STATIC PROPERTIES API

        public static bool IsBackgroundAudioSourceMute
        {
            get => Instance.backgroundAudioSource.mute; 
            set => Instance.backgroundAudioSource.mute = value;
        }

        public static bool IsEffectAudioSourceMute
        {
            get => Instance.effectAudioSource.mute; 
            set => Instance.effectAudioSource.mute = value;
        }

        #endregion
    }
}
