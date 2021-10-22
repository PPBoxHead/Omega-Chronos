using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;



public class RebindSaveLoad : MonoBehaviour
{
    public enum KeyMaping
    {
        Move,
        Jump,
        ReleaseJump
    }
    [SerializeField] private InputActionAsset actions;
    private InputActionMap inputActions;
    private Keyboard keyboard = Keyboard.current;
    private string actionMap;
    private string actionToRebind;
    //ver de pasar los bindings a un json
    // y leerlos de ahi
    void Start()
    {
        int i = 0;
        // esto ver de usar una variable asi podes elegir que actionmap queres cambiar
        inputActions = actions.FindActionMap("PlayerActions");
        foreach (InputAction item in inputActions.actions)
        {
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

    public void StartRebindKey(string keyToRebind)
    {
        // aca podrias hacer aparecer un panel y mensaje para que quede mas bonito
        actions.Disable();
        Debug.Log("Waiting for keypress");
        StartCoroutine("Co_WaitForKeyPress", keyToRebind);
    }

    IEnumerator Co_WaitForKeyPress(int keyToRebind)
    {
        var rebindOperation = inputActions.actions[keyToRebind].PerformInteractiveRebinding()
.WithControlsExcluding("Mouse")
.OnMatchWaitForAnother(0.1f)
.Start();

        while (!keyboard.anyKey.wasPressedThisFrame)
        {
            yield return null;
        }

        // to avoid memory leak
        rebindOperation.Dispose();
        SaveKey(keyToRebind);
    }

    private void SaveKey(int keyToRebind)
    {
        Debug.Log("Saved key");
        actions.Enable();
        // de momento 0 es el keyboard, cambiar si se agregan mas schemes
        InputBinding binding = inputActions.actions[keyToRebind].bindings[0];
        binding.overridePath = inputActions.actions[keyToRebind].bindings[0].effectivePath;
        inputActions.actions[keyToRebind].ApplyBindingOverride(0, binding);

        // checks if action is jump to modify releasejump too
        if (inputActions.actions[keyToRebind].bindings[0].action == "Jump")
        {
            // modifica release jump
            inputActions.actions[keyToRebind + 1].ApplyBindingOverride(0, binding);
            PlayerPrefs.SetString("CO" + inputActions.actions[keyToRebind + 1].bindings[0].action, binding.overridePath);
        }
        // saves changed action to prefs
        PlayerPrefs.SetString("CO" + inputActions.actions[keyToRebind].bindings[0].action, binding.overridePath);
    }

    public void ChangeActionMap(string newActionMap)
    {
        actionMap = newActionMap;
    }

    public void ChangeActionToRebind(string newAction)
    {

    }
}