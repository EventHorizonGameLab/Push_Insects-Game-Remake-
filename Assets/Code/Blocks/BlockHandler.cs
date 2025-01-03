using UnityEditor;

using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    [SerializeField] Block.TypeOfBlock TypeOfBlock;

   

#if UNITY_EDITOR
    private void OnValidate()
    {
        MenageComponent<Block_Movable_X>(TypeOfBlock == Block.TypeOfBlock.MoveOnX);
        MenageComponent<Block_Movable_Z>(TypeOfBlock == Block.TypeOfBlock.MoveOnZ);
        MenageComponent<Block_Movable_XZ>(TypeOfBlock == Block.TypeOfBlock.MoveOnBothAxis);
        MenageComponent<Block_FixedPosition>(TypeOfBlock == Block.TypeOfBlock.Fixed);
        MenageComponent<Block_Sliding_X>(TypeOfBlock == Block.TypeOfBlock.SlideOnX);
        MenageComponent<Block_Sliding_Z>(TypeOfBlock == Block.TypeOfBlock.SlideOnZ);
    }



    private void MenageComponent<T>(bool shouldHaveComponent) where T : Component
    {
        EditorApplication.delayCall += () =>
        {
            if (this == null) return;

            T component = GetComponent<T>();

            if (shouldHaveComponent)
            {
                if (component == null)
                {
                    gameObject.AddComponent<T>();
                }
            }
            else
            {
                if (component != null)
                {
                    DestroyImmediate(component);
                }
            }
        };
    }

#endif
}

