public class Player
{ 
    private const int NewPayerHealth = 100;
    private const int BaseDamage = 10;

    public int Health {
        get; private set;
    }

    public Player() {
        Health = NewPlayerHealth;
    }

    public void Hit() {
        Health -= BaseDamage;
    }
}

class Program
{
    protected static Player player;

    public static void Main(string[] args)
    {
        // Создаем нового игрока.
        player = new Player();
        // Ударяем игрока.
        player.Hit();
    }
}
