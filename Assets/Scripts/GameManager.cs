using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.Video;
using TextSpeech;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Text text;
    public Animator animator;
    public GameObject winLose;
    Text winLoseText;
    const string languageCode = "en-US";

    private void Awake()
    {
        CheckPermissions();
        Initialise();
        videoPlayer.loopPointReached += OnEndReached;
        winLoseText = winLose.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        winLose.SetActive(false);
        animator.gameObject.SetActive(false);
    }

    private void Start()
    {
        SpeechToText.instance.onResultCallback += OnFinalSpeech;
    }

    void CheckPermissions()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);
#endif
    }


    void OnEndReached(VideoPlayer videoPlayer)
    {
        text.text = "Speak";
        ShowPopUp();
        StartListening();
    }

    void Initialise()
    {
        SpeechToText.instance.Setting(languageCode);
    }

    void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }

    void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }

    void OnFinalSpeech(string result)
    {
        StopListening();
        HidePopUp();
        Debug.Log("Done" + result);
        text.text = result;
        if (text.text == "D" || text.text == "d")
        {
            ShowWinPopUp();
        }
        else
        {
            ShowLosePopUp();
        }

    }

    void ShowPopUp()
    {
        animator.gameObject.SetActive(true);
        animator.Play("Show");
    }

    void HidePopUp()
    {
        animator.Play("Hide");
    }

    void ShowWinPopUp()
    {
        winLose.SetActive(true);
        winLoseText.text = "You Won";
    }

    void ShowLosePopUp()
    {
        winLose.SetActive(true);
        winLoseText.text = "You Lost";
    }

    public void OnClickReplay()
    {
        SceneManager.LoadScene("Mainmenu");
    }
}
