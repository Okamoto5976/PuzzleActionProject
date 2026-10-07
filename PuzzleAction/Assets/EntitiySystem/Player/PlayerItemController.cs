using UnityEngine;

public class PlayerItemController
{
    public PlayerItemController(
        PlayerController player,
        InventorySystem inventory
        )
    {
        m_playerController = player;
        m_inventorySystem = inventory;
    }

    //------------component----------
    private PlayerController m_playerController;

    private InventorySystem m_inventorySystem;

    //=========Use Item========================
    private Vector3 m_playerPos => m_playerController.transform.position;

    public bool m_isUsingArrow = false;
    public bool m_isUsingSetItem = false;
    public bool m_isUsingAttackItem = false;

    private Vector3 m_setOffSet;

    private Vector3 m_arrowTemporaryForward;

    private float m_power;


    private float m_trapPlaceRange = 5f;
    private Vector3 m_trapSetPosition;

    //-------Search Item====================

    private float m_itemSearchRange = 5f;

    private float m_closeMessageTime = 0f;

    //==============================
    //Use Item Method
    //==============================

    public void UseItemCancel()
    {
        m_playerController.CancelMessageActive(false);


        if (m_isUsingArrow)
        {
            m_isUsingArrow = false;
            m_playerController.ReticleActive(false);
            m_playerController.AimTrailActive(false);

            m_power = 0f;
        }
        else if(m_isUsingSetItem)
        {
            m_isUsingSetItem= false;
            m_playerController.ReticleActive(false);
            m_playerController.SetItemPreview(false);

            m_power = 0f;
        }
        else if(m_isUsingAttackItem)
        {
            m_isUsingAttackItem = false;
            m_power = 0f;
        }
    }

    public void UseItemPressed(int hotberIndex)
    {
        //Debug.Log("Pressed");

        if (m_inventorySystem.IsCheckCurrentItem(hotberIndex, ItemUseType.Arrow))
        {
            m_playerController.CancelMessageActive(true);


            m_isUsingArrow = true;

            m_power = 0f;
            m_playerController.ReticleActive(true);


            m_playerController.AimTrailActive(true);

            //start to pull the bow



        }
        else if (m_inventorySystem.IsCheckCurrentItem(hotberIndex, ItemUseType.Set))
        {
            m_playerController.CancelMessageActive(true);


            m_isUsingSetItem = true;
            m_power = 0f;

            m_playerController.ReticleActive(true);


            m_playerController.SetItemPreview(true);

        }
        else if (m_inventorySystem.IsCheckCurrentItem(hotberIndex, ItemUseType.Attack))
        {
            m_playerController.CancelMessageActive(true);


            m_isUsingAttackItem = true;
            m_power = 3f;
        }
        else
        {
            ItemRecieveData data = CreateItemData(m_playerController.Forward, 0f, m_playerController.m_pullOffSet);

            m_inventorySystem.UsePressed(hotberIndex, data);

        }

    }


    public void UseItemHold()
    {


        if (m_isUsingArrow)
        {
            OnReticle();

            m_power += Time.deltaTime * 1.5f;

            m_power = Mathf.Min(m_power, 3f);

            m_playerController.m_aimTrail.UpdateVariables(m_playerController.m_pullOffSet, m_arrowTemporaryForward, m_power * 8);
        }
        else if (m_isUsingSetItem)
        {
            OnReticle();

            m_playerController.m_trapPreview.transform.position = m_trapSetPosition;
        }
        else if (m_isUsingAttackItem)
        {
            OnReticle();

            m_power += Time.deltaTime;

            m_power = Mathf.Min(m_power, 4f);
        }
    }

