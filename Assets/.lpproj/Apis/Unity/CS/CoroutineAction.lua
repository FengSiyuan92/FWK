---@class CS.CoroutineAction : CS.System.MulticastDelegate
CS.CoroutineAction = {}

---@param object : CS.System.Object
---@param method : CS.System.IntPtr
---@return CS.CoroutineAction
function CS.CoroutineAction(object, method)
end

---@return CS.System.Collections.IEnumerator
function CS.CoroutineAction:Invoke()
end

---@param callback : CS.System.AsyncCallback
---@param object : CS.System.Object
---@return CS.System.IAsyncResult
function CS.CoroutineAction:BeginInvoke(callback, object)
end

---@param result : CS.System.IAsyncResult
---@return CS.System.Collections.IEnumerator
function CS.CoroutineAction:EndInvoke(result)
end