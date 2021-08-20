---@class CS.FAction.DelayAction : CS.FAction.FAction
CS.FAction.DelayAction = {}

---@field public CS.FAction.DelayAction.delayTime : CS.System.Single
CS.FAction.DelayAction.delayTime = nil

---@property readonly CS.FAction.DelayAction.ActionType : CS.FAction.ActionType
CS.FAction.DelayAction.ActionType = nil

---@return CS.FAction.DelayAction
function CS.FAction.DelayAction()
end

function CS.FAction.DelayAction:Tick()
end

function CS.FAction.DelayAction:Clear()
end

function CS.FAction.DelayAction:Replay()
end