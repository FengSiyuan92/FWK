---@class CS.AssetUtils : CS.System.Object
CS.AssetUtils = {}

---@field public CS.AssetUtils.AppName : CS.System.String
CS.AssetUtils.AppName = nil

---@field public CS.AssetUtils.AssetMapSplit : CS.System.Char
CS.AssetUtils.AssetMapSplit = nil

---@field public CS.AssetUtils.FileDetail : CS.System.String
CS.AssetUtils.FileDetail = nil

---@field public CS.AssetUtils.AssetMap : CS.System.String
CS.AssetUtils.AssetMap = nil

---@field public CS.AssetUtils.VersionFileName : CS.System.String
CS.AssetUtils.VersionFileName = nil

---@field public CS.AssetUtils.LuaFramework : CS.System.String
CS.AssetUtils.LuaFramework = nil

---@field public CS.AssetUtils.LuaScripts : CS.System.String
CS.AssetUtils.LuaScripts = nil

---@property readonly CS.AssetUtils.PersistentPath : CS.System.String
CS.AssetUtils.PersistentPath = nil

---@property readonly CS.AssetUtils.StreamAssetPath : CS.System.String
CS.AssetUtils.StreamAssetPath = nil

---@return CS.AssetUtils
function CS.AssetUtils()
end

---@param source : CS.System.String
---@return CS.System.String
function CS.AssetUtils.GetStringMD5(source)
end

---@param file : CS.System.String
---@return CS.System.String
function CS.AssetUtils.GetFileMD5ByPath(file)
end

---@param bundleName : CS.System.String
function CS.AssetUtils.LogBundleDtExist(bundleName)
end

---@param file : CS.System.String
---@return CS.System.String
function CS.AssetUtils.GetPersistentFilePath(file)
end

---@param file : CS.System.String
---@return CS.System.String
function CS.AssetUtils.GetStreamingFilePath(file)
end

---@param fileName : CS.System.String
---@return CS.System.String
function CS.AssetUtils.GetValidFilePath(fileName)
end

---@param assetName : CS.System.String
---@param type : CS.System.String
function CS.AssetUtils.LogAssetEmpty(assetName, type)
end

---@return CS.System.String
function CS.AssetUtils.GetWerServerBundlePath()
end