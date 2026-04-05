using UnityEngine;

[CreateAssetMenu(fileName = "NewDrillHead", menuName = "Tools/new DrillHead")]
public class DrillHead : ScriptableObject
{
    public ResourceType[] mineableResources = new ResourceType[0];
    public float miningRate;
    public int miningAmount; 
    public float range=4f;
}
