using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public static void StartScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

}
