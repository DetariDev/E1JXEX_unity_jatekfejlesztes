using System.Collections.Generic;
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
    KillNest,
    final
}

public enum KeyHint
{
    WASD,
    Shoot,
    Mine,
    Pickup,
    Drop,
    Build,
    Menu,
    QuitMenu,
    Changeweapon
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public TutorialStage currentStage = TutorialStage.None;
    public TMP_Text tutorialText;
    public TMP_Text keyHintText;

    public Dictionary<KeyHint, bool> keyHints = new Dictionary<KeyHint, bool>();

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void InitializeKeyHints()
    {
        foreach (KeyHint hint in System.Enum.GetValues(typeof(KeyHint)))
        {
            if (hint == KeyHint.WASD || hint == KeyHint.Shoot || hint == KeyHint.Mine || hint == KeyHint.Menu || hint == KeyHint.Build)
            {
                keyHints[hint] = true;
            }
            else
            {
                keyHints[hint] = false;
            }
            
        }

    }
    void Start()
    {
        InitializeKeyHints();
        UpdateKeyHintText();
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
            case TutorialStage.KillNest:
                tutorialText.text = "Destroy enemy nests to stop them from spawning more enemies.";
                break;
            case TutorialStage.final:
                tutorialText.text = "Good luck! Make a strong character and base!";
                break;
        }
    }
    public void NextStage()
    {
        SetTutorialStage(currentStage + 1);
    }

    public void ToggleKeyHint(KeyHint hint, bool show)
    {
        if (!keyHints.ContainsKey(hint) || keyHints[hint] != show)
        {
            keyHints[hint] = show;
            UpdateKeyHintText();
        }
    }

    private void UpdateKeyHintText()
    {
        keyHintText.text = "";

        foreach (var hint in keyHints) 
        {
            if (!hint.Value) continue;
            switch (hint.Key)
            {
                case KeyHint.WASD:
                    keyHintText.text += "Move: WASD\n";
                    break;
                case KeyHint.Shoot:
                    keyHintText.text += "Shoot: Left Mouse Button\n";
                    break;
                case KeyHint.Mine:
                    keyHintText.text += "Mine: Right Mouse Button\n";
                    break;
                case KeyHint.Pickup:
                    keyHintText.text += "Pickup: E\n";
                    break;
                case KeyHint.Drop:
                    keyHintText.text += "Drop: Q\n";
                    break;
                case KeyHint.Build:
                    keyHintText.text += "Build mode: F\n";
                    break;
                case KeyHint.Menu:
                    keyHintText.text += "Open Menu: TAB\n";
                    break;
                case KeyHint.QuitMenu:
                    keyHintText.text += "Close Menu: TAB\n";
                    break;
                case KeyHint.Changeweapon:
                    keyHintText.text += "Change Weapon: Mouse Scroll\n";
                    break;
                default:
                    break;
            }
        }

    }
}
