---@class CS.FMonoModule : CS.UnityEngine.MonoBehaviour
CS.FMonoModule = {}

---@property readwrite CS.FMonoModule.STATE : CS.F_MODULE_STATE
CS.FMonoModule.STATE = nil

---@property readonly CS.FMonoModule.Name : CS.System.String
CS.FMonoModule.Name = nil

---@return CS.FMonoModule
function CS.FMonoModule()
end

---@return CS.System.Collections.IEnumerator
function CS.FMonoModule:OnPrepare()
end

function CS.FMonoModule:OnInitialize()
end

function CS.FMonoModule:OnPause()
end

function CS.FMonoModule:OnRefresh()
end

function CS.FMonoModule:OnResume()
end

function CS.FMonoModule:Restart()
end