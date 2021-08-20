---@class CS.AssetRuntime.OnRequestDone : CS.System.MulticastDelegate
CS.AssetRuntime.OnRequestDone = {}

---@param object : CS.System.Object
---@param method : CS.System.IntPtr
---@return CS.AssetRuntime.OnRequestDone
function CS.AssetRuntime.OnRequestDone(object, method)
end

---@param loaded : CS.AssetRuntime.Loaded
function CS.AssetRuntime.OnRequestDone:Invoke(loaded)
end

---@param loaded : CS.AssetRuntime.Loaded
---@param callback : CS.System.AsyncCallback
---@param object : CS.System.Object
---@return CS.System.IAsyncResult
function CS.AssetRuntime.OnRequestDone:BeginInvoke(loaded, callback, object)
end

---@param result : CS.System.IAsyncResult
function CS.AssetRuntime.OnRequestDone:EndInvoke(result)
end