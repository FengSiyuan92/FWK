---@class CS.ActionManager : CS.FMonoModule
CS.ActionManager = {}

---@field public CS.ActionManager.instance : CS.ActionManager
CS.ActionManager.instance = nil

---@return CS.ActionManager
function CS.ActionManager()
end

---@return CS.System.Collections.IEnumerator
function CS.ActionManager:OnPrepare()
end

---@param action : CS.FAction.FAction
function CS.ActionManager.AppendAction(action)
end

---@param action : CS.FAction.FAction
function CS.ActionManager.RemoveAction(action)
end

function CS.ActionManager:OnRefresh()
end