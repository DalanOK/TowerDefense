using UnityEngine;

public enum projectileType
{
    bullette1, bullette2
};

public class Projectile : MonoBehaviour
{
    [SerializeField]
    int attackDamage;

    [SerializeField]
    projectileType pType;

    public int AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }
    public projectileType ProjectileType
    {
        get
        {
            return pType;
        }
    }
}
