using UnityEngine;

[CreateAssetMenu(fileName = "GridSettingSO", menuName = "ScriptableObjects/GridSettingSO", order = 0)]
public class GridSettingSO : ScriptableObject
{

    public int gridSize = 9;
    public float cellSize = 2f;
    public float cellSpacing = 0.25f;

}
