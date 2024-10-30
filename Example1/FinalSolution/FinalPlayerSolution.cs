
namespace DefaultNamespace
{
  public class Player
  {
    public int Health { get; private set; }
    private readonly int _baseDamage;
    private bool _isDead => Health <= 0;
    public Player(NewPlayerSettings settings)
    {
      Health = settings.BaseHealth;
      _baseDamage = settings.BaseDamage;
    }

    public Player(PlayerSave save)
    {
      Health = save.Health;
    }

    public Player Restore(PlayerSave save)
    {
      Health = save.Health;
      return this;
    }

    public virtual void Hit(params Func<int, int>[] damageModifiers)
    {
      if (isDead)
        return;
      
      int modifiedDamage = Damage;

      if (damageModifiers != null && damageModifiers.Length > 0)
      {
        foreach (var modifier in damageModifiers)
        {
          modifiedDamage = modifier(modifiedDamage);
        }
      }

      Health -= modifiedDamage;
    }
  }

  [Serializable]
  public class PlayerSave
  {
    public int Health { get; }
  }

  [Serializable]
  public class NewPlayerSettings
  {
    public int BaseHealth { get; }
    public int BaseDamage { get; }
  }

  public static class PlayerExtensions
  {
    public static PlayerSave ToSaveData(this Player player)
    {
      return new PlayerSave
      {
        Health = player.Health
      };
    }
  }

  class Program
  {
    private const string NewPlayerPath = "PlayerSetting.json";
    private const string PlayerSavePath = "PlayerSave.json";

    protected static Player player;

    public static void Main(string[] args)
    {
      // Загружаем настройки нового игрока.
      var playerSettings = Serializer.LoadFromFile<NewPlayerSettings>(SettingsPath);
      // Создаем нового игрока.
      player = new Player(playerSettings);
      // Ударяем игрока без модификаторов.
      player.Hit();

      //...
      
      // Ударяем игрока с модификатором.
      player.Hit(
        damage => damage * 2,
        damage => damage + 3,
        damage => damage / 5);
      
      //...
      
      // Сохраняем игрока.
      Serializer.SaveToFile<PlayerSave>(PlayerSavePath, player.ToSaveData());

      // Загружаем игрока.
      var playerSave = Serializer.LoadFromFile<PlayerSave>(PlayerSavePath);
      player = player != null ? player.Restore(playerSave) : new Player(playerSave);

    }
  }
}