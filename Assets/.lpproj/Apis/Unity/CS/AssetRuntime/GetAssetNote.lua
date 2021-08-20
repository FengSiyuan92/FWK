---@class CS.AssetRuntime.GetAssetNote : CS.System.Object
CS.AssetRuntime.GetAssetNote = {}

---@field public CS.AssetRuntime.GetAssetNote.targetAssetName : CS.System.String
CS.AssetRuntime.GetAssetNote.targetAssetName = nil

---@field public CS.AssetRuntime.GetAssetNote.onAssetLoaded : CS.System.Action
CS.AssetRuntime.GetAssetNote.onAssetLoaded = nil

---@field public CS.AssetRuntime.GetAssetNote.loadedBundle : CS.AssetRuntime.LoadedBundle
CS.AssetRuntime.GetAssetNote.loadedBundle = nil

---@property readonly CS.AssetRuntime.GetAssetNote.TargetName : CS.System.String
CS.AssetRuntime.GetAssetNote.TargetName = nil

---@return CS.AssetRuntime.GetAssetNote
function CS.AssetRuntime.GetAssetNote()
end

---@param loaded : CS.AssetRuntime.Loaded
function CS.AssetRuntime.GetAssetNote:RequestOver(loaded)
end

function CS.AssetRuntime.GetAssetNote:Clear()
end