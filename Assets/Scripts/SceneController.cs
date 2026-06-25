using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Sprite[] loadingImages;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private float _progressBarTarget;
    [SerializeField] private GameObject _continueInstruction;
    
    [SerializeField] private bool _sceneIsLoading;
    [SerializeField] private bool _continuePressed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    /// <summary>
    /// Load a new scene using the string variable. Unloads the old one. Opens a load screen during the laod.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public IEnumerator LoadScene(string sceneName) 
    {
        if (_sceneIsLoading == false)
        {
            Debug.Log("Loading Scene: " + sceneName + " via SceneController");
            
            //Disable the cars if exiting the Darius Legs minigame.
            if (SceneManager.GetActiveScene().name == "DariusLegsMinigame")
            {
                GameManager.Instance.SetCarActive(false);
                Debug.Log("Disabled Cars");
            }
            //unload the current scene unless it's the ManagerScene
            if (SceneManager.GetActiveScene().name != "ManagerScene")
            {
                var UnloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => UnloadOperation.isDone);
            }

            SetLoadingScreen(sceneName);
            
            //boolean to check if a scene is loading for the continue button.
            _sceneIsLoading = true;
            
            //load the new scene
            var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            do {
                _progressBarTarget = scene.progress;
            } while (scene.progress < 0.9f);
            
            //Continue on scene laod if it's loading the libraryhub
            if (sceneName == "LibraryHub")
            {
                yield return new WaitUntil(() => scene.progress == 1f);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("LibraryHub")); 
            }
            //Otherwise wait for ContinuePress and sceneload
            else
            {
                //stop the scene from fully laoding
                scene.allowSceneActivation = false;
                
                //Wait for the scene to almost be done loading
                yield return new WaitUntil(() => scene.progress >= 0.9f);
                
                //Deactivate Progressbar.
                _progressBar.gameObject.SetActive(false);
                //Activate the continue Instruction
                _continueInstruction.SetActive(true);
                
                //wait for keypress to finish loading.
                yield return new WaitUntil(() => _continuePressed);
                //fully load the scene
                scene.allowSceneActivation = true;
                
                //wait until scene is fully laoded
                yield return new WaitUntil(() => scene.progress == 1f);
                //set new scene as the active one
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                _continueInstruction.SetActive(false);
            }

            if (sceneName == "DariusLegsMinigame")
            {
                GameManager.Instance.SetCarActive(true);
                Debug.Log("Enabled Cars");
            }
            GameManager.Instance.FlipPlayerJoining(sceneName); //disable or enable player joining dependent on the scene.
            GameManager.Instance.SwitchActionMaps(sceneName); //switch to the corresponding action map.
            GameManager.Instance.SwitchPlayerControllers(sceneName); //Enable the correct additional player script and disable the rest.
            _loaderCanvas.SetActive(false); //disable the load screen
            _sceneIsLoading = false; //set sceneIsLoading to false to prevent _continuePressed from being changed.
            _continuePressed = false; //set _continuePressed to false to make sure the next scene loads correctly
        }
    }
    
    //Update the progressbar on the sceneload screen to show how far it's loaded.
    private void Update()
    {
        _progressBar.value = Mathf.MoveTowards(_progressBar.value, _progressBarTarget, Time.deltaTime * 10f);
    }

    private void SetLoadingScreen(string sceneName)
    {
        //resets the progressBar to ensure it's correct.
        _progressBar.gameObject.SetActive(true);
        _progressBarTarget = 0;
        _progressBar.value = 0;
        
        _loaderCanvas.SetActive(true); //enables the load screen.
        int sceneIndex = Array.IndexOf(GameManager.Instance.scenes, sceneName);
        if (sceneIndex > 4)
        {
            sceneIndex = 4;
        }
        _loaderCanvas.GetComponentInChildren<Image>().sprite = loadingImages[sceneIndex];
    }

    //sets the _continuePressed boolean if continue is pressed and a scene is loaded to allow the LoadScene method to continue.
    private void ContinuePressed()
    {
        if (_sceneIsLoading)
        {
            _continuePressed = true;
            Debug.Log("Continue pressed");
        }
    }

    //Subscription to the PlayerController Continue event.
    private void OnEnable()
    {
        PlayerController.Continue += ContinuePressed;
    }

    private void OnDisable()
    {
        PlayerController.Continue -= ContinuePressed;
    }
}
