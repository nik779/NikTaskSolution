namespace Cleanup
{
  public class TestableTargetFinder : TargetFinder
  {
    public ITarget LockedCandidateTarget
    {
      get => _lockedCandidateTarget;
      set => _lockedCandidateTarget = value;
    }

    public ITarget LockedTarget
    {
      get => _lockedTarget;
      set => _lockedTarget = value;
    }
    
    public ITarget Target
    {
      get => _target;
      set => _target = value;
    }
    
    public ITarget PreviousTarget
    {
      get => _previousTarget;
      set => _previousTarget = value;
    }
    
    public ITarget ActiveTarget
    {
      get => _activeTarget;
      set => _activeTarget = value;
    }
    
    public ITargetInRangeContainer TargetInRangeContainer
    {
      get => _targetInRangeContainer;
      set => _targetInRangeContainer = value;
    }
    
    public ITime TimeValue
    {
      get => Time;
      set => Time = value;
    }
    
    public ITargetableEntity TargetableEntityValue
    {
      get => TargetableEntity;
      set => TargetableEntity = value;
    }
    
    public bool IsTargetSet
    {
      get => _isTargetSet;
      set => _isTargetSet = value;
    }

    public double PreviousTargetSetTime
    {
      get => _previousTargetSetTime;
      set => _previousTargetSetTime = value;
    }

    public IFrame FrameValue => Frame;
  }
}