using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public static void StartGame(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

}