    public void UseItemRelease(int hotberIndex)
    {
        m_playerController.CancelMessageActive(false);

        if (m_isUsingArrow)
        {
            m_isUsingArrow = false;

            m_playerController.ReticleActive(false);

            m_playerController.AimTrailActive(false);

            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward, m_power * 8, m_playerController.m_pullOffSet);
            m_inventorySystem.UseRelease(hotberIndex, data);

        }
        else if (m_isUsingSetItem)
        {
            m_isUsingSetItem = false;

            m_playerController.ReticleActive(false);

            m_playerController.SetItemPreview(false);

            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward, 0f, m_setOffSet);
            m_inventorySystem.UseRelease(hotberIndex, data);
        }
        else if (m_isUsingAttackItem)
        {
            m_isUsingAttackItem = false;

            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward, m_power);
            m_inventorySystem.UseRelease(hotberIndex, data);
        }
    }

    private ItemRecieveData CreateItemData(
    Vector3 forward,
    float power,
    Vector3 offset = new Vector3())
    {
        return new ItemRecieveData
        {
            entity = m_playerController,
            power = power,
            pos = m_playerController.transform.position,
            dir = forward,
            offset = offset,

        };
    }

    private void OnReticle()
    {
        m_playerController.m_reticle.position = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0f, m_playerPos.y, 0f));

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 mousePos = ray.GetPoint(distance);

            Vector3 dir = mousePos - m_playerPos;
            dir.y = 0f;

            m_playerController.OnRotatePlayerDirObject(dir);

            //temporary save, use when arrow pull
            m_arrowTemporaryForward = m_playerController.Forward;

            //trap

            Vector3 offset = mousePos - m_playerPos;
            offset.y = 0f;

            if (offset.magnitude > m_trapPlaceRange) offset = offset.normalized * m_trapPlaceRange;

            m_trapSetPosition = m_playerPos + offset;

            m_setOffSet = offset;
        }

    }

    //==============================
    //ItemSearch Method
    //==============================

    public void Update()
    {
        if(m_closeMessageTime <= 0f)
        {
            CloseErrorMessage();
        }
        else
        {
            m_closeMessageTime -= Time.deltaTime;
        }
    }

    private DropItem GetNearestItem()
    {
        Collider[] hits = Physics.OverlapSphere(
        m_playerPos,
        m_itemSearchRange,
        m_playerController.m_itemLayerMask
        );

        DropItem nearest = null;
        float nearestDistanceSqr = m_itemSearchRange * m_itemSearchRange;

        foreach (Collider hit in hits)
        {
            DropItem item = hit.GetComponentInParent<DropItem>();

            if (item == null)
                continue;

            float distanceSqr =
                (item.transform.position - m_playerPos).sqrMagnitude;

            if (distanceSqr <= nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearest = item;
            }
        }

        return nearest;
    }

    public void GetItem()
    {
        DropItem item = GetNearestItem();

        if (item == null) return;

        if (ReceiveItem(item.ItemData))
        {
            item.ItemGet();
        }
        else
        {
            m_closeMessageTime = 3f;
            m_playerController.TextErrorMessageActive(true);
        }
    }

    public bool CanGetItem()
    {
        DropItem item = GetNearestItem();

        if (item != null) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual bool ReceiveItem(Item item)
    {
        //Debug.Log("ReceiveItemŠJŽn");

        if (item == null)
        {
            //Debug.Log("item‚ªnull");
            return false;
        }

        if (m_inventorySystem == null)
        {
            //Debug.Log("InventorySystem‚ªnull");
            return false;
        }

        //Debug.Log("AddItem‚ðŒÄ‚Ñ‚Ü‚·");

        bool success = m_inventorySystem.AddItem(item, 1);

        //Debug.Log("AddItemI—¹ : " + success);

        return success;
    }

    private void CloseErrorMessage()
    {
        m_playerController.TextErrorMessageActive(false);
    }

    private DropItem m_hoverItem;
    private Vector3 m_popupPosition;
    public void SearchItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, m_playerController.m_itemLayerMask))
        {
            DropItem drop = hit.collider.GetComponentInParent<DropItem>();

            if (drop != null)
            {

                float distancee = Vector3.Distance(m_playerPos, drop.transform.position);

                if (distancee > m_itemSearchRange)
                {
                    m_hoverItem = null;
                    m_playerController.ItemTextDescription("");
                    return;
                }
                if (m_hoverItem != drop)
                {
                    m_hoverItem = drop;
                    m_popupPosition = Input.mousePosition + new Vector3(20f, -20f, 0f);
                }

                m_playerController.ItemDescriptionPanelActive(true);

                m_playerController.ItemTextName(drop.ItemData.ItemName);
                //m_itemDescriptionText.rectTransform.position = m_popupPosition;
                m_playerController.m_textPanel.position = m_popupPosition;
                m_playerController.ItemTextDescription(drop.ItemData.info);
                return;
            }
        }
        m_hoverItem = null;
        m_playerController.ItemTextDescription("");

        m_playerController.ItemDescriptionPanelActive(false);

    }
}
