using UnityEngine;

public class RockItemTest :
    MonoBehaviour
{
    [SerializeField]
    private RockTrap m_rockPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Use();
        }
    }

    private void Use()
    {
        RockTrap rock =
            Instantiate(
                m_rockPrefab
            );

        RockUseData data =
            new RockUseData();

        data.Owner =
            gameObject;

        data.Position =
            transform.position;

        data.Direction =
            transform.forward;

        data.Range =
            10f;

        rock.Initialize(data);
    }
}