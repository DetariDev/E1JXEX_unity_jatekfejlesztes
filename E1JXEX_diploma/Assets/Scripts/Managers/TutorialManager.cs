using TMPro;
using UnityEngine;

public enum TutorialStage
{
    None,
    Movement,
    Mining,
    Pickup,
    CarrytoBase,
    UpgradeBase,
    BuildWall,
    KillEnemy,
    UseTurret,
    MakeEnergy,
    UseAutominer,
    KillNest
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public TutorialStage currentStage = TutorialStage.None;
    public TMP_Text tutorialText;


    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void SetTutorialStage(TutorialStage stage)
    {
        currentStage = stage;
        switch (currentStage)
        {
            case TutorialStage.None:
                tutorialText.text = "";
                break;
            case TutorialStage.Movement:
                tutorialText.text = "Use WASD to move around.";
                break;
            case TutorialStage.Mining:
                tutorialText.text = "Mine resources by holding Right mouse button on resource nodes.";
                break;
            case TutorialStage.Pickup:
                tutorialText.text = "Pick up resources by walking over them.";
                break;
            case TutorialStage.CarrytoBase:
                tutorialText.text = "Carry resources back to the base to store them.";
                break;
            case TutorialStage.UpgradeBase:
                tutorialText.text = "Upgrade your base to unlock new features and increase storage capacity.";
                break;
            case TutorialStage.BuildWall:
                tutorialText.text = "Build walls around your base!";
                break;
            case TutorialStage.KillEnemy:
                tutorialText.text = "Defend your base by killing incoming enemies.";
                break;
            case TutorialStage.UseTurret:
                tutorialText.text = "Use turrets to automatically defend your base.";
                break;
            case TutorialStage.MakeEnergy:
                tutorialText.text = "Generate energy by building energy generators and connecting them to your base.";
                break;
            case TutorialStage.UseAutominer:
                tutorialText.text = "Use autominers to automatically mine resources for you.";
                break;
            case TutorialStage.KillNest:
                tutorialText.text = "Destroy enemy nests to stop them from spawning more enemies.";
                break;
        }
    }
    public void NextStage()
    {
        SetTutorialStage(currentStage + 1);
    }


}
