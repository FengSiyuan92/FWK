---@class CS.AssetRuntime.LoadAssetNote : CS.System.Object
CS.AssetRuntime.LoadAssetNote = {}

---@field public CS.AssetRuntime.LoadAssetNote.targetAssetName : CS.System.String
CS.AssetRuntime.LoadAssetNote.targetAssetName = nil

---@field public CS.AssetRuntime.LoadAssetNote.onAssetLoaded : CS.System.Action
CS.AssetRuntime.LoadAssetNote.onAssetLoaded = nil

---@property readonly CS.AssetRuntime.LoadAssetNote.TargetName : CS.System.String
CS.AssetRuntime.LoadAssetNote.TargetName = nil

---@return CS.AssetRuntime.LoadAssetNote
function CS.AssetRuntime.LoadAssetNote()
end

---@param loaded : CS.AssetRuntime.Loaded
function CS.AssetRuntime.LoadAssetNote:RequestOver(loaded)
end

function CS.AssetRuntime.LoadAssetNote:Clear()
end