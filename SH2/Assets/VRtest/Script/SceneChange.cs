using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class SceneChange : MonoBehaviour
{
    public GameObject TitlePanel;
    public GameObject HelpPanel;
    public GameObject MapSelectPanel;
    public GameObject EndPanel;

    public GameObject Pointer;
    public GameObject Button;
    public Text ButtonText;
    public GameObject PlayerController;
    public Light lightSwitch;

    [SerializeField] AudioClip[] clips;
    AudioSource source;

    float lightwaitTime = 0.0f;
    float footwaitTime = 0.0f;

    private void Awake()
    {
        //if (SceneManager.GetActiveScene().name == "Title")
           // source = GetComponent<AudioSource>().Play()[0];
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();

        //表示するパネル変更
        /*TitlePanel.SetActive(false);
        HelpPanel.SetActive(false);
        MapSelectPanel.SetActive(false);
        EndPanel.SetActive(false);
        Button.SetActive(false);
        Pointer.SetActive(false);
        PlayerController.GetComponent<OVRPlayerController>().enabled = true;*/

        if (SceneManager.GetActiveScene().name != "Game")
        {
            Button.SetActive(true);
            Pointer.SetActive(true);
            PlayerController.GetComponent<OVRPlayerController>().enabled = false;
        }

        if (SceneManager.GetActiveScene().name == "Title")
        {
            TitlePanel.SetActive(true);
            ButtonText.text = "Next";
            //source = gameObject.GetComponent<AudioSource>();
            source.PlayOneShot(clips[0]);
            source.loop = !source.loop;
        }
        else if (SceneManager.GetActiveScene().name == "Help")
        {
            HelpPanel.SetActive(true);
            ButtonText.text = "GameStart";
        }
        else if (SceneManager.GetActiveScene().name == "MapSelect")
        {
            MapSelectPanel.SetActive(true);
            ButtonText.text = "Title";
        }
        else if (SceneManager.GetActiveScene().name == "End")
        {
            EndPanel.SetActive(true);
            ButtonText.text = "Title";
        }
    }

    private void Update()
    {
        //マップ選択・タイトルへ割り込み
        if (OVRInput.Get(OVRInput.RawButton.A) &&
           OVRInput.Get(OVRInput.RawButton.B) &&
           OVRInput.Get(OVRInput.RawButton.RIndexTrigger) &&
           OVRInput.Get(OVRInput.RawButton.RHandTrigger) &&
           OVRInput.Get(OVRInput.RawButton.Start))
        {
            if (SceneManager.GetActiveScene().name == "Title")
                SceneManager.LoadScene("MapSelect");
            else if (SceneManager.GetActiveScene().name != "Title" && SceneManager.GetActiveScene().name != "MapSelect")
            {
                SceneManager.LoadScene("Title");
            }
        }

        if (OVRInput.Get(OVRInput.RawButton.Y))
            SceneManager.LoadScene("End");

        //サウンド        
        if (lightwaitTime <= 0.0f)
        {
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                LightSwitch();
        }
        else
        {
            lightwaitTime -= Time.deltaTime;
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstick))
            PlayFoot();
    }

    //シーン変更
    public void FadeInvoke()
    {
        if (SceneManager.GetActiveScene().name == "Help")
            Invoke("GameScene", 3);
        else if (SceneManager.GetActiveScene().name == "Title")
            Invoke("HelpScene", 3);
        else if (SceneManager.GetActiveScene().name == "End" || SceneManager.GetActiveScene().name == "MapSelect")
            Invoke("TitleScene", 3);
    }
    void GameScene()
    {
        SceneManager.LoadScene("Game");
        Button.SetActive(false);
        Pointer.SetActive(false);
    }
    void HelpScene()
    {
        SceneManager.LoadScene("Help");
    }
    void TitleScene()
    {
        SceneManager.LoadScene("Title");
    }


    void LightSwitch()
    {
        lightSwitch.enabled = !lightSwitch.enabled;
        source.PlayOneShot(clips[5]);
        lightwaitTime = 0.5f;
    }

    public void PlayFoot()
    {
        source.PlayOneShot(clips[Random.Range(1, 4)]);
    }
}