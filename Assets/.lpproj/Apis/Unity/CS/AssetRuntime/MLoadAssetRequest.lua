---@class CS.AssetRuntime.MLoadAssetRequest : CS.AssetRuntime.AsyncRequest
CS.AssetRuntime.MLoadAssetRequest = {}

---@property readonly CS.AssetRuntime.MLoadAssetRequest.Asset : CS.UnityEngine.Object
CS.AssetRuntime.MLoadAssetRequest.Asset = nil

---@return CS.AssetRuntime.MLoadAssetRequest
function CS.AssetRuntime.MLoadAssetRequest()
end

---@param operation : CS.UnityEngine.AsyncOperation
function CS.AssetRuntime.MLoadAssetRequest:SetAsynOperation(operation)
end

function CS.AssetRuntime.MLoadAssetRequest:Clear()
end

function CS.AssetRuntime.MLoadAssetRequest:OnRequestOver()
end