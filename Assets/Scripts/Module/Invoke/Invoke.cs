using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using FAction;

[LuaCallCSharp]
// lua 使用action调用行为的常见类型封装
public class Invoke 
{
    /// <summary>
    /// 延迟多少s调用一次函数
    /// </summary>
    /// <param name="delayTime"></param>
    /// <param name="logic"></param>
    public static void DelayInvoke(float delayTime, System.Action logic) {
        var action =  Action.Sequence(Action.Delay(delayTime), Action.Call(logic));
        action.AutoReuse = true;
        action.Run();
    }

    /// <summary>
    /// 先调用一次函数,并且每隔多少秒再调用下一次,一共调用Count次
    /// </summary>
    /// <param name="count"></param>
    /// <param name="interval"></param>
    /// <param name="logic"></param>
    public static void RepeatInvoke(int count, float interval, System.Action logic)
    {
        var action = Action.Repeat(count, Action.Call(logic),  Action.Delay(interval));
        action.AutoReuse = true;
        action.Run();
    }
}
