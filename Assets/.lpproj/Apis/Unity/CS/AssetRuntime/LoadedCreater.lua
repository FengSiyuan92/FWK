---@class CS.AssetRuntime.LoadedCreater : CS.System.MulticastDelegate
CS.AssetRuntime.LoadedCreater = {}

---@param object : CS.System.Object
---@param method : CS.System.IntPtr
---@return CS.AssetRuntime.LoadedCreater
function CS.AssetRuntime.LoadedCreater(object, method)
end

---@return CS.AssetRuntime.Loaded
function CS.AssetRuntime.LoadedCreater:Invoke()
end

---@param callback : CS.System.AsyncCallback
---@param object : CS.System.Object
---@return CS.System.IAsyncResult
function CS.AssetRuntime.LoadedCreater:BeginInvoke(callback, object)
end

---@param result : CS.System.IAsyncResult
---@return CS.AssetRuntime.Loaded
function CS.AssetRuntime.LoadedCreater:EndInvoke(result)
end