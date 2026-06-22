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
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(LoadScene("LibraryHub"));
    }

    public IEnumerator LoadScene(string sceneName) 
    {
        if (_sceneIsLoading == false)
        {
            Debug.Log("Loading Scene: " + sceneName + " via SceneController");
        
            //boolean to check if the coroutine is running for the continue button
            _sceneIsLoading = true;
        
            _progressBarTarget = 0;
            _progressBar.value = 0;
            var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            if (SceneManager.GetActiveScene().name != "ManagerScene")
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            
            if (sceneName == "LibraryHub")
            {
                yield return new WaitUntil(() => scene.progress == 1f);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("LibraryHub")); 
                
                GameManager.Instance.FlipPlayerJoining(SceneManager.GetActiveScene().name);
                GameManager.Instance.SwitchActionMaps("LibraryHub");
            }
            else
            {
                Debug.Log("Loading non-Library Scene");
                scene.allowSceneActivation = false;
                _loaderCanvas.SetActive(true);

                do {
                    _progressBarTarget = scene.progress;
                } while (scene.progress < 0.9f);
        
                //wait for keypress to finish loading.
                yield return new WaitUntil(() => _continuePressed);
                Debug.Log("Continuing to scene");
                GameManager.Instance.PlayerInputsActive(false);
                _loaderCanvas.SetActive(false);
                _continuePressed = false;
        
                //fully load the scene
                scene.allowSceneActivation = true;
                yield return new WaitUntil(() => scene.progress == 1f);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                
                GameManager.Instance.PlayerInputsActive(true);
                GameManager.Instance.FlipPlayerJoining(sceneName);
                GameManager.Instance.SwitchActionMaps(sceneName);
                GameManager.Instance.SwitchPlayerControllers(sceneName);
                
            }
            _sceneIsLoading = false;
        }
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
