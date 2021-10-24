using UnityEngine;
using UnityEngine.InputSystem;

public class LoadBindings : MonoBehaviour
{
    #region Variables
    [SerializeField] private InputActionAsset actionsAsset;
    #endregion

    #region Methods
    void Awake()
    {
        foreach (InputActionMap item in actionsAsset.actionMaps)
        {
            // Debug.Log(item.ToJson());
            // actionMaps.Add(item);
            foreach (InputAction action in item.actions)
            {
                string name = "CO" + action.bindings[0].action;
                if (PlayerPrefs.HasKey(name))
                {
                    InputBinding binding = action.bindings[0];
                    binding.overridePath = PlayerPrefs.GetString(name);
                    action.ApplyBindingOverride(0, binding);
                }
                else
                {
                    PlayerPrefs.SetString(name, action.bindings[0].effectivePath);
                }
            }
        }
    }
    #endregion
}