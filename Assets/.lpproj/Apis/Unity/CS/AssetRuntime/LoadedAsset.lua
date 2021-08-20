---@class CS.AssetRuntime.LoadedAsset : CS.AssetRuntime.Loaded
CS.AssetRuntime.LoadedAsset = {}

---@property readonly CS.AssetRuntime.LoadedAsset.Asset : CS.UnityEngine.Object
CS.AssetRuntime.LoadedAsset.Asset = nil

---@return CS.AssetRuntime.LoadedAsset
function CS.AssetRuntime.LoadedAsset()
end

function CS.AssetRuntime.LoadedAsset:Clear()
end

---@param bundle : CS.AssetRuntime.LoadedBundle
---@param asset : CS.UnityEngine.Object
function CS.AssetRuntime.LoadedAsset:SetInfo(bundle, asset)
end