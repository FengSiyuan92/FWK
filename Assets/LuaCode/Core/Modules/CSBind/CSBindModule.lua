---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-23 15:46:00
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class CSBindModule

-- 组件类型映射函数
local genCSFunc = {}
local CSBindMap = require "Core.Modules.CSBind.CSBindMap"
for k, v in pairs(CSBindMap) do
	genCSFunc[k] = function(t)
		return t:GetComponent(v)
	end
end

-- 修改GameObject__index函数

-- 索引函数
local function findGO(t, k)
	local binding = rawget(t, "binding_")
	if not binding then return end
	local node = rawget(t, k)
	local rawGo = binding:GetGameObject(k)
	if not rawGo  then return end
	return rawGo
end

local function init(mod)
	local go = mod.owner_.gameObject
	assert(go, "CS自动绑定模块,需要使用在有gameObject字段的对象中")
	mod.binding_ = go:GetComponent(typeof(CS.LuaBinding))
	assert(mod.binding_, "GameObject对象没有添加LuaBinding脚本")
	mod.init_ = true
end

local CSBindModule = {}

function CSBindModule.Create(owner, mod)
	mod.owner_ = owner
	setmetatable(mod, {__index = function(t, k)
		if not mod.init_ then init(mod) end
		return findGO(mod, k) end
	})
	mod.init_ = false
end

function CSBindModule.Destroy(mod)
	mod.binding_ = nil
end

return CSBindModule