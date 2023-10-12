using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public delegate void PlayerFire();
    public static event PlayerFire OnPlayerFire;

    public void invokeFireEvent()
    {
        OnPlayerFire?.Invoke();
    }
}
