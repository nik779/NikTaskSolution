namespace DefaultNamespace;

public class ExtPlayer : Player
{
  private const int CriticalHealthChange = 10;
  private Action<bool> innerHealthChanged;

  public event Action<bool> HealthChanged {
    add {
      innerHealthChanged += value;
      // тут передаем false так как это просто инициализация.
      value(false);
    }
    remove {
      innerHealthChanged -= value;
    }
  }
  
  // В таком случае мы можем переопределить метод Hit и вызывать событие HealthChanged.
  public virtual void Hit(params Func<int, int>[] damageModifiers)
  {
    if (isDead)
    {
      // Последни раз оповещаем подписчиков о смерти. Считаем что смерть это критичесикй урон.
      innerHealthChanged.Invoke(true);
      // Если игрок мертв, то стоит подумать о очистке подписчиков.
      innerHealthChanged = null;
      return;
    }
      
    int modifiedDamage = Damage;

    if (damageModifiers != null && damageModifiers.Length > 0)
    {
      foreach (var modifier in damageModifiers)
      {
        modifiedDamage = modifier(modifiedDamage);
      }
    }

    Health -= modifiedDamage;
    var isCriticalHealthChange = modifiedDamage >= CriticalHealthChange;
    innerHealthChanged.Invoke(isCriticalHealthChange);
  }
}

// В таком случае код в Program будет выглядеть так:
class ExtProgram : Program
{
  // Виджет, отображающий игроку здоровье.
  private static TextView healthView = new TextView();

  public static void ExtMain(string[] args)
  {
    // Вызов кода по созданию игрока.
    Main(args);

    player.HealthChanged += OnPlayerHealthChanged;

    // Ударяем игрока.
    HitPlayer();
  }

  private static void OnPlayerHealthChanged(bool isCritical)
  {
    healthView.Text = player.Health.ToString();
    healthView.Color = isCritical ? Color.Red : Color.White;
  }
}