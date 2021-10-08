using System.Collections.Generic;

namespace Xml_Data
{
    public class diaglogueInfo
    {
        public string name;
        public string messege;
        public string imageName;
        public float imageScale;
    }

    public class dialoguesData
    {
        public List<diaglogueInfo> dialogueInfos;
    }
    public class gridCoordinate
    {
        public int x;
        public int y;
    }
    public class levelUnwalkableArea
    {
        public gridCoordinate[] unwalkableAreas;
    }
}
