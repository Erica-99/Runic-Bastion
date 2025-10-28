using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public float baseAttack;

    public float attackBuff;

    private ICastable loadedAttack;
    private GameObject loadedAttackObject;

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
            loadedAttack = attackObject.GetComponent<ICastable>();

            MonoBehaviour attackmb = loadedAttack as MonoBehaviour;
            if (attackmb != null)
            {
                loadedAttackObject = attackmb.gameObject;
            }
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
                if (loadedAttackObject != null)
                {
                    loadedAttackObject.transform.localScale = new Vector3(baseAttack*attackBuff, baseAttack*attackBuff, baseAttack*attackBuff);
                }

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
