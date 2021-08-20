---@class CS.Invoke : CS.System.Object
CS.Invoke = {}

---@return CS.Invoke
function CS.Invoke()
end

---@param delayTime : CS.System.Single
---@param logic : CS.System.Action
function CS.Invoke.DelayInvoke(delayTime, logic)
end

---@param count : CS.System.Int32
---@param interval : CS.System.Single
---@param logic : CS.System.Action
function CS.Invoke.RepeatInvoke(count, interval, logic)
end