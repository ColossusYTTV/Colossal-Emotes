using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Photon.Pun;
using UnityEngine;

namespace Colossal
{
    internal class AssetBundleLoader : MonoBehaviour
    {
        public static AssetBundle bundle;
        public static GameObject assetBundleParent;
        public static string parentName = "ColossalEmotes";

        public static GameObject KyleRobot;
        public static AudioSource audioSource;
        public static GameObject Promo;

        public static Dictionary<string, AudioClip> audioPool = new Dictionary<string, AudioClip> { };

        public void Start()
        {
            Debug.Log("[EMOTE] Asset Bundle Loader Start");

            // Load the asset bundle
            bundle = LoadAssetBundle("ColossalEmotes.AssetBundle.colossalemotes");
            if (bundle != null)
            {
                assetBundleParent = Instantiate(bundle.LoadAsset<GameObject>(parentName));

                if (assetBundleParent != null)
                {
                    assetBundleParent.transform.position = new Vector3(-67.2225f, 11.8f, -82.59f);

                    KyleRobot = assetBundleParent.transform.GetChild(0).gameObject;
                    if (KyleRobot != null)
                        audioSource = KyleRobot.GetComponent<AudioSource>();

                    Promo = assetBundleParent.transform.GetChild(1).gameObject;

                    // Populate the audioPool dictionary with all audio clips in the bundle
                    LoadAudioClips();
                }
                else
                {
                    Debug.Log("[EMOTE] assetBundleParent is null");
                }
            }
            else
            {
                Debug.Log("[EMOTE] bundle is null");
            }
        }

        // Load the asset bundle from a file or resource
        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            if (stream == null)
            {
                Debug.Log("[Emote] Could not find resource at path: " + path);
                return null;
            }

            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        // Load all audio clips from the asset bundle and store them in the audioPool dictionary
        public void LoadAudioClips()
        {
            if (bundle == null)
            {
                Debug.LogError("[EMOTE] AssetBundle is null.");
                return;
            }

            // Find all AudioClip assets in the AssetBundle
            AudioClip[] audioClips = bundle.LoadAllAssets<AudioClip>();
            foreach (AudioClip clip in audioClips)
            {
                if (!audioPool.ContainsKey(clip.name))
                {
                    audioPool.Add(clip.name, clip);
                    Debug.Log("[EMOTE] Loaded AudioClip: " + clip.name);
                }
            }
        }

        // Play an audio clip by its name from the asset bundle
        public static void PlayAudioByName(string audioClipName)
        {
            if (audioSource == null)
            {
                Debug.LogError("[EMOTE] AudioSource is not assigned.");
                return;
            }

            // Check if the audio clip exists in the pool
            if (audioPool.ContainsKey(audioClipName))
            {
                AudioClip clip = audioPool[audioClipName];
                audioSource.clip = clip;
                audioSource.Play();

                // SS Audio
                if(PhotonNetwork.InRoom)
                {
                    GorillaTagger.Instance.myRecorder.SourceType = Photon.Voice.Unity.Recorder.InputSourceType.AudioClip;
                    GorillaTagger.Instance.myRecorder.AudioClip = clip;
                    GorillaTagger.Instance.myRecorder.RestartRecording();
                }

                Debug.Log("[EMOTE] Playing AudioClip: " + audioClipName);
            }
            else
            {
                Debug.LogError("[EMOTE] AudioClip not found: " + audioClipName);
            }
        }
    }
}