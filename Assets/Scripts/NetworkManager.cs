using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    private string serverUrl = "http://localhost:3000"; 
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI resultText;

    public string loggedInEmail;
    public static string userId;  
    public string loggedInUserId;

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

    // Register a new user with username, email, and password
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

    [System.Serializable]
    public class LoginResponse
    {
        public string user_id; 
    }
    public bool isLoggedIn => !string.IsNullOrEmpty(userId);

    public IEnumerator LoginUser(string email, string password, System.Action<string> callback)
    {
        string jsonData = $"{{\"email\":\"{email}\", \"password\":\"{password}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;

                var loginResponse = JsonUtility.FromJson<LoginResponse>(response);
                loggedInUserId = loginResponse.user_id; 
                userId = loggedInUserId; 

                callback(response); 
            }
            else
            {
                callback(request.error); // Callback with error message
            }
        }
    }

    // UpdateProgress method in NetworkManager
    public IEnumerator UpdateProgress(string level, float oxygen, float time, int checkpoint, int score)
    {
        // Construct the data to send to the server
        string url = "http://localhost:3000/update-progress";  // Replace with your actual endpoint
        WWWForm form = new WWWForm();
        form.AddField("level", level);
        form.AddField("oxygen", oxygen.ToString());
        form.AddField("time", time.ToString());
        form.AddField("checkpoint", checkpoint);
        form.AddField("score", score);

        // Send the data
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Progress update sent successfully!");
            }
        }
    }

    [System.Serializable]
    public class ProgressData
    {
        public string userId;
        public string level;
        public float oxygen;
        public float time;
        public int checkpoint;
        public int score;
    }


    // Register user button handler
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
                SceneManager.LoadScene("Main menu (register)"); // Load the correct scene after registration
            }
        }));
    }

    // Login user button handler
    public void LoginUserButton()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(LoginUser(email, password, (response) =>
        {
            if (response.Contains("error"))
            {
                resultText.text = "Login Failed: " + response;
            }
            else
            {
                loggedInEmail = email;
                resultText.text = "Login Success";
                Debug.Log("Login Success: " + response);
                SceneManager.LoadScene("Main menu (login)"); // Load the correct scene after login
            }
        }));
    }
}
