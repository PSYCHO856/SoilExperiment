using System.Globalization;
using System.Collections.Generic;

public class SpaceDynData
{
   public string nextSearchStart;
    public int pageSize;
    public int totalSize;
    public List<SpaceResults> results = null;
}
public class SpaceResults
{
    public MediaData[] attaches;
    public string title;
    public string content;
    public string circleId;//当做动态UID

}

public class MediaData
{
    public string address;
    public string type;

    public string musicName;//获取歌曲-名字

}