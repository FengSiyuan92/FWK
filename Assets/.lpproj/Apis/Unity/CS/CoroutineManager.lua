---@class CS.CoroutineManager : CS.FMonoModule
CS.CoroutineManager = {}

---@return CS.CoroutineManager
function CS.CoroutineManager()
end

function CS.CoroutineManager:OnInitialize()
end

---@param action : CS.CoroutineAction
---@return CS.Task
function CS.CoroutineManager.StartGlobal(action)
end

---@param agent : CS.UnityEngine.MonoBehaviour
---@param action : CS.CoroutineAction
---@return CS.Task
function CS.CoroutineManager.StartLocal(agent, action)
end