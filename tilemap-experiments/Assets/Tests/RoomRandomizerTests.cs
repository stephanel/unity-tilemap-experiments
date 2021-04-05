using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoomRandomizerTests
{
    [Test]
    [Repeat(1000)]
    public void ShouldDefineARandomRoomType()
    {
        int maxRoomType = Enum.GetValues(typeof(RoomType)).Cast<int>().Max();
        var floorIndex = 1;
        int roomType = (int)ExecuteGetRoomType(floorIndex);
        Assert.That(roomType, Is.GreaterThanOrEqualTo(1));
        Assert.That(roomType, Is.LessThanOrEqualTo(maxRoomType));
    }

    [Test]
    public void ShouldThrowInvalidFloorIndexWhenItIsLowerThanFirstFloorIndex()
    {
        var floorIndex = 0;
        Assert.That(() => { ExecuteGetRoomType(floorIndex); },
            Throws.Exception.TypeOf<InvalidFloorIndex>());
    }

    [Test]
    public void ShouldDefineOnlyBasementsAtFloor1()
    {
        var floorIndex = 1;
        var roomType = ExecuteGetRoomType(floorIndex);
        Assert.That(roomType, Is.EqualTo(RoomType.Basement));
    }

    [Test]
    [Repeat(1000)]
    public void ShouldNotDefineBasementsAtFloorsHigherThanFloor1(
        [NUnit.Framework.Range(2, RoomRandomizer.FloorCounts.Six)] int floorIndex)
    {
        var roomType = ExecuteGetRoomType(floorIndex);
        Assert.That(roomType, Is.Not.EqualTo(RoomType.Basement));
    }

    private RoomType ExecuteGetRoomType(int floorIndex)
    {
        return new RoomRandomizer(0).GetRandomRoomType(floorIndex);
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
        var arrayOfWidth = new RoomRandomizer(0).GenerateArrayOfPossibleWidthForRooms();

        CheckThatPossibleWidthForRoomsCount(arrayOfWidth.Length);

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

    void CheckThatPossibleWidthForRoomsCount(int possibleWidthCount)
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
