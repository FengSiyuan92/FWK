---@class CS.FAction.Action : CS.System.Object
CS.FAction.Action = {}

---@param p : CS.FAction.FAction[]
---@return CS.FAction.SequenceAction
function CS.FAction.Action.Sequence(p)
end

---@param p : CS.FAction.FAction[]
---@return CS.FAction.SyncAction
function CS.FAction.Action.Sync(p)
end

---@param p : CS.FAction.FAction[]
---@return CS.FAction.SyncWaitFastAction
function CS.FAction.Action.WaitFast(p)
end

---@param repeatCount : CS.System.Int32
---@param p : CS.FAction.FAction[]
---@return CS.FAction.RepeatAction
function CS.FAction.Action.Repeat(repeatCount, p)
end

---@param p : CS.FAction.FAction[]
---@return CS.FAction.RepeatAction
function CS.FAction.Action.Loop(p)
end

---@param action : CS.System.Action
---@return CS.FAction.CallFuncAction
function CS.FAction.Action.Call(action)
end

---@param delayTime : CS.System.Single
---@return CS.FAction.DelayAction
function CS.FAction.Action.Delay(delayTime)
end

---@param frameCount : CS.System.Int32
---@return CS.FAction.WaitFrameAction
function CS.FAction.Action.WaitFrame(frameCount)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.MoveLocalPos(targetTransform, x, y, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param z : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.MoveLocalPos(targetTransform, x, y, z, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.MovePos(targetTransform, x, y, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param z : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.MovePos(targetTransform, x, y, z, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.ScaleTo(targetTransform, x, y, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param z : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.ScaleTo(targetTransform, x, y, z, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.RotateToLocal(targetTransform, x, y, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param z : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.RotateToLocal(targetTransform, x, y, z, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.RotateTo(targetTransform, x, y, duration)
end

---@param targetTransform : CS.UnityEngine.Transform
---@param x : CS.System.Single
---@param y : CS.System.Single
---@param z : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.TransformToAction
function CS.FAction.Action.RotateTo(targetTransform, x, y, z, duration)
end

---@param graphic : CS.UnityEngine.UI.Graphic
---@param r : CS.System.Single
---@param g : CS.System.Single
---@param b : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.GraphicToAction
function CS.FAction.Action.ColorTo(graphic, r, g, b, duration)
end

---@param graphic : CS.UnityEngine.UI.Graphic
---@param a : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.GraphicToAction
function CS.FAction.Action.AlphaTo(graphic, a, duration)
end

---@param graphic : CS.UnityEngine.UI.Graphic
---@param r : CS.System.Single
---@param g : CS.System.Single
---@param b : CS.System.Single
---@param a : CS.System.Single
---@param duration : CS.System.Single
---@return CS.FAction.GraphicToAction
function CS.FAction.Action.ColorTo(graphic, r, g, b, a, duration)
end