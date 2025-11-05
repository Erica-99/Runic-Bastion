using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public float baseAttack;

    public float attackBuff;

    public GameObject playerCamera;

    private ICastable loadedAttack;

    private Manager manager;

    private void Awake()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        manager = gm.GetComponent<Manager>();
    }

    public void LoadAttack(GameObject attackObject)
    {
        attackObject.transform.parent = transform;

        if (attackObject.GetComponent<ICastable>() == null)
        {
            Destroy(attackObject);
            return;
        } else
        {
            if (loadedAttack != null && loadedAttack.casted == false)
            {
                MonoBehaviour mb = loadedAttack as MonoBehaviour;

                Destroy(mb.gameObject);
            }

            loadedAttack = attackObject.GetComponent<ICastable>();
        }
    }

    public void ReadyAttack()
    {
        if (loadedAttack != null)
        {
            loadedAttack.ReadyCast(gameObject);
        }
    }

    public void Update()
    {
        if (loadedAttack != null)
        {
            if (loadedAttack.readied)
            {
                loadedAttack.damageBuff = attackBuff;

                manager.SetCrosshairState(true);
            } else
            {
                manager.SetCrosshairState(false);
            }

            if (loadedAttack.casted)
            {
                loadedAttack = null;
            }
        }
    }
}
