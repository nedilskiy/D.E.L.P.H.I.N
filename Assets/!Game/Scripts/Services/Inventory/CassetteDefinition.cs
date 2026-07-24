using UnityEngine;

namespace Delphin.Services
{
    [CreateAssetMenu(fileName = "New Cassette", menuName = "Delphin/Cassette Definition")]
    public class CassetteDefinition : ItemDefinition
    {
        public bool HasRecordedData { get; private set; }
        public Vector3 RecordedCoordinates { get; private set; }

        public void Record(Vector3 coordinates)
        {
            RecordedCoordinates = coordinates;
            HasRecordedData = true;
        }
    }
}
