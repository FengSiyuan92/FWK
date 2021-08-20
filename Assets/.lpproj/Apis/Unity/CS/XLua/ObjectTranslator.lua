---@class CS.XLua.ObjectTranslator : CS.System.Object
CS.XLua.ObjectTranslator = {}

---@field public CS.XLua.ObjectTranslator.cacheRef : CS.System.Int32
CS.XLua.ObjectTranslator.cacheRef = nil

---@param luaenv : CS.XLua.LuaEnv
---@param L : CS.System.IntPtr
---@return CS.XLua.ObjectTranslator
function CS.XLua.ObjectTranslator(luaenv, L)
end

---@param type : CS.System.Type
---@param loader : CS.System.Action
function CS.XLua.ObjectTranslator:DelayWrapLoader(type, loader)
end

---@param type : CS.System.Type
---@param creator : CS.System.Func
function CS.XLua.ObjectTranslator:AddInterfaceBridgeCreator(type, creator)
end

---@param L : CS.System.IntPtr
---@param type : CS.System.Type
---@return CS.System.Boolean
function CS.XLua.ObjectTranslator:TryDelayWrapLoader(L, type)
end

---@param type : CS.System.Type
---@param alias : CS.System.String
function CS.XLua.ObjectTranslator:Alias(type, alias)
end

---@param L : CS.System.IntPtr
---@param delegateType : CS.System.Type
---@param idx : CS.System.Int32
---@return CS.System.Object
function CS.XLua.ObjectTranslator:CreateDelegateBridge(L, delegateType, idx)
end

---@return CS.System.Boolean
function CS.XLua.ObjectTranslator:AllDelegateBridgeReleased()
end

---@param L : CS.System.IntPtr
---@param reference : CS.System.Int32
---@param is_delegate : CS.System.Boolean
function CS.XLua.ObjectTranslator:ReleaseLuaBase(L, reference, is_delegate)
end

---@param L : CS.System.IntPtr
---@param interfaceType : CS.System.Type
---@param idx : CS.System.Int32
---@return CS.System.Object
function CS.XLua.ObjectTranslator:CreateInterfaceBridge(L, interfaceType, idx)
end

---@param L : CS.System.IntPtr
function CS.XLua.ObjectTranslator:CreateArrayMetatable(L)
end

---@param L : CS.System.IntPtr
function CS.XLua.ObjectTranslator:CreateDelegateMetatable(L)
end

---@param L : CS.System.IntPtr
function CS.XLua.ObjectTranslator:OpenLib(L)
end

---@param L : CS.System.IntPtr
---@param idx : CS.System.Int32
---@return CS.System.Type
function CS.XLua.ObjectTranslator:GetTypeOf(L, idx)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@param type : CS.System.Type
---@return CS.System.Boolean
function CS.XLua.ObjectTranslator:Assignable(L, index, type)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@param type : CS.System.Type
---@return CS.System.Object
function CS.XLua.ObjectTranslator:GetObject(L, index, type)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@param type : CS.System.Type
---@return CS.System.Array
function CS.XLua.ObjectTranslator:GetParams(L, index, type)
end

---@param L : CS.System.IntPtr
---@param ary : CS.System.Array
function CS.XLua.ObjectTranslator:PushParams(L, ary)
end

---@param L : CS.System.IntPtr
---@param type : CS.System.Type
---@return CS.System.Int32
function CS.XLua.ObjectTranslator:GetTypeId(L, type)
end

---@param L : CS.System.IntPtr
---@param type : CS.System.Type
function CS.XLua.ObjectTranslator:PrivateAccessible(L, type)
end

---@param L : CS.System.IntPtr
---@param o : CS.System.Object
function CS.XLua.ObjectTranslator:PushAny(L, o)
end

---@param L : CS.System.IntPtr
---@param type : CS.System.Type
---@param idx : CS.System.Int32
---@return CS.System.Int32
function CS.XLua.ObjectTranslator:TranslateToEnumToTop(L, type, idx)
end

---@param L : CS.System.IntPtr
---@param o : CS.XLua.LuaDLL.lua_CSFunction
function CS.XLua.ObjectTranslator:Push(L, o)
end

---@param L : CS.System.IntPtr
---@param o : CS.XLua.LuaBase
function CS.XLua.ObjectTranslator:Push(L, o)
end

---@param L : CS.System.IntPtr
---@param o : CS.System.Object
function CS.XLua.ObjectTranslator:Push(L, o)
end

---@param L : CS.System.IntPtr
---@param o : CS.System.Object
---@param type_id : CS.System.Int32
function CS.XLua.ObjectTranslator:PushObject(L, o, type_id)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@param obj : CS.System.Object
function CS.XLua.ObjectTranslator:Update(L, index, obj)
end

---@param type : CS.System.Type
---@return CS.System.Boolean
function CS.XLua.ObjectTranslator:HasCustomOp(type)
end

---@param L : CS.System.IntPtr
---@param val : CS.System.Decimal
function CS.XLua.ObjectTranslator:PushDecimal(L, val)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@return CS.System.Boolean
function CS.XLua.ObjectTranslator:IsDecimal(L, index)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@return CS.System.Decimal
function CS.XLua.ObjectTranslator:GetDecimal(L, index)
end

---@param L : CS.System.IntPtr
---@param index : CS.System.Int32
---@param val : CS.System.Decimal
function CS.XLua.ObjectTranslator:Get(L, index, val)
end