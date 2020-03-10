using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEnableUI : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] bool menuEnabled = false;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //EnableMenu(false);
    }

    public void EnableMenu(bool state)
    {
        if (state)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
        else
        {
            AudioSource.PlayClipAtPoint(audioClips[1], Camera.main.transform.position, 0.8f);
        }
        menu.SetActive(state);

    }
}
