---@class CS.AssetManager : CS.FMonoModule
CS.AssetManager = {}

---@return CS.AssetManager
function CS.AssetManager()
end

function CS.AssetManager:OnInitialize()
end

function CS.AssetManager:Restart()
end

function CS.AssetManager:OnRefresh()
end

---@return CS.System.Boolean
function CS.AssetManager.HasUnDoneTask()
end

---@param imgAssetName : CS.System.String
---@param onLoad : CS.System.Action
function CS.AssetManager.GetSprite(imgAssetName, onLoad)
end

---@param imgAssetName : CS.System.String
---@param onLoad : CS.System.Action
function CS.AssetManager.GetTexture(imgAssetName, onLoad)
end

---@param prefabName : CS.System.String
---@param onLoad : CS.System.Action
function CS.AssetManager.GetPrefab(prefabName, onLoad)
end

---@param imgAssetName : CS.System.String
function CS.AssetManager.ReturnSprite(imgAssetName)
end

---@param imgAssetName : CS.System.String
function CS.AssetManager.ReturnTexture(imgAssetName)
end

---@param prefabName : CS.System.String
function CS.AssetManager.ReturnPrefab(prefabName)
end