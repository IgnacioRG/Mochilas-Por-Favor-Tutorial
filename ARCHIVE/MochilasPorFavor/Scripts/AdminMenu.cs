using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MochilasPorFavor
{
    [RequireComponent(typeof(AudioSource))]
    public class AdminMenu : MonoBehaviour
    {
        public AudioSource music;
        public AudioSource engine;
        public AudioClip engineStartClip;
        public AudioClip engineLoopClip;


        void Start()
        {
            GetComponent<AudioSource>().loop = true;
            StartCoroutine(playEngineSound());
        }

        IEnumerator playEngineSound()
        {
            engine.clip = engineStartClip;
            engine.Play();
            yield return new WaitForSeconds(2.7f);
            engine.clip = engineLoopClip;
            engine.Play();
            music.Play();
        }


        public void ChangeScene(string sceneName)
        {
            Debug.Log("Changing Scene to " + sceneName);
            SceneManager.LoadScene(sceneName);
        }


    }
}
