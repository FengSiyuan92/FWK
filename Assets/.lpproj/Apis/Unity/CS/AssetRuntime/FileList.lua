---@class CS.AssetRuntime.FileList : CS.System.Object
CS.AssetRuntime.FileList = {}

---@param belong : CS.AssetRuntime.FileBelong
---@return CS.AssetRuntime.FileList
function CS.AssetRuntime.FileList(belong)
end

---@param filePath : CS.System.String
---@return CS.System.Boolean
function CS.AssetRuntime.FileList:FillInfoByFilePath(filePath)
end

---@param fileName : CS.System.String
---@return CS.System.Boolean
function CS.AssetRuntime.FileList:ContainsFile(fileName)
end

---@param fileName : CS.System.String
---@return CS.System.String
function CS.AssetRuntime.FileList:TrySearchFilePath(fileName)
end