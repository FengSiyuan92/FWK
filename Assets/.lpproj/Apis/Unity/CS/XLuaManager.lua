---@class CS.XLuaManager : CS.FMonoModule
CS.XLuaManager = {}

---@return CS.XLuaManager
function CS.XLuaManager()
end

---@return CS.System.Collections.IEnumerator
function CS.XLuaManager:OnPrepare()
end

function CS.XLuaManager:Restart()
end

function CS.XLuaManager:OnInitialize()
end

---@param filepath : CS.System.String
---@return CS.System.Byte[]
function CS.XLuaManager.CustomLoader(filepath)
end