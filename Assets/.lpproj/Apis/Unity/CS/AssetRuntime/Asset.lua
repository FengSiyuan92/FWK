---@module CS.AssetRuntime
CS.AssetRuntime = {}

---@class CS.AssetRuntime.Asset : CS.AssetRuntime.AsyncTemplate
CS.AssetRuntime.Asset = {}

---@return CS.AssetRuntime.Asset
function CS.AssetRuntime.Asset()
end

function CS.AssetRuntime.Asset.Initialize()
end

function CS.AssetRuntime.Asset.ReStart()
end

function CS.AssetRuntime.Asset:ReduceAssetReferenct()
end

---@param assetName : CS.System.String
---@param callback : CS.System.Action
function CS.AssetRuntime.Asset.GetAssetAsync(assetName, callback)
end

---@param assetName : CS.System.String
function CS.AssetRuntime.Asset.ReturnAsset(assetName)
end