using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : BaseCharacter 
{
	public override AvatarInfo GetAvatarInfo()
    {
        var defaultInfo = base.GetAvatarInfo();
        defaultInfo.AvatarStyle = SpriteHelper.instance.Get("Sprites/UI/action_bar => action_bar_4");
        return defaultInfo;
    }
}