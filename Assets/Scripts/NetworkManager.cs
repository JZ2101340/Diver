using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    private string serverUrl = "http://localhost:3000"; 
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI resultText;

    public static string userId;
    private string loggedInEmail;
    public static readonly string[] levelScenes = { "Easy", "Medium", "Hard", "Very Hard" };
    public bool isLoggedIn => !string.IsNullOrEmpty(userId);

    [System.Serializable]
    public class ProgressData
    {
        public string user_id;
        public string current_level;
        public int checkpointsReached;
        public int oxygen_level;
        public int total_score;
        public int time_taken;
    }



    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("NetworkManager");
                _instance = obj.AddComponent<NetworkManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject); 
        }
    }
    public void StartProgressSaving()
    {
        StartCoroutine(UpdateProgressPeriodically());
    }

    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(LoginUser(email, password));
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string id;
        public string username;
        public string email;
    }

    private IEnumerator LoginUser(string email, string password)
    {
        string url = "http://localhost:3000/login";
        string jsonData = $"{{\"email\":\"{email}\", \"password\":\"{password}\"}}";

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Login failed: " + www.error);
            }
            else
            {
                string response = www.downloadHandler.text;
                LoginResponse loginData = JsonUtility.FromJson<LoginResponse>(response);

                userId = loginData.id;
                loggedInEmail = loginData.email;

                Debug.Log($"Login Success! userId = {userId}");
                SceneManager.LoadScene("Main menu (login)");
            }
        }
    }


    public void RegisterUserButton()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(RegisterUser(username, email, password, (response) =>
        {
            if (response.Contains("error"))
            {
                resultText.text = "Registration Failed: " + response;
            }
            else
            {
                resultText.text = "Registration Success";
                Debug.Log("Register Success: " + response);
                SceneManager.LoadScene("Main menu (register)");
            }
        }));
    }

    public IEnumerator RegisterUser(string username, string email, string password, System.Action<string> callback)
    {
        string jsonData = $"{{\"username\":\"{username}\", \"email\":\"{email}\", \"password\":\"{password}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/register", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            callback(request.result == UnityWebRequest.Result.Success ? request.downloadHandler.text : request.error);
        }
    }
    private IEnumerator UpdateProgressPeriodically()
    {
        while (OxygenManager.Instance == null || ScoreManager.Instance == null || CheckpointManager.Instance == null || TimerManager.Instance == null)
        {
            Debug.Log("Waiting for managers to initialize...");
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("All managers ready. Starting live progress saving...");

        while (true)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (isLoggedIn && System.Array.Exists(levelScenes, scene => scene == currentScene))
            {
                Debug.Log("Saving Live Progress...");

                yield return UpdateProgress(
                    currentScene,
                    Mathf.FloorToInt(OxygenManager.Instance.GetOxygenLevel()),
                    Mathf.FloorToInt(TimerManager.Instance.GetTime()),
                    CheckpointManager.Instance.GetReachedCheckpointCount(),
                    ScoreManager.Instance.GetScore()
                );
            }
            else
            {
                Debug.Log("Skipping progress save in non-level scene: {currentScene}");
            }

            yield return new WaitForSeconds(3f);
        }
    }




    public IEnumerator UpdateProgress(string level, int oxygen, int timeTaken, int lastCheckpoint, int score)
    {
        string url = serverUrl + "/update-progress";

        ProgressData data = new ProgressData
        {
            user_id = userId,
            current_level = level,
            checkpointsReached = lastCheckpoint,
            oxygen_level = oxygen,
            total_score = score,
            time_taken = timeTaken
        };

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest www = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Progress saved successfully!");
        }
        else
        {
            Debug.LogError("Progress save failed: " + www.error);
        }
    }


    public void ContinueProgress()
    {
        if (isLoggedIn)
            LoadProgress();
        else
        {
            if (resultText != null)
                resultText.text = "Please log in first!";
            else
                Debug.LogWarning("ResultText UI is not set. Please log in first!");
        }
    }


    private void LoadProgress()
    {
        StartCoroutine(LoadProgressCoroutine());
    }

    private IEnumerator LoadProgressCoroutine()
    {
        string url = $"{serverUrl}/progress/{userId}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Failed to load progress: " + www.error);
                resultText.text = "Failed to load progress!";
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Loaded progress: " + responseText);

                if (string.IsNullOrEmpty(responseText) || responseText == "{}")
                {
                    Debug.LogError("No saved progress found for user.");
                    yield break;
                }

                ProgressData progress = JsonUtility.FromJson<ProgressData>(responseText);
                ApplyProgress(progress);

            }
        }
    }


    private void ApplyProgress(ProgressData progress)
    {
        if (progress != null && !string.IsNullOrEmpty(progress.current_level))
        {
            PlayerPrefs.SetInt("restoreProgress", 1);
            PlayerPrefs.SetString("currentLevel", progress.current_level);
            PlayerPrefs.SetInt("oxygenLevel", progress.oxygen_level);
            PlayerPrefs.SetInt("checkpointsReached", progress.checkpointsReached);
            PlayerPrefs.SetInt("totalScore", progress.total_score);
            PlayerPrefs.SetInt("timeTaken", progress.time_taken);

            PlayerPrefs.Save(); 
            Debug.Log("Saved Progress to PlayerPrefs.");

            SceneManager.LoadScene(progress.current_level);
        }
        else
        {
            Debug.LogError("Cannot load scene: progress is null or currentLevel is empty.");
        }
    }




}
