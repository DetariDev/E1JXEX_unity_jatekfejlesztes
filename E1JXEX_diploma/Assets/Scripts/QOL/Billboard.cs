using UnityEngine;
using UnityEngine.Animations;

public class Billboard : MonoBehaviour
{
    private void Start()
    {
        if (Camera.main == null) return;

        RotationConstraint constraint = gameObject.GetComponent<RotationConstraint>();
        if (constraint == null)
        {
            constraint = gameObject.AddComponent<RotationConstraint>();
        }

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = Camera.main.transform;
        source.weight = 1f;

        constraint.AddSource(source);
        constraint.constraintActive = true;

        Destroy(this);
    }
}