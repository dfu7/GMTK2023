using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject OptionsMenu;
    //[SerializeField] GameObject ControlsMenu;
    [SerializeField] AudioMixer masterAudioMixer;
    [SerializeField] GameObject Menu;

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
        Menu.SetActive(false);
    }

    /*public void OpenControls()
    {
        ControlsMenu.SetActive(true);
        Menu.SetActive(false);
    }
    */

    public void GoBack()
    {
        OptionsMenu.SetActive(false);
        //ControlsMenu.SetActive(false);
        Menu.SetActive(true);

    }

    public void GoToStart()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void SetSFXVolume(float level)
    {
        // must convert values from db using log10
        masterAudioMixer.SetFloat("sfx-volume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        masterAudioMixer.SetFloat("music-volume", Mathf.Log10(level) * 20);
    }

    public void ToggleMute(bool muted)
    {
        masterAudioMixer.SetFloat("master-volume", muted ? -80 : 0);
    }
}