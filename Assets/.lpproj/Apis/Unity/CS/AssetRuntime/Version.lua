---@class CS.AssetRuntime.Version : CS.System.Object
CS.AssetRuntime.Version = {}

---@property readonly CS.AssetRuntime.Version.VersionCode : CS.System.String
CS.AssetRuntime.Version.VersionCode = nil

---@return CS.AssetRuntime.Version
function CS.AssetRuntime.Version()
end

---@param newVersion : CS.AssetRuntime.Version
---@return CS.AssetRuntime.UpdateType
function CS.AssetRuntime.Version:Compare(newVersion)
end

---@param newVersion : CS.AssetRuntime.Version
---@return CS.System.Collections.IEnumerator
function CS.AssetRuntime.Version:UpdateTo(newVersion)
end

---@return CS.AssetRuntime.Version
function CS.AssetRuntime.Version.GenServerVersion()
end

---@param fileName : CS.System.String
---@return CS.System.String
function CS.AssetRuntime.Version:GetFilePath(fileName)
end

---@param versionCode : CS.System.String
function CS.AssetRuntime.Version:SetVersionCode(versionCode)
end

---@param versionCode : CS.System.String
---@return CS.AssetRuntime.Version
function CS.AssetRuntime.Version.op_Implicit(versionCode)
end

---@return CS.AssetRuntime.Version
function CS.AssetRuntime.Version.GenClientVersion()
end