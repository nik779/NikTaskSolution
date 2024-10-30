using System;
using Microsoft.CSharp.RuntimeBinder;
using NSubstitute;
using NUnit.Framework;

namespace Cleanup.TestTarget
{
  [TestFixture]
  [TestOf(typeof(TestableTargetFinder))]
  public class TargetFinderTest
  {
    [Test]
    public void IfTargetableEntityValueIsNull_ThenNullReferenceExceptionCaught()
    {
      // Arrange
      var finder = new TestableTargetFinder
      {
        TargetableEntityValue = null,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
      };


      // Assert
      Assert.Catch<NullReferenceException>(() => finder.CleanupTest(Substitute.For<IFrame>()));
    }

    [Test]
    public void IfTargetInRangeContainerIsNull_ThenRuntimeBinderExceptionCaught()
    {
      // Arrange
      var finder = new TestableTargetFinder
      {
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = null,
      };

      // Assert
      Assert.Catch<RuntimeBinderException>(() => finder.CleanupTest(Substitute.For<IFrame>()));
    }

    [Test]
    public void IfLockedTargetIsNull_ThenLockedCandidate_StillNull()
    {
      // Arrange
      var finder = new TestableTargetFinder
      {
        LockedTarget = null,

        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TimeValue = Substitute.For<ITime>(),
      };


      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.LockedCandidateTarget, Is.Null);
    }

    [Test]
    public void IfLockedTargetIsSet_AndCannotBeTarget_ThenLockedCandidate_IsNull()
    {
      // Arrange
      var lockedCandidateTarget = Substitute.For<ITarget>();
      lockedCandidateTarget.CanBeTarget.Returns(false);
      var finder = new TestableTargetFinder
      {
        LockedCandidateTarget = lockedCandidateTarget,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TimeValue = Substitute.For<ITime>(),
      };


      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.LockedCandidateTarget, Is.Null);
    }

    [Test]
    public void IfLockedTargetIsSet_AndCanBeTarget_ThenLockedCandidate_IsNotNull()
    {
      // Arrange
      var lockedCandidateTarget = Substitute.For<ITarget>();
      lockedCandidateTarget.CanBeTarget.Returns(true);
      var finder = new TestableTargetFinder
      {
        LockedCandidateTarget = lockedCandidateTarget,
        TimeValue = Substitute.For<ITime>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
      };


      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.LockedCandidateTarget, Is.Not.Null);
    }

    [Test]
    public void IfLockedTargetIsEmpty_ThenLockedTargetStillEmpty()
    {
      // Arrange
      var finder = new TestableTargetFinder
      {
        LockedTarget = null,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TimeValue = Substitute.For<ITime>(),
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.LockedTarget, Is.Null);
    }


    [Test]
    public void IfLockedTargetIsNotEmpty_And_LockedTargetNotTarget_Then_LockedTargetShouldBeEmpty()
    {
      // Arrange
      var finder = new TestableTargetFinder
      {
        LockedTarget = Substitute.For<ITarget>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TimeValue = Substitute.For<ITime>(),
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.LockedTarget, Is.Null);
    }

    [Test]
    public void IfLockedTargetIsNotEmpty_And_LockedTargetCanBeTarget_Then_LockedTargetShouldNotBeEmpty()
    {
      // Arrange
      var lockedTarget = Substitute.For<ITarget>();
      lockedTarget.CanBeTarget.Returns(true);
      var finder = new TestableTargetFinder
      {
        TimeValue = Substitute.For<ITime>(),
        LockedTarget = lockedTarget,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.LockedTarget, Is.Not.Null);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetNull_And_ActiveTargetNull_And_TargetRangeInContainerReturnNull_ThenTargetSetEqualsToFalse()
    {
      // Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetIsNotNull_AndLocketTargetCanBeTargetFalse_And_ActiveTargetNull_And_TargetRangeInContainerReturnNull_ThenTargetSetEqualsToFalse()
    {
      // Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var lockedTarget = Substitute.For<ITarget>();
      lockedTarget.CanBeTarget.Returns(false);
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = lockedTarget,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
      };


      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetIsNotNull_AndLocketTargetCanBeTargetTrue_And_ActiveTargetNull_And_TargetRangeInContainerReturnNull_ThenTargetSetEqualsToTrue()
    {
      // Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var lockedTarget = Substitute.For<ITarget>();
      lockedTarget.CanBeTarget.Returns(true);
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = lockedTarget,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.True);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetIsNull_And_ActiveTargetIsNotNull_And_ActiveTargetCanBeTargetReturnsFalse_And_TargetRangeInContainerReturnNull_ThenTargetSetEqualsToFalse()
    {
      // Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var activeTarget = Substitute.For<ITarget>();
      activeTarget.CanBeTarget.Returns(false);
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = null,
        ActiveTarget = activeTarget,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetIsNull_And_ActiveTargetIsNotNull_And_ActiveTargetCanBeTargetReturnsTrue_And_TargetRangeInContainerReturnNull_ThenTargetSetEqualsToTrue()
    {
      // Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var activeTarget = Substitute.For<ITarget>();
      activeTarget.CanBeTarget.Returns(true);
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = null,
        ActiveTarget = activeTarget,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.True);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetIsNull_And_ActiveTargetIsNull_And_TargetRangeInContainerReturnSomeTarget_ThenTargetSetEqualsToTrue()
    {
      // Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => Substitute.For<ITarget>());
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.True);
    }

    [Test]
    public void
      IfTargetSet_And_TargetCanBeTargetReturnFalse_LockerTargetIsNull_AndActiveTargetIsNull_And_TimeDeltaLessThenTargetChangeTime_ThenTargetIsNotSet()
    {
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(false);
      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }

    [Test]
    public void
      IfTargetSet_And_TargetCanBeTarget_LockerTargetIsNull_AndActiveTargetIsNull_TargetCanBeTargetReturnsTrue_And_TimeDeltaLessThenTargetChangeTime_ThenTargetIsNotSet()
    {
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(true);
      var time = Substitute.For<ITime>();
      time.time.Returns(5);
      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = time,
        PreviousTargetSetTime = 1,
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }

    [Test]
    public void
      IfTargetSet_And_TargetCanBeTarget_LockerTargetIsNull_AndActiveTargetIsNull_TargetCanBeTargetReturnsTrue_And_TimeDeltaEqualsToTargetChangeTime_ThenTargetIsNotSet()
    {
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(true);
      var time = Substitute.For<ITime>();
      time.time.Returns(1);
      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = time,
        PreviousTargetSetTime = 0,
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }

    [Test]
    public void
      IfTargetSet_And_TargetCanBeTarget_LockerTargetIsNull_AndActiveTargetIsNull_TargetCanBeTargetReturnsTrue_And_TimeDeltaGreaterThenTargetChangeTime_ThenTargetIsNotSet()
    {
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(true);
      var time = Substitute.For<ITime>();
      time.time.Returns(1);
      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = time,
        PreviousTargetSetTime = 0.5,
      };

      // Act
      finder.CleanupTest(Substitute.For<IFrame>());

      // Assert
      Assert.That(finder.IsTargetSet, Is.True);
    }

    [Test]
    public void IfTargetNull_ThenPreviousTargetIsNull()
    {
      //Arrange
      var finder = new TestableTargetFinder
      {
        Target = null,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.PreviousTarget, Is.Null);
    }

    [Test]
    public void IfTargetIsNotNull_AndCanBeTarget_AndDeltaTimeLessThenTargetChangeTime_ThenPreviousTargetEqualsToTarget()
    {
      //Arrange
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(true);
      var time = Substitute.For<ITime>();
      time.time.Returns(0);

      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>(),
        PreviousTargetSetTime = 0
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.PreviousTarget, Is.EqualTo(finder.Target));
    }

    [Test]
    public void IfTargetIsNotNull_AndCanBeTarget_AndDeltaTimeGreaterTargetChangeTime_ThenPreviousTargetEqualsToTarget()
    {
      //Arrange
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(true);
      var time = Substitute.For<ITime>();
      time.time.Returns(5);

      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = time,
        PreviousTargetSetTime = 1
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.PreviousTarget, !Is.EqualTo(finder.Target));
    }

    [Test]
    public void IfTargetIsNotNull_And_CanBeTargetIsFalse_ThenPreviousTargetEqualsToTarget()
    {
      //Arrange
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(false);
      var time = Substitute.For<ITime>();
      time.time.Returns(5);

      var finder = new TestableTargetFinder
      {
        Target = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = time,
        PreviousTargetSetTime = 1
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.PreviousTarget, !Is.EqualTo(finder.Target));
    }

    [Test]
    public void
      IfPreviousTargetHasValue_And_TargetIsNotNull_AndCanBeTarget_AndDeltaTimeLessThenTargetChangeTime_ThenPreviousTargetEqualsToTarget()
    {
      //Arrange
      var target = Substitute.For<ITarget>();
      target.CanBeTarget.Returns(true);
      var time = Substitute.For<ITime>();
      time.time.Returns(0);
      var previousTarget = Substitute.For<ITarget>();

      var finder = new TestableTargetFinder
      {
        Target = target,
        PreviousTarget = target,
        LockedTarget = null,
        ActiveTarget = null,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>(),
        PreviousTargetSetTime = 0
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.PreviousTarget, Is.EqualTo(finder.Target));
    }

    [Test]
    public void IfActiveTargetHasValue_AtEndNotNull()
    {
      //Arrange
      var active = Substitute.For<ITarget>();

      var finder = new TestableTargetFinder
      {
        ActiveTarget = active,
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>(),
        PreviousTargetSetTime = 0
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.ActiveTarget, !Is.Null);
    }

    [Test]
    public void IfTargetInRangeContainerHasValue_AtEndNotNull()
    {
      //Arrange
      var finder = new TestableTargetFinder
      {
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>(),
        PreviousTargetSetTime = 0
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.TargetInRangeContainer, !Is.Null);
    }

    [Test]
    public void IfTimeHasValue_ThenAtEndAreNotNull()
    {
      //Arrange
      var finder = new TestableTargetFinder
      {
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.TimeValue, !Is.Null);
    }

    [Test]
    public void IfTargetableEntityHasValue_ThenAtEndAreNotNull()
    {
      //Arrange
      var finder = new TestableTargetFinder
      {
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.TargetableEntityValue, !Is.Null);
    }

    [Test]
    public void IfFrameAreSet_ThenItShouldBeNotNull()
    {
      //Arrange
      var finder = new TestableTargetFinder
      {
        TargetInRangeContainer = Substitute.For<ITargetInRangeContainer>(),
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.FrameValue, !Is.Null);
    }

    [Test]
    public void
      IfTargetIsNull_And_LockedTargetIsNull_And_ActiveTargetIsNull_And_TargetInRangeContainerDoesntHaveTarget_ThenTargetIsSetReturnFalse()
    {
      //Arrange
      var targetInRangeContainer = Substitute.For<ITargetInRangeContainer>();
      targetInRangeContainer.GetTarget().Returns(x => null);
      var finder = new TestableTargetFinder
      {
        TargetInRangeContainer = targetInRangeContainer,
        TargetableEntityValue = Substitute.For<ITargetableEntity>(),
        TimeValue = Substitute.For<ITime>()
      };

      //Act
      finder.CleanupTest(Substitute.For<IFrame>());

      //Assert
      Assert.That(finder.IsTargetSet, Is.False);
    }
    
  }
}