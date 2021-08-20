---@class CS.FAction.SyncAction : CS.FAction.GroupAction
CS.FAction.SyncAction = {}

---@property readonly CS.FAction.SyncAction.ActionType : CS.FAction.ActionType
CS.FAction.SyncAction.ActionType = nil

---@return CS.FAction.SyncAction
function CS.FAction.SyncAction()
end

function CS.FAction.SyncAction:ClearGroupInfo()
end

function CS.FAction.SyncAction:Replay()
end

function CS.FAction.SyncAction:Tick()
end