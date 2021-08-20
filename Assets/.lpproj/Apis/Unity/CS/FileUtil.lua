---@class CS.FileUtil : CS.System.Object
CS.FileUtil = {}

---@param inFile : CS.System.String
---@return CS.System.Byte[]
function CS.FileUtil.SafeReadAllBytes(inFile)
end

---@param source : CS.System.String
---@param s : CS.ICSharpCode.SharpZipLib.Zip.ZipOutputStream
function CS.FileUtil.CompressZip(source, s)
end

---@param sourceFile : CS.System.String
---@param targetPath : CS.System.String
---@return CS.System.Boolean
function CS.FileUtil.Decompress(sourceFile, targetPath)
end