using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAction;

public class ActionManager : FMonoModule
{ 
    static LinkedList<FAction.FAction> actions = new LinkedList<FAction.FAction>();
    public static ActionManager instance;
    public override IEnumerator OnPrepare()
    {
        ActionFactory.Preload();
        instance = this;
        yield return null;
    }

    public static void AppendAction(FAction.FAction action)
    {
        actions.AddLast(action);
    }
  
    public static void RemoveAction(FAction.FAction action)
    {
        actions.Remove(action);
    }

    public override void OnRefresh()
    {
        if (actions.Count == 0)
        {
            return;
        }

        var node = actions.First;
        do
        {
            var action = node.Value;
            // 暂停的action不tick
            if (!action.IsPause)
            {
                action.Tick();
            }

            // 完成的action就移除掉,需要重新执行run才行运行
            if (action.IsFinish)
            {
                action.Stop();
            }
        
            node = node.Next;
        } while (node != null && node != actions.First);


#if UNITY_EDITOR
        Test();
#endif
    }

#if UNITY_EDITOR

    FAction.FAction action;
    void Test()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            var callLog = Action.Call(() => Debug.Log("测试函数执行1"));
            var delay = Action.Delay(1);
            var deltaFrame = Action.Delay(3);
            var callLog2 = Action.Call(() => Debug.Log("测试函数执行2"));
            //var seq = Action.Sequence(delay, callLog, deltaFrame, callLog2);

            //seq.Run();

            var repeat = Action.Loop(Action.Sequence(delay, callLog, deltaFrame, callLog2));
            repeat.Run();


            action = repeat;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            action.Run();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            action.Destroy();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!action.IsPause)
            {
                action.Pause();
            }
            else
            {
                action.Resume();
            }

        }
    }
#endif
}
