using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Game/Bullet Configuration")]
public class BulletConfig : ScriptableObject
{
    public Color color = Color.red;
    public int damage = 10;
    public float speed = 10f;
    public float lifetime = 3f;
}