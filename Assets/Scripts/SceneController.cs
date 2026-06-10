using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private float _progressBarTarget;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName + " via SceneController");

        _progressBarTarget = 0;
        _progressBar.value = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
     
        _loaderCanvas.SetActive(true);

        do {
            _progressBarTarget = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(10000); //remove this
        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }
    
    private void Update()
    {
        _progressBar.value = Mathf.MoveTowards(_progressBar.value, _progressBarTarget, Time.deltaTime * 10f);
    }
}
