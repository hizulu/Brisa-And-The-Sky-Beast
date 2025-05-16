using TMPro;
using UnityEngine;

/*
 * NOMBRE CLASE: UINameNPC
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 18/04/2025
 * DESCRIPCIÓN: Muestra un panel con el nombre del NPC con el que puede interactuar Player.
 * VERSIÓN: 1.0. 
 */

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
        SkinnedMeshRenderer npcRender = npcTransform.GetComponentInChildren<SkinnedMeshRenderer>();

        if (npcRender != null)
        {
            float height = npcRender.bounds.size.y;
            npcOffset = new Vector3(0f, height + 0.5f, 0f);
        }
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
