
using System.Threading;

namespace Cleanup
{
    // mock frame
    public interface IFrame
    {
        
    }
    
    // mock time 
    public interface ITime
    {
        double time { get; set; }
    }
    
    // mock TargetableEntity
    public interface ITargetableEntity
    {
        ITarget Selected { get; set; }
    }
    
    // mock TargetEntity
    public interface ITarget 
    {
        bool CanBeTarget { get; set; }
    }
    
    public interface ITargetInRangeContainer
    { 
        ITarget GetTarget();
    }

    public class TargetFinder
    {
        private const double TargetChangeTime = 1;

        protected double _previousTargetSetTime;
        protected bool _isTargetSet;
        protected dynamic _lockedCandidateTarget;
        protected dynamic _lockedTarget;
        protected dynamic _target;
        protected dynamic _previousTarget;
        protected dynamic _activeTarget;
        protected dynamic _targetInRangeContainer;
        
        protected IFrame Frame;
        protected ITargetableEntity TargetableEntity;
        protected ITime Time;

        private bool CanCleanLockedCandidate => _lockedCandidateTarget != null && !_lockedCandidateTarget.CanBeTarget;
        private bool CanCleanLocked => _lockedTarget != null && !_lockedTarget.CanBeTarget;
        public void CleanupTest(IFrame frame)
        {
            try
            {
                TryCleanLockedTargetAndLockedCandidate();
                TrySetActiveTargetFromQuantum(frame);

                _isTargetSet = IsCurrentTargetStillExistAndStillActual();
                _previousTarget = _target;

                if (!_isTargetSet)
                {
                    if (TrySetTargetFrom(_lockedTarget)) 
                        return;
                    
                    if (TrySetTargetFrom(_activeTarget))
                        return;
                    
                    _target = _targetInRangeContainer.GetTarget();
                    if (_target != null)
                    {
                        _isTargetSet = true;
                    }
                }
            }
            finally
            {
                if (_isTargetSet)
                {
                    if (_previousTarget != _target)
                    {
                        _previousTargetSetTime = Time.time;
                    }
                }
                else
                {
                    _target = null;
                }
                TargetableEntity.Selected = _target;
            }
        }

        private bool TrySetTargetFrom(dynamic targetToSet)
        {
            if (targetToSet != null && targetToSet.CanBeTarget)
            {
                _target = targetToSet;
                _isTargetSet = true;
            }

            return _isTargetSet;
        }

        private bool IsCurrentTargetStillExistAndStillActual()
        {
            return _target != null && _target.CanBeTarget && Time.time - _previousTargetSetTime < TargetChangeTime;
        }
        
        private void TryCleanLockedTargetAndLockedCandidate()
        {
            if (CanCleanLockedCandidate) 
                _lockedCandidateTarget = null;

            if (CanCleanLocked)
                _lockedTarget = null;
        }

        private void TrySetActiveTargetFromQuantum(IFrame frame)
        {
            Frame = frame;
        }
    }
    
    internal class Program
    {
        
        // MORE CLASS CODE
        public static void Main(string[] args)
        {
            // 
        }
    }
    
}




