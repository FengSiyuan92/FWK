---@class CS.AssetRuntime.LoadedBundle : CS.AssetRuntime.Loaded
CS.AssetRuntime.LoadedBundle = {}

---@property readonly CS.AssetRuntime.LoadedBundle.Bundle : CS.UnityEngine.AssetBundle
CS.AssetRuntime.LoadedBundle.Bundle = nil

---@return CS.AssetRuntime.LoadedBundle
function CS.AssetRuntime.LoadedBundle()
end

---@param assetbundle : CS.UnityEngine.AssetBundle
function CS.AssetRuntime.LoadedBundle:SetBundle(assetbundle)
end

function CS.AssetRuntime.LoadedBundle:Clear()
end