using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TheDarkNight.Rooms {

    public interface IRoom {
        IEnumerable<Transform> GetAdjacentRoomsEntries();

    }


}