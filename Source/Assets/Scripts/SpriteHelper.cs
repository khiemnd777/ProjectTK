using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHelper
{

    static SpriteHelper _instance;

    public static SpriteHelper instance
    {
        get { return _instance ?? (_instance = new SpriteHelper()); }
    }

    public Dictionary<string, Sprite> icons;

    public SpriteHelper()
    {
        icons = new Dictionary<string, Sprite>();
    }

    public Sprite Get(string name)
    {
        var icon = icons.ContainsKey(name) ? icons[name] : null;
        if (icon != null)
            return icon;
        var prefabCond = name.Split(new[] { "=>" }, System.StringSplitOptions.RemoveEmptyEntries);
        var n1 = prefabCond[0].Trim();
        if (prefabCond.Length == 1)
        {
            icons.Add(name, Resources.Load<Sprite>(n1));
            return Get(name);
        }
        if (prefabCond.Length == 2)
        {
            var n2 = prefabCond[1].Trim();
            var sprites = Resources.LoadAll<Sprite>(n1);
            if(sprites.Count() == 0)
                return null;
            var singleSprite = sprites.First(x => x.name == n2);
            if(singleSprite.IsNull())
                return null;
            icons.Add(name, singleSprite);
            return Get(name);
        }
        return null;
    }

	public int Count(string name)
	{
		return Resources.LoadAll<Sprite>(name).Count();
	}
}
