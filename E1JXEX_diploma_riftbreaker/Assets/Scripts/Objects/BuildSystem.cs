using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class eBuildSystem : MonoBehaviour
{
    public GameObject wallPrefab;
    public PlayerAim playerAim;

    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // O1 miatt 

    private void Start()
    {
        InputManager.instance.input.Player.Attack.performed += TryBuildWall;
    }

    private void OnDestroy()
    {
        if (InputManager.instance != null)
        {
            InputManager.instance.input.Player.Attack.performed -= TryBuildWall;
        }
    }

    private void TryBuildWall(InputAction.CallbackContext context)
    {
        if (!PlayerManager.Instance.inBuildState || playerAim.aimTarget == null) return;

        Vector3 targetPos = playerAim.aimTarget.position;
        Vector3 snapPos = new Vector3(Mathf.Round(targetPos.x), 0.5f, Mathf.Round(targetPos.z));

        if (!occupiedPositions.Contains(snapPos))
        {
            Instantiate(wallPrefab, snapPos, Quaternion.identity);
            occupiedPositions.Add(snapPos);
        }
    }
}