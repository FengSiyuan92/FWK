---@class CS.FAction.SequenceAction : CS.FAction.GroupAction
CS.FAction.SequenceAction = {}

---@property readonly CS.FAction.SequenceAction.ActionType : CS.FAction.ActionType
CS.FAction.SequenceAction.ActionType = nil

---@return CS.FAction.SequenceAction
function CS.FAction.SequenceAction()
end

function CS.FAction.SequenceAction:ClearGroupInfo()
end

function CS.FAction.SequenceAction:Replay()
end

function CS.FAction.SequenceAction:Tick()
end