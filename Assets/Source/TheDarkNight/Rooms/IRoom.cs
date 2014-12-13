using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TheDarkNight.Rooms {

    public interface IRoom {
        IEnumerable<Room> GetAdjacentRooms();

        IEnumerable<Vector3> GetEntryPositions();
    }


}