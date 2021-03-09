using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoomRandomizerTests
{
    [Test]
    public void RoomRandomizerGenerateBetween2And6Floors()
    {
        var floors = new RoomRandomizer(0).Randomize();

        Assert.That(floors.Count, Is.GreaterThanOrEqualTo(RoomRandomizer.FloorCounts.Two));
        Assert.That(floors.Count, Is.LessThanOrEqualTo(RoomRandomizer.FloorCounts.Six));
    }

    [TestCase(100)]
    [TestCase(200)]
    [TestCase(1000)]
    public void RoomRandomizerGenerateTotalRoomWidthEqualsToGridWidth(int totalWidth)
    {
        var floors = new RoomRandomizer(totalWidth).Randomize();

        // Assert.That(floors, Is.All.Matches<List<Room>>(
        //     floor => floor.Sum(room => room.Width) == totalWidth));

        foreach (var floor in floors)
        {
            var roomWidth = floor.Sum(room => room.Width);

            Assert.That(roomWidth, Is.EqualTo(totalWidth));
        }
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
