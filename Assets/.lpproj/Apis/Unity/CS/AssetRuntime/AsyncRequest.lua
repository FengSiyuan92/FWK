---@class CS.AssetRuntime.AsyncRequest : CS.System.Object
CS.AssetRuntime.AsyncRequest = {}

---@property readonly CS.AssetRuntime.AsyncRequest.isDone : CS.System.Boolean
CS.AssetRuntime.AsyncRequest.isDone = nil

---@param id : CS.System.String
---@return CS.System.Boolean
function CS.AssetRuntime.AsyncRequest:IsID(id)
end

function CS.AssetRuntime.AsyncRequest:OnRequestOver()
end

---@param id : CS.System.String
function CS.AssetRuntime.AsyncRequest:AssignID(id)
end

---@param creater : CS.AssetRuntime.LoadedCreater
function CS.AssetRuntime.AsyncRequest:SetLoadedCreater(creater)
end

---@param operation : CS.UnityEngine.AsyncOperation
function CS.AssetRuntime.AsyncRequest:SetAsynOperation(operation)
end

function CS.AssetRuntime.AsyncRequest:Clear()
end