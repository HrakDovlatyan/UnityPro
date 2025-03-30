using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Game/Bullet Configuration")]
public class BulletConfig : ScriptableObject
{
    public Color Color = Color.red;
    public int Damage = 10;
    public float Speed = 10f;
    public float LifeTime = 3f;
}