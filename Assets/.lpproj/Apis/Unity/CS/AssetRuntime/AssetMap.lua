---@class CS.AssetRuntime.AssetMap : CS.System.Object
CS.AssetRuntime.AssetMap = {}

---@return CS.AssetRuntime.AssetMap
function CS.AssetRuntime.AssetMap()
end

---@param assetName : CS.System.String
---@return CS.System.Boolean
function CS.AssetRuntime.AssetMap:ContainsAsset(assetName)
end

---@param assetName : CS.System.String
---@return CS.System.String
function CS.AssetRuntime.AssetMap:GetAssetBundleName(assetName)
end