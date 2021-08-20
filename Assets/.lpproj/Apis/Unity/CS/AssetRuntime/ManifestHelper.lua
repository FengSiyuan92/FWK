---@class CS.AssetRuntime.ManifestHelper : CS.System.Object
CS.AssetRuntime.ManifestHelper = {}

---@return CS.AssetRuntime.ManifestHelper
function CS.AssetRuntime.ManifestHelper()
end

---@param version : CS.AssetRuntime.Version
function CS.AssetRuntime.ManifestHelper:ReStart(version)
end

---@param assetBundleName : CS.System.String
---@return CS.System.String[]
function CS.AssetRuntime.ManifestHelper:GetAllDependencies(assetBundleName)
end