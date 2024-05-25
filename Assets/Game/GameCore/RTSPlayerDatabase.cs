using Amazon;
using Amazon.DynamoDBv2.DocumentModel;
using ZeroLag.MultiplayerTools.Modules.Database;

public class RTSPlayerDatabase : DynamoDBPlayerDatabase<RTSPlayerData, RTSPlayerCustomData>
{
    public RTSPlayerDatabase(string accessKey, string secretKey, RegionEndpoint region, string tableName) 
        : base(accessKey, secretKey, region, tableName) { }

    public override Document FillDocumentFromPlayerData(long playerId, RTSPlayerData newData)
    {
        var doc = base.FillDocumentFromPlayerData(playerId, newData);
        doc["playerAvatar"] = newData.customData.playerAvatar;
        doc["level"] = newData.customData.level;
        return doc;
    }
    
    public override RTSPlayerData ProcessPlayerDataDocument(Document doc)
    {
        var data = base.ProcessPlayerDataDocument(doc);
        data.customData = new RTSPlayerCustomData()
        {
            playerAvatar = doc["playerAvatar"],
            level = doc["level"].AsInt()
        };
        return data;
    }
}