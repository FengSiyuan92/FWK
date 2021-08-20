---@class CS.AssetRuntime.LoadBundleNote : CS.System.Object
CS.AssetRuntime.LoadBundleNote = {}

---@field public CS.AssetRuntime.LoadBundleNote.mainBundleName : CS.System.String
CS.AssetRuntime.LoadBundleNote.mainBundleName = nil

---@field public CS.AssetRuntime.LoadBundleNote.dependCount : CS.System.Int32
CS.AssetRuntime.LoadBundleNote.dependCount = nil

---@field public CS.AssetRuntime.LoadBundleNote.onBundlesLoaded : CS.System.Action
CS.AssetRuntime.LoadBundleNote.onBundlesLoaded = nil

---@property readonly CS.AssetRuntime.LoadBundleNote.TargetName : CS.System.String
CS.AssetRuntime.LoadBundleNote.TargetName = nil

---@return CS.AssetRuntime.LoadBundleNote
function CS.AssetRuntime.LoadBundleNote()
end

---@param loaded : CS.AssetRuntime.Loaded
function CS.AssetRuntime.LoadBundleNote:RequestOver(loaded)
end

function CS.AssetRuntime.LoadBundleNote:Clear()
end