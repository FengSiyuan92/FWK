using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.UI;

[LuaCallCSharp]
public class LuaBinding : MonoBehaviour
{
    
    public GameObject GetGameObject(string name)
    {
        var child = FindChild(transform, name);
        if (child == null)
        {
            Debug.LogErrorFormat("没有从{0}节点下找到{1}节点,请检查代码和prefab是否一致", gameObject.name, name);
            return null;
        }
        return child.gameObject;
    }


    Transform FindChild(Transform transform, string name)
    {
        var childcount = transform.childCount;
        for (int i = 0; i < childcount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name == name)
            {
                return child;
            }
            var deep = FindChild(child, name);
            if (deep != null)
            {
                return deep;
            }
        }
        return null;
    }


}

[LuaCallCSharp]
// 运行时组件getcomponent有缓存,所以这里直接提供接口返回
public static class GameObjectExtend
{
    public static Image GetImage(this GameObject go)
    {
        return go.GetComponent<Image>();
    }

    public static Button GetButton(this GameObject go)
    {
        return go.GetComponent<Button>();
    }

    public static Text GetText(this GameObject go)
    {
        return go.GetComponent<Text>();
    }

    public static ScrollRect GetScrollRect(this GameObject go)
    {
        return go.GetComponent<ScrollRect>();
    }

    public static RawImage GetRawImage(this GameObject go)
    {
        return go.GetComponent<RawImage>();
    }

    public static Slider GetSlider(this GameObject go)
    {
        return go.GetComponent<Slider>();
    }

    public static Toggle GetToggle(this GameObject go)
    {
        return go.GetComponent<Toggle>();
    }

    public static Scrollbar GetScrollbar(this GameObject go)
    {
        return go.GetComponent<Scrollbar>();
    }

    public static Dropdown GetDropDown(this GameObject go)
    {
        return go.GetComponent<Dropdown>();
    }
}
