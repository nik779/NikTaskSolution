namespace DefaultNamespace;

public class Player
{
  private const Epsilon = 0.1f;
  private bool isMoving;
  private Vector2 currentPosition;
  private Vector2? lastKnownEnemyPosition;
  private List<Vector2> activeWalkPath = null;
  private Player currentEnemy;
  
  public void SetEnemy(Player enemy)
  {
    currentEnemy = enemy;
    lastKnownEnemyPosition = enemy?.currentPosition;

    if (currentEnemy != null)
    {
      UpdatePathToEnemy();
    }
    else
    {
      isMoving = false;
      activeWalkPath = null;
    }
    
    UpdatePathToEnemy();
  }
  
  public void Update()
  {
    if (currentEnemy == null)
    {
      isMoving = false;
      return;
    }
    
    if (IsPlayerInInteractionRangeWith(currentEnemy))
    {
      isMoving = false;
      currentEnemy = null;
      return;
    }
    
    if (IsLastKnownEnemyPositionChanged())
    {
      lastKnownEnemyPosition = currentEnemy.currentPosition;
      UpdatePathToEnemy();
    }
  }

  private bool IsLastKnownEnemyPositionChanged()
  {
    return lastKnownEnemyPosition.HasValue && Vector2.Distance(lastKnownEnemyPosition.Value, currentEnemy.currentPosition) > Epsilon;
  }
  
  private bool IsPlayerInInteractionRangeWith(Player enemy)
  {
    return Vector2.Distance(currentPosition, enemy.currentPosition) < Epsilon;
  }
  
  private void UpdatePathToEnemy()
  {
    activeWalkPath = <много строк кода по созданию пути>;

    isMoving = activeWalkPath != null;
    if (!isMoving)
    {
      currentEnemy = null;
    }
  }
}
