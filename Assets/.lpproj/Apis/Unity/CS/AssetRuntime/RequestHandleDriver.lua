---@class CS.AssetRuntime.RequestHandleDriver : CS.System.Object
CS.AssetRuntime.RequestHandleDriver = {}

---@property readonly CS.AssetRuntime.RequestHandleDriver.Runing : CS.System.Boolean
CS.AssetRuntime.RequestHandleDriver.Runing = nil

---@return CS.AssetRuntime.RequestHandleDriver
function CS.AssetRuntime.RequestHandleDriver()
end

function CS.AssetRuntime.RequestHandleDriver.Drive()
end

---@param handlerKey : CS.System.String
---@return CS.AssetRuntime.AsyncRequest
function CS.AssetRuntime.RequestHandleDriver.GetRequestHandler(handlerKey)
end

---@param key : CS.System.String
---@param handler : CS.AssetRuntime.AsyncRequest
function CS.AssetRuntime.RequestHandleDriver.RegisterRequestHandler(key, handler)
end