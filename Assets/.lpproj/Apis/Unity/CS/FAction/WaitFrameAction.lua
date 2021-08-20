---@class CS.FAction.WaitFrameAction : CS.FAction.FAction
CS.FAction.WaitFrameAction = {}

---@field public CS.FAction.WaitFrameAction.waitCount : CS.System.Int32
CS.FAction.WaitFrameAction.waitCount = nil

---@property readonly CS.FAction.WaitFrameAction.ActionType : CS.FAction.ActionType
CS.FAction.WaitFrameAction.ActionType = nil

---@return CS.FAction.WaitFrameAction
function CS.FAction.WaitFrameAction()
end

function CS.FAction.WaitFrameAction:Tick()
end

function CS.FAction.WaitFrameAction:Clear()
end

function CS.FAction.WaitFrameAction:Replay()
end