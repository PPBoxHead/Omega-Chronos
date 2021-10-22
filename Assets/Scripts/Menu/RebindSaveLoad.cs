using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class RebindSaveLoad : MonoBehaviour
{
    #region Actions
    [SerializeField] private InputActionAsset actionsAsset;
    #endregion
    #region ActionsMap
    private List<InputActionMap> actionMaps = new List<InputActionMap>();
    // current actionmap
    private InputActionMap actionMap;
    #endregion
    #region Actions
    private List<string> actionList = new List<string>();
    private int actionToRebind;
    #endregion
    private Keyboard keyboard = Keyboard.current;
    //ver de pasar los bindings a un json
    // y leerlos de ahi
    void Start()
    {
        foreach (InputActionMap item in actionsAsset.actionMaps)
        {
            actionMaps.Add(item);
        }

        foreach (InputActionMap map in actionMaps)
        {
            // revisar esto
            // el rebind para el slowmo no anda tampoco , revisar porque
            foreach (InputAction item in map.actions)
            {
                int i = 0;
                string name = "CO" + item.bindings[0].action;
                if (PlayerPrefs.HasKey(name))
                {
                    // changes controls to previously saved scheme
                    InputBinding binding = item.bindings[0];
                    binding.overridePath = PlayerPrefs.GetString(name);
                    item.ApplyBindingOverride(0, binding);
                }
                else
                {
                    // set initial controls
                    PlayerPrefs.SetString(name, item.bindings[0].effectivePath);
                }
                i++;
            }
        }
    }

    public void StartRebindKey()
    {
        // aca podrias hacer aparecer un panel y mensaje para que quede mas bonito
        actionsAsset.Disable();
        Debug.Log("Waiting for keypress");
        StartCoroutine("Co_WaitForKeyPress");
    }

    IEnumerator Co_WaitForKeyPress()
    {
        var rebindOperation = actionMap.actions[actionToRebind].PerformInteractiveRebinding()
.WithControlsExcluding("Mouse")
.OnMatchWaitForAnother(0.1f)
.Start();

        while (!keyboard.anyKey.wasPressedThisFrame)
        {
            yield return null;
        }

        // to avoid memory leak
        rebindOperation.Dispose();
        SaveKey();
    }

    private void SaveKey()
    {
        Debug.Log("Saved key");
        actionsAsset.Enable();
        // de momento 0 es el keyboard, cambiar si se agregan mas schemes
        InputBinding binding = actionMap.actions[actionToRebind].bindings[0];
        binding.overridePath = actionMap.actions[actionToRebind].bindings[0].effectivePath;
        actionMap.actions[actionToRebind].ApplyBindingOverride(0, binding);

        // checks if action is jump to modify releasejump too
        if (actionMap.actions[actionToRebind].bindings[0].action == "Jump")
        {
            // modifica release jump
            actionMap.actions[actionToRebind + 1].ApplyBindingOverride(0, binding);
            PlayerPrefs.SetString("CO" + actionMap.actions[actionToRebind + 1].bindings[0].action, binding.overridePath);
        }
        // saves changed action to prefs
        PlayerPrefs.SetString("CO" + actionMap.actions[actionToRebind].bindings[0].action, binding.overridePath);
    }

    public void ChangeInputActions(string newActionMap)
    {
        actionMap = actionsAsset.FindActionMap(newActionMap);
        foreach (var item in actionMap.actions)
        {
            actionList.Add(item.name);
        }
    }

    public void RebindAction(string newAction)
    {
        actionToRebind = actionList.IndexOf(newAction);
        StartRebindKey();
    }

}