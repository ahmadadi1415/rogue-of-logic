using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject[] interactChoice;
    private Vector3? itemPosition;
    private GameObject interactedItem;

    private static ItemManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            // Debug.LogError("Found more than one Item Manager in the scene.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            interactPanel.SetActive(false);
            DontDestroyOnLoad(gameObject);
            itemPosition = Vector3.zero;
        }
    }

    public static ItemManager GetInstance() 
    {
        return instance;
    }

    private void Update() {
        if (interactPanel != null && interactPanel.activeSelf) {
            MovePanelToItemPosition();
        }
    }

    private bool _itemInteracting = false;
    public bool ItemInteracting {
        get {
            return _itemInteracting;
        }
        set {
            _itemInteracting = value;
        }
    }
    
    public void ShowInteractionPanel() {
        IsItemPanelActive = true;
        interactedItem = FindObjectOfType<InteractionItem>().interactedItem;
        itemPosition = interactedItem.transform.position;
        interactPanel.SetActive(true);
        ItemInteracting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInteracting;
        StartCoroutine(SelectFirstChoice());
    }

    private bool _isItemPanelActive = false;
    public bool IsItemPanelActive {
        get {
            return _isItemPanelActive;
        }
        set {
            _isItemPanelActive = value;
        }
    }

    public void HideInteractionPanel() {
        IsItemPanelActive = false;
        interactPanel.SetActive(false);
        itemPosition = null;
        interactedItem = null;
    }

    private void MovePanelToItemPosition() {
        if (itemPosition == null) {
            return;
        } 
        Vector3 interactPanelPosition = Camera.main.WorldToScreenPoint(new Vector3(itemPosition.Value.x, itemPosition.Value.y, itemPosition.Value.z));
        interactPanel.transform.position = interactPanelPosition;
        // Debug.Log(npcPosition);
    }

    
    private IEnumerator SelectFirstChoice() {
        // Unity event system need to be cleared, and then select it after at least one frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(interactChoice[0].gameObject);
    }

    public void ResetItemPosition() {
        interactedItem.GetComponent<Item>().ResetPosition();
        ItemInteracting = false;
        HideInteractionPanel();
    }

    public void ShowItemHints() {
        FindObjectOfType<LevelInformation>().UpdateInformation("When the rewind is clicked, the box is drawn back to its starting position.");
        ItemInteracting = false;
        HideInteractionPanel();
    }
}
