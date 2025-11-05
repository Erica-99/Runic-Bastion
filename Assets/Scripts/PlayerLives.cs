using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static int Lives;
    public int startLives = 100;

    void Start()
    {
        Lives = startLives;
    }

    void Update()
    {
        if (Lives == 0)
        {
            print("I'm dead!!!");
        }
    }
}
