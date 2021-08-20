---@class CS.FAction.FAction : CS.System.Object
CS.FAction.FAction = {}

---@property readwrite CS.FAction.FAction.IsPause : CS.System.Boolean
CS.FAction.FAction.IsPause = nil

---@property readwrite CS.FAction.FAction.AutoReuse : CS.System.Boolean
CS.FAction.FAction.AutoReuse = nil

---@property readwrite CS.FAction.FAction.IsFinish : CS.System.Boolean
CS.FAction.FAction.IsFinish = nil

---@property readonly CS.FAction.FAction.ActionType : CS.FAction.ActionType
CS.FAction.FAction.ActionType = nil

---@return CS.FAction.FAction
function CS.FAction.FAction()
end

function CS.FAction.FAction:Tick()
end

function CS.FAction.FAction:OnStartTick()
end

function CS.FAction.FAction:Replay()
end

function CS.FAction.FAction:Clear()
end

function CS.FAction.FAction:Destroy()
end

function CS.FAction.FAction:Run()
end

function CS.FAction.FAction:Stop()
end

function CS.FAction.FAction:Pause()
end

function CS.FAction.FAction:Resume()
end