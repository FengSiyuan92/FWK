---@class CS.AssetRuntime.CreateRequest : CS.System.MulticastDelegate
CS.AssetRuntime.CreateRequest = {}

---@param object : CS.System.Object
---@param method : CS.System.IntPtr
---@return CS.AssetRuntime.CreateRequest
function CS.AssetRuntime.CreateRequest(object, method)
end

---@param targetName : CS.System.String
---@param note : CS.AssetRuntime.IRequestNote
---@return CS.AssetRuntime.AsyncRequest
function CS.AssetRuntime.CreateRequest:Invoke(targetName, note)
end

---@param targetName : CS.System.String
---@param note : CS.AssetRuntime.IRequestNote
---@param callback : CS.System.AsyncCallback
---@param object : CS.System.Object
---@return CS.System.IAsyncResult
function CS.AssetRuntime.CreateRequest:BeginInvoke(targetName, note, callback, object)
end

---@param result : CS.System.IAsyncResult
---@return CS.AssetRuntime.AsyncRequest
function CS.AssetRuntime.CreateRequest:EndInvoke(result)
end