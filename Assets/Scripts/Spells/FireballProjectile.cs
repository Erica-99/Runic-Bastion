using UnityEngine;

public class FireballProjectile : MonoBehaviour, ICastable
{
    public void ReadyCast()
    {
        // To be called by attack manager. Set up floating fireball in hand or whatever.
    }

    public void DoCast()
    {
        // Shoot the fireball forwards.
    }
}
