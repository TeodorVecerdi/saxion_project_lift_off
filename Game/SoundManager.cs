using System.Collections.Generic;
using GXPEngine;

namespace Game {
    public class SoundManager {
        private static SoundManager instance;
        public static SoundManager Instance => instance ?? (instance = new SoundManager());
        
        private Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();
        private Dictionary<string, SoundChannel> soundChannels = new Dictionary<string, SoundChannel>();

        private SoundManager() {
            sounds.Add("drilling", new Sound("data/sounds/Drilling_1.wav"));
            sounds.Add("stoneHit", new Sound("data/sounds/StoneHit.wav"));
            sounds.Add("ModeSwitch", new Sound("data/sounds/ModeSwitch.wav"));
            sounds.Add("UpgradeMined", new Sound("data/sounds/UpgradeMined.wav"));
            sounds.Add("fuelLow", new Sound("data/sounds/LowFuel.wav", true));
            sounds.Add("ambient", new Sound("data/sounds/PlayerGeneral_Ambience_V1.wav", true));
        }

        public void Play(string soundName, bool restartIfAlreadyPlaying = true) {
            if (!sounds.ContainsKey(soundName)) {
                Debug.LogError($"Sound {soundName} does not exist.");
                return;
            }

            if (soundChannels.ContainsKey(soundName)) {
                if (restartIfAlreadyPlaying) {
                    soundChannels[soundName].Stop();
                    soundChannels[soundName] = null;
                    soundChannels.Remove(soundName);
                    soundChannels[soundName] = sounds[soundName].Play();
                }
            } else {
                soundChannels[soundName] = sounds[soundName].Play();
            }
        }

        public void Stop(string soundName) {
            if (!sounds.ContainsKey(soundName)) {
                Debug.LogError($"Sound {soundName} does not exist.");
                return;
            }
            if (!soundChannels.ContainsKey(soundName)) {
                return;
            }

            soundChannels[soundName].Stop();
            soundChannels[soundName] = null;
            soundChannels.Remove(soundName);
        }

        public void SetVolume(string soundName, float volume) {
            if (!soundChannels.ContainsKey(soundName)) {
                Debug.LogWarning($"Sound {soundName} is not currently playing.");
                return;
            }

            if (volume < 0 || volume > 1) {
                Debug.LogWarning("Volume should be between 0 and 1.");
                volume = Mathf.Clamp01(volume);
            }

            soundChannels[soundName].Volume = volume;
        }
    }
}