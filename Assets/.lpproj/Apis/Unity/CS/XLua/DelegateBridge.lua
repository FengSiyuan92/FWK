---@class CS.XLua.DelegateBridge : CS.XLua.DelegateBridgeBase
CS.XLua.DelegateBridge = {}

---@field public CS.XLua.DelegateBridge.Gen_Flag : CS.System.Boolean
CS.XLua.DelegateBridge.Gen_Flag = nil

---@param reference : CS.System.Int32
---@param luaenv : CS.XLua.LuaEnv
---@return CS.XLua.DelegateBridge
function CS.XLua.DelegateBridge(reference, luaenv)
end

---@param L : CS.System.IntPtr
---@param nArgs : CS.System.Int32
---@param nResults : CS.System.Int32
---@param errFunc : CS.System.Int32
function CS.XLua.DelegateBridge:PCall(L, nArgs, nResults, errFunc)
end

function CS.XLua.DelegateBridge:Action()
end