#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.InputSystem;
#endregion

/*
 * NOMBRE CLASE: DialogManager
 * AUTOR: Lucía García López
 * FECHA: 10/04/2025
 * DESCRIPCIÓN: Clase que gestiona el sistema de diálogos. Funciona con archivos CSV. Los dialogos pueden tener varias opciones de respuesta 
 * y opciones condicionales que solo aparecen si se ha desbloqueado otra linea de dialogo antes.
 * VERSIÓN: 1.0 Sistema de diálogos inicial.
 * 1.1 Añadida corrutina para hacer el efecto de máquina de escribir.
 * 1.2 Añadido el efecto de pulsar la tecla para continuar. Se puede usar el ratón o el teclado.
 * 1.3 DialogIDRead para el funcionamiento de apertura de una puerta en la escena 2.
 * 1.4 Hay ciertas preguntas de brisa que solo aparecen si ha recogido un arma especial
 */

public class DialogManager : MonoBehaviour
{
    #region Variables
    [Header("Data")]
    public TextAsset csvFile;

    [Header("UI References")]
    public GameObject dialogPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;
    public Image continueIndicator;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Special Weapon")]
    [SerializeField] WeaponSlot weaponSlot;
    [SerializeField] WeaponData specialWeapon;

    private Dictionary<int, DialogEntry> dialogDict = new Dictionary<int, DialogEntry>();
    private List<int> unlockedDialogIDs = new List<int>();
    private List<int> seenDialogIDs = new List<int>();

    private int currentID, startID, endID;
    private DialogEntry currentEntry;
    private DialogEntry lastOptionsEntry;

    private bool isDialogActive = false;
    private bool isTyping = false;
    private bool textCompleted = false;
    private Coroutine typingCoroutine;
    private Coroutine indicatorCoroutine;
    #endregion

    void Awake()
    {
        LoadDialogFromCSV();
        dialogPanel.SetActive(false);
        HideAllOptions();
        continueIndicator.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerInput.UIPanelActions.DialogueContinue.performed += OnContinuePerformed; // Suscribirse al evento de continuar diálogo
    }

    private void OnDisable()
    {
        playerInput.UIPanelActions.DialogueContinue.performed -= OnContinuePerformed; // Desuscribirse del evento de continuar diálogo
    }

    private void OnContinuePerformed(InputAction.CallbackContext context)
    {
        if (!isDialogActive) return;

        // Determinar qué input se usó
        bool isKeyboard = context.control.device is Keyboard;
        bool isMouse = context.control.device is Mouse;

        if (isTyping)
        {
            CompleteCurrentText();
        }
        else if (textCompleted && !AnyOptionActive()) // Solo avanzar si no hay opciones
        {
            AdvanceDialog();
        }
    }

    /// Método para cargar el CSV y llenar el diccionario de diálogos
    void LoadDialogFromCSV()
    {
        dialogDict.Clear();
        string[] lines = csvFile.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');
            if (values.Length < 14 || !int.TryParse(values[0], out int id)) continue;

            //Asignar cada columna del CSV a variables de la clase DialogEntry
            dialogDict[id] = new DialogEntry
            {
                ID = id,
                Name = values[1],
                Text = values[2].Replace("\\n", "\n"),
                HasOptions = values[3] == "1",
                OptionTexts = new string[3] { values[4], values[6], values[8] },
                OptionNextIDs = new int[3]
                {
                    int.TryParse(values[5], out int o1) ? o1 : -1,
                    int.TryParse(values[7], out int o2) ? o2 : -1,
                    int.TryParse(values[9], out int o3) ? o3 : -1
                },
                OptionWithRequirementText = values[10],
                OptionWithRequirementID = int.TryParse(values[11], out int o4) ? o4 : -1,
                NextLineID = int.TryParse(values[12], out int next) ? next : -1,
                RequiredID = int.TryParse(values[13], out int req) ? req : -1
            };
        }
    }

    public void StartDialog(int start, int end)
    {
        if (isDialogActive) return;

        startID = start;
        endID = end;
        currentID = startID;
        isDialogActive = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialogPanel.SetActive(true);
        ShowDialogue(currentID);
    }

    public void AdvanceDialog()
    {
        if (!isDialogActive || isTyping || AnyOptionActive()) return;

        //Si el diálogo no ha terminado, avanzar al siguiente
        if (currentEntry.NextLineID != -1)
        {
            ShowDialogue(currentEntry.NextLineID);
        }
        //Si el dialogo no tiene más lineas, se muestran todas las opciones de dialogo
        else if (lastOptionsEntry != null)
        {
            //Debug.Log("Volviendo a mostrar opciones anteriores al finalizar diálogo");
            ShowOptions(lastOptionsEntry);
        }
        else
        {
            CloseDialog();
        }
    }

    void ShowDialogue(int id)
    {
        if (!dialogDict.TryGetValue(id, out currentEntry))
        {
            //Debug.LogError($"Diálogo con ID {id} no encontrado");
            return;
        }

        currentID = id;
        textCompleted = false;

        // Registrar que hemos visto este diálogo
        if (!seenDialogIDs.Contains(id))
        {
            //Debug.Log($"Nuevo diálogo visto - ID: {id}, Texto: {currentEntry.Text}");
            seenDialogIDs.Add(id);
        }

        // Si este diálogo tiene un RequiredID, desbloquearlo
        if (currentEntry.RequiredID != -1 && !unlockedDialogIDs.Contains(currentEntry.RequiredID))
        {
            //Debug.Log($"Desbloqueando diálogo requerido - ID: {currentEntry.RequiredID}");
            unlockedDialogIDs.Add(currentEntry.RequiredID);
        }

        //Cambiar el texto del diálogo y el nombre del personaje
        nameText.text = currentEntry.Name;
        continueIndicator.gameObject.SetActive(false);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (indicatorCoroutine != null)
            StopCoroutine(indicatorCoroutine);

        // Iniciar escritura de texto
        typingCoroutine = StartCoroutine(TypeText(currentEntry.Text, () =>
        {
            textCompleted = true;
            //Si el diálogo tiene opciones, mostrarlas
            if (currentEntry.HasOptions)
            {
                //Debug.Log($"Mostrando opciones para diálogo ID: {id}");
                lastOptionsEntry = currentEntry;
                ShowOptions(currentEntry);
            }
            else if (currentEntry.NextLineID == -1 && lastOptionsEntry != null)
            {
                //Debug.Log("No hay siguiente línea, volviendo a opciones anteriores");
                ShowOptions(lastOptionsEntry);
            }
            else
            {
                indicatorCoroutine = StartCoroutine(AnimateContinueIndicator());
            }
        }));
    }

    // Corrutina para escribir el texto con efecto de máquina de escribir
    IEnumerator TypeText(string text, System.Action onComplete)
    {
        isTyping = true;
        dialogueText.text = "";
        float typingSpeed = 0.015f;
        float timer = 0f;
        int visibleChars = 0;

        while (visibleChars < text.Length && isDialogActive)
        {
            timer += Time.deltaTime;
            if (timer >= typingSpeed)
            {
                timer = 0f;
                visibleChars++;
                dialogueText.text = text.Substring(0, visibleChars);
            }
            yield return null;
        }

        dialogueText.text = text;
        isTyping = false;
        onComplete?.Invoke();
    }

    // Corrutina para animar el indicador de continuar
    IEnumerator AnimateContinueIndicator()
    {
        if (continueIndicator == null || currentEntry.HasOptions) yield break;

        continueIndicator.gameObject.SetActive(true);
        float pulseDuration = 0.5f;
        float timer = 0f;
        bool increasing = true;
        float minAlpha = 0.3f;
        float maxAlpha = 1f;

        //Mientras el texto esté completo y no haya opciones activas, animar el indicador
        //Se hace un efecto de palpitación y se oculta al finalizar
        while (textCompleted && !isTyping && isDialogActive && !AnyOptionActive())
        {
            timer += Time.deltaTime * (increasing ? 1 : -1);
            timer = Mathf.Clamp(timer, 0, pulseDuration);

            if (timer == 0 || timer == pulseDuration)
                increasing = !increasing;

            continueIndicator.color = new Color(1, 1, 1, Mathf.Lerp(minAlpha, maxAlpha, timer / pulseDuration));
            yield return null;
        }

        continueIndicator.gameObject.SetActive(false);
    }

    // Método para completar el texto actual si se pulsa un input antes de que la corrutina haya acabado
    void CompleteCurrentText()
    {
        if (!isTyping || typingCoroutine == null) return;

        StopCoroutine(typingCoroutine);
        dialogueText.text = currentEntry.Text;
        isTyping = false;
        textCompleted = true;

        if (currentEntry.HasOptions)
        {
            lastOptionsEntry = currentEntry;
            ShowOptions(currentEntry);
        }
        else if (currentEntry.NextLineID == -1 && lastOptionsEntry != null)
        {
            ShowOptions(lastOptionsEntry);
        }
        else
        {
            indicatorCoroutine = StartCoroutine(AnimateContinueIndicator());
        }
    }

    // Método para mostrar las opciones de diálogo
    void ShowOptions(DialogEntry entry)
    {
        HideAllOptions();

        int buttonIndex = 0;
        bool hasSpecialWeapon = weaponSlot.CheckForWeapon(specialWeapon);

        // Mostrar opciones normales
        for (int i = 0; i < 3 && buttonIndex < optionButtons.Length; i++)
        {
            if (!string.IsNullOrEmpty(entry.OptionTexts[i]))
            {
                // Condición especial para la columna de "¿Sabes dónde está la anciana?"
                if (i == 0 && entry.OptionTexts[i].Contains("anciana") && !hasSpecialWeapon)
                {
                    continue; // Saltar esta opción si no tiene el arma
                }

                if (entry.OptionNextIDs[i] != -1)
                {
                    SetupOption(buttonIndex++, entry.OptionTexts[i], entry.OptionNextIDs[i]);
                }
            }
        }

        // Mostrar opción condicional solo si el diálogo requerido ha sido visto
        if (buttonIndex < optionButtons.Length &&
            !string.IsNullOrEmpty(entry.OptionWithRequirementText) &&
            entry.OptionWithRequirementID != -1 &&
            seenDialogIDs.Contains(entry.RequiredID))
        {
            SetupOption(buttonIndex++, entry.OptionWithRequirementText, entry.OptionWithRequirementID);
        }

        // Mostrar botón "Adiós" solo si hay al menos una opción mostrada
        if (buttonIndex > 0 && buttonIndex < optionButtons.Length)
        {
            SetupOption(buttonIndex, "Adiós.", -1);
        }
    }

    // Método para configurar cada opción de diálogo en el botón correspondiente
    void SetupOption(int index, string text, int nextID)
    {
        if (index < 0 || index >= optionButtons.Length || optionButtons[index] == null)
        {
            //Debug.LogError($"Índice de opción inválido: {index}");
            return;
        }

        optionButtons[index].gameObject.SetActive(true);
        optionTexts[index].text = text;
        optionButtons[index].onClick.RemoveAllListeners();
        optionButtons[index].onClick.AddListener(() => OnOptionSelected(nextID));

        //Debug.Log($"Configurada opción {index}: {text} -> {nextID}");
    }

    // Método para manejar la selección de una opción
    void OnOptionSelected(int nextID)
    {
        if (!isDialogActive) return;

        //Debug.Log($"Opción seleccionada, siguiente ID: {nextID}");

        //Cuando se ha seleccionado una opción, se ocultan todas las opciones
        HideAllOptions();

        if (nextID == -1)
        {
            //Debug.Log("Cerrando diálogo");
            CloseDialog();
            return;
        }

        if (!dialogDict.TryGetValue(nextID, out DialogEntry nextEntry))
        {
            //Debug.LogError($"Siguiente diálogo con ID {nextID} no encontrado");
            return;
        }

        //Al terminar de leer el texto, se muestra el siguiente diálogo
        ShowDialogue(nextID);
        //Si el siguiente diálogo no tiene opciones, se inicia la animación del indicador de continuar
        if (!nextEntry.HasOptions)
        {
            if (indicatorCoroutine != null)
                StopCoroutine(indicatorCoroutine);

            indicatorCoroutine = StartCoroutine(AnimateContinueIndicator());
        }
    }

    void HideAllOptions()
    {
        foreach (var btn in optionButtons)
        {
            if (btn != null)
            {
                btn.gameObject.SetActive(false);
                btn.onClick.RemoveAllListeners();
            }
        }
    }

    bool AnyOptionActive()
    {
        foreach (var btn in optionButtons)
        {
            if (btn.gameObject.activeSelf) return true;
        }
        return false;
    }

    public void CloseDialog()
    {
        if (!isDialogActive) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        //Debug.Log("Cerrando diálogo");

        HideAllOptions();
        dialogueText.text = "";
        nameText.text = "";
        dialogPanel.SetActive(false);
        isDialogActive = false;

        EventsManager.TriggerNormalEvent("ResetCameraDialogue");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ForceCloseDialog()
    {
        CloseDialog();
    }

    //Método para realizar una accion si se ha desbloqueado una linea de dialogo especifica
    public bool DialogIDRead(int id)
    {
        return seenDialogIDs.Contains(id);
    }
}