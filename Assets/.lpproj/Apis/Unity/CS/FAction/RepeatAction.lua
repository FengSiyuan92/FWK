---@class CS.FAction.RepeatAction : CS.FAction.GroupAction
CS.FAction.RepeatAction = {}

---@field public CS.FAction.RepeatAction.repeatCount : CS.System.Int32
CS.FAction.RepeatAction.repeatCount = nil

---@property readonly CS.FAction.RepeatAction.ActionType : CS.FAction.ActionType
CS.FAction.RepeatAction.ActionType = nil

---@return CS.FAction.RepeatAction
function CS.FAction.RepeatAction()
end

function CS.FAction.RepeatAction:Tick()
end

function CS.FAction.RepeatAction:Replay()
end

function CS.FAction.RepeatAction:ClearGroupInfo()
end