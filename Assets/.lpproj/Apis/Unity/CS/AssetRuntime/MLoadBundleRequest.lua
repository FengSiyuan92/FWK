---@class CS.AssetRuntime.MLoadBundleRequest : CS.AssetRuntime.AsyncRequest
CS.AssetRuntime.MLoadBundleRequest = {}

---@property readonly CS.AssetRuntime.MLoadBundleRequest.assetBundle : CS.UnityEngine.AssetBundle
CS.AssetRuntime.MLoadBundleRequest.assetBundle = nil

---@return CS.AssetRuntime.MLoadBundleRequest
function CS.AssetRuntime.MLoadBundleRequest()
end

---@param operation : CS.UnityEngine.AsyncOperation
function CS.AssetRuntime.MLoadBundleRequest:SetAsynOperation(operation)
end

function CS.AssetRuntime.MLoadBundleRequest:Clear()
end

function CS.AssetRuntime.MLoadBundleRequest:OnRequestOver()
end