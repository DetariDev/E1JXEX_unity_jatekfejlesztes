using System.Collections.Generic;
using UnityEngine;
public enum UpgradePlace
{
    Head,
    Body,
    Arm,
    Leg
}
[CreateAssetMenu(fileName = "NewMechUpgrade", menuName = "Equipment/MechUpgrade")]
public class MechUpgrade : ScriptableObject
{
    public UpgradePlace place;
    public List<UpgradeModifier> modifiers;
}