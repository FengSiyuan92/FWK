---@class CS.FModuleInterface
CS.FModuleInterface = {}

---@property readwrite CS.FModuleInterface.STATE : CS.F_MODULE_STATE
CS.FModuleInterface.STATE = nil

---@property readonly CS.FModuleInterface.Name : CS.System.String
CS.FModuleInterface.Name = nil

---@return CS.System.Collections.IEnumerator
function CS.FModuleInterface:OnPrepare()
end

function CS.FModuleInterface:OnInitialize()
end

function CS.FModuleInterface:OnRefresh()
end

function CS.FModuleInterface:OnPause()
end

function CS.FModuleInterface:OnResume()
end

function CS.FModuleInterface:Restart()
end