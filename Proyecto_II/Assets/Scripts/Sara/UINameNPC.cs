using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINameNPC : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private RectTransform panelTransform;

    private Transform currentNPC;
    private Vector3 npcOffset;

    public void ShowNPCPanelName(string npcName, Transform npcTransform)
    {
        interactionText.text = $"Habla con {npcName}";
        interactionPanel.SetActive(true);
        currentNPC = npcTransform;

        if (npcTransform.TryGetComponent(out Renderer renderer))
            npcOffset = new Vector3(0, renderer.bounds.max.y - npcTransform.position.y + 0.5f, 0f);
    }


    public void HideNPCPanelName()
    {
        interactionPanel.SetActive(false);
        currentNPC = null;
    }

    private void Update()
    {
        if (currentNPC != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentNPC.position + npcOffset);
            panelTransform.position = screenPos;
        }
    }
}
