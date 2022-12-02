
public class HealthSystem
{
    private float health;
    private float healthMax;

    public HealthSystem(float healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public float GetHealth()
    {
        return health;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;
    }

    public void Heal(float healthAmount)
    {
        health += healthAmount;
        if (health > healthMax) health = healthMax;
    }
}
