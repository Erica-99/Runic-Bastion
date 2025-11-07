using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static int Lives;
    public int startLives = 100;

    void Awake()
    {
        Lives = startLives;
    }
}
