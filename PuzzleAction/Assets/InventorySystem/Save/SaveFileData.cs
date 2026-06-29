using System;
using System.Collections.Generic;

[Serializable]
public class SaveFileData
{
    public List<SaveItemData> activeItems = new();
    public List<SaveItemData> passiveItems = new();
}
