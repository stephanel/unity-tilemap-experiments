using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoomRandomizerTests
{
    [Test]
    public void FirstFloorIndexShouldBeEqualTo1()
    {
        Assert.That(RoomRandomizer.FirstFloorIndex, Is.EqualTo(1));
    }

    [Test]
    public void MinWidthShouldBeEqualTo20()
    {
        Assert.That(RoomRandomizer.MinWidth, Is.EqualTo(20));
    }

    [Test]
    public void MinWidthShouldBeEqualTo55()
    {
        Assert.That(RoomRandomizer.MaxWidth, Is.EqualTo(55));
    }

    [Test]
    public void WidthFractionalShouldBeEqualTo5()
    {
        Assert.That(RoomRandomizer.WidthFractional, Is.EqualTo(5));
    }

    [Test]
    [Repeat(5000)]
    public void ShouldNeverThrowInvalidFloorIndex()
    {
        Assert.That(() => { new RoomRandomizer(0).Randomize(); },
            Throws.Nothing);
    }

    [Test]
    [Repeat(1000)]
    public void ShouldDefineARandomRoomType()
    {
        int maxRoomType = Enum.GetValues(typeof(RoomType)).Cast<int>().Max();
        var floorIndex = 4;
        var lastFloorIndex = 6;
        int roomType = (int)ExecuteGetRoomType(floorIndex, lastFloorIndex);
        Assert.That(roomType, Is.GreaterThanOrEqualTo(1));
        Assert.That(roomType, Is.LessThanOrEqualTo(maxRoomType));
    }

    [Test]
    public void ShouldDefineOnlyBasementsAtFloor1()
    {
        var floorIndex = 1;
        var lastFloorIndex = 2;
        var roomType = ExecuteGetRoomType(floorIndex, lastFloorIndex);
        Assert.That(roomType, Is.EqualTo(RoomType.Basement));
    }

    [Test]
    public void ShouldDefineOnlyAtticsAtLastFloor()
    {
        var floorIndex = 1;
        var lastFloorIndex = 1;
        var roomType = ExecuteGetRoomType(floorIndex, lastFloorIndex);
        Assert.That(roomType, Is.EqualTo(RoomType.Attic));
    }

    [Test]
    [Repeat(1000)]
    public void ShouldNeverDefineBasementsAtFloorsHigherThanFloor1(
        [NUnit.Framework.Range(2, RoomRandomizer.FloorCounts.Six)] int floorIndex)
    {
        var lastFloorIndex = RoomRandomizer.FloorCounts.Six;
        var roomType = ExecuteGetRoomType(floorIndex, lastFloorIndex);
        Assert.That(roomType, Is.Not.EqualTo(RoomType.Basement));
    }

    [Test]
    [Repeat(1000)]
    public void ShouldNeverDefineAtticsAtFloorsLowerThanLastFloor(
        [NUnit.Framework.Range(1, 5)] int floorIndex)
    {
        var lastFloorIndex = RoomRandomizer.FloorCounts.Six;
        var roomType = ExecuteGetRoomType(floorIndex, lastFloorIndex);
        Assert.That(roomType, Is.Not.EqualTo(RoomType.Attic));
    }

    private RoomType ExecuteGetRoomType(int floorIndex, int lastFloorIndex)
    {
        return FloorFactory.Create(floorIndex, lastFloorIndex).GetRandomRoomType();
    }

    [Test]
    public void ShouldThrowRoomsTotalWidthSettingNotValide()
    {
        Assert.That(() => { new RoomRandomizer(1); },
            Throws.Exception.TypeOf<RoomsTotalWidthSettingNotValide>());
    }

    [Test]
    public void ShouldGenerateArrayOfValideWidth()
    {
        var arrayOfWidth = new DefaultFloor().GenerateArrayOfPossibleWidthForRooms();

        CheckPossibleWidthForRoomsCount(arrayOfWidth.Length);

        foreach (var width in arrayOfWidth)
        {
            CheckThatWidthIsGreaterThanOrEqualToMinWidth(width);
            CheckThatWidthIsLessThanOrEqualToMaxWidth(width);
            CheckThatWidthIsMultipleOfWidthFractional(width);
        }
    }

    [Test]
    public void ShouldGenerateValideFloorsCount()
    {
        var floors = new RoomRandomizer(0).Randomize();

        CheckThatFloorsCountIsGreaterThanOrEqualToThree(floors.Count);
        CheckThatFloorsCountIsLessThanOrEqualToSix(floors.Count);
    }

    [TestCase(RoomRandomizer.MaxWidth * 2)]
    [TestCase(RoomRandomizer.MaxWidth * 3)]
    [TestCase(RoomRandomizer.MaxWidth * 4)]
    [TestCase(RoomRandomizer.MaxWidth * 5)]
    [TestCase(RoomRandomizer.MaxWidth * 6)]
    [TestCase(RoomRandomizer.MaxWidth * 10)]
    [TestCase(RoomRandomizer.MaxWidth * 20)]
    [TestCase(RoomRandomizer.MaxWidth * 100)]
    public void ShouldGenerateAGridOfValideRoomsWidth(int mapWidth)
    {
        var floors = new RoomRandomizer(mapWidth).Randomize();

        foreach (var floor in floors)
        {
            var totalRoomsWidth = floor.Sum(room => room.Width);

            Assert.That(totalRoomsWidth, Is.EqualTo(mapWidth));

            foreach (var room in floor)
            {
                CheckThatWidthIsGreaterThanOrEqualToMinWidth(room.Width);
                // CheckThatWidthIsLessThanOrEqualToMaxWidth(room.Width);
                CheckThatWidthIsMultipleOfWidthFractional(room.Width);
            }
        }
    }

    void CheckPossibleWidthForRoomsCount(int possibleWidthCount)
    {
        int expectedValuesCount = (RoomRandomizer.MaxWidth - RoomRandomizer.MinWidth + RoomRandomizer.WidthFractional)
            / RoomRandomizer.WidthFractional;
        Assert.That(possibleWidthCount, Is.EqualTo(expectedValuesCount));
    }

    void CheckThatWidthIsGreaterThanOrEqualToMinWidth(int width)
    {
        Assert.That(width, Is.GreaterThanOrEqualTo(RoomRandomizer.MinWidth));
    }

    void CheckThatWidthIsLessThanOrEqualToMaxWidth(int width)
    {
        Assert.That(width, Is.LessThanOrEqualTo(RoomRandomizer.MaxWidth));
    }

    void CheckThatWidthIsMultipleOfWidthFractional(int width)
    {
        Assert.That(width % RoomRandomizer.WidthFractional, Is.EqualTo(0));
    }

    void CheckThatFloorsCountIsGreaterThanOrEqualToThree(int floorsCount)
    {
        Assert.That(floorsCount, Is.GreaterThanOrEqualTo(RoomRandomizer.FloorCounts.Three));
    }

    void CheckThatFloorsCountIsLessThanOrEqualToSix(int floorsCount)
    {
        Assert.That(floorsCount, Is.LessThanOrEqualTo(RoomRandomizer.FloorCounts.Six));
    }

    // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator RoomRandomizerTestsWithEnumeratorPasses()
    // {
    //     // Use yield to skip a frame.
    //     yield return null;
    // }
}
