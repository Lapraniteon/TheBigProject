using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private float _progressBarTarget;
    
    [SerializeField] private bool _sceneIsLoading;
    [SerializeField] private bool _continuePressed;

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

    public IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName + " via SceneController");
        
        //boolean to check if the coroutine is running for the continue button
        _sceneIsLoading = true;
        
        _progressBarTarget = 0;
        _progressBar.value = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
     
        _loaderCanvas.SetActive(true);

        do {
            _progressBarTarget = scene.progress;
        } while (scene.progress < 0.9f);
        
        yield return new WaitUntil(() => _continuePressed);
        Debug.Log("Continuing to scene");
        GameManager.Instance.PlayerInputsActive(false);
        _loaderCanvas.SetActive(false);
        _sceneIsLoading = false;
        _continuePressed = false;
        scene.allowSceneActivation = true;
        GameManager.Instance.PlayerInputsActive(true);
    }
    
    private void Update()
    {
        _progressBar.value = Mathf.MoveTowards(_progressBar.value, _progressBarTarget, Time.deltaTime * 10f);
    }

    private void ContinuePressed()
    {
        if (_sceneIsLoading)
        {
            _continuePressed = true;
            Debug.Log("Continue pressed");
        }
    }

    private void OnEnable()
    {
        PlayerController.Continue += ContinuePressed;
    }

    private void OnDisable()
    {
        PlayerController.Continue -= ContinuePressed;
    }
}
