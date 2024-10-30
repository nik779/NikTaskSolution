
public class Player
{
	public int Health {
		get;
		private set;
	}

	public Player(int health) 
	{
        Health = health;
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
	
	public void Hit(int damage) {
        Health -= damage;
    }
}

[Serializable]
public PlayerSave
{
	public int Health { get; }
}

[Serializable]
public NewPlayerSettings
{
	public int BaseHealth { get; }
	public int BaseDamage { get; }
}
	
public static PlayerExtensions
{
    public static PlayerSave ToSaveData(this Player player)
    {
        return new PlayerSave
        {
	        Health = player.Health
        };
    }
    
    public static 
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
		// Ударяем игрока.
		player.Hit(playerSettings.BaseDamage);
		
		// Сохраняем игрока.
		Serializer.SaveToFile<PlayerSave>(PlayerSavePath, player.ToSaveData());
		
		// Загружаем игрока.
		var playerSave = Serializer.LoadFromFile<PlayerSave>(PlayerSavePath);
		player = player != null ? player.Restore(playerSave) : new Player(playerSave);
		
	}
}
