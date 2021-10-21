using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class RebindSaveLoad : MonoBehaviour
{
    public InputActionReference triggerAction;
    public InputActionAsset actions;
    private List<InputAction> inputBindings = new List<InputAction>();
    private Keyboard keyboard = Keyboard.current;

    //ver de pasar los bindings a un json
    // y leerlos de ahi
    void Start()
    {
        // Debug.Log(actions.FindActionMap("PlayerActions").actions);
        foreach (var item in actions.FindActionMap("PlayerActions").actions)
        {
            inputBindings.Add(item);
        }


        if (PlayerPrefs.HasKey("OCjumpkey"))
        {
            InputBinding binding = inputBindings[1].bindings[0];
            binding.overridePath = PlayerPrefs.GetString("OCjumpkey");
            inputBindings[1].ApplyBindingOverride(0, binding);

        }
    }

    public void Test()
    {
        actions.Disable();
        var rebindOperation = inputBindings[1].PerformInteractiveRebinding()
            // To avoid accidental input from mouse motion
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .Start();
    }

    public void Test2()
    {
        actions.Enable();
        Debug.Log(inputBindings[1].bindings[0].effectivePath);
    }

    public void Save()
    {
        InputBinding binding = inputBindings[1].bindings[0];
        binding.overridePath = inputBindings[1].bindings[0].effectivePath;
        inputBindings[1].ApplyBindingOverride(0, binding);

        PlayerPrefs.SetString("OCjumpkey", binding.effectivePath);
    }
}
