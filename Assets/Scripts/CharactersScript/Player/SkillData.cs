using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skill Data", order = 0)]
public class SkillData : ScriptableObject
{
    [SerializeField] int skillId;
    [SerializeField] string skillName;
    [SerializeField] string description;
    [SerializeField] float cooldown;

    public int GetId { get { return skillId; } }
    public string GetName { get { return skillName; } }
    public string GetDescription { get { return description; } }
    public float GetCooldown { get { return cooldown; } }
}
