-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 16:13:49
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class class
local arches = {}
local instanceMap = {}
local _initInstance
local _isDestroyed
local _modifyClassHook

function clearclass(className)
	if not arches[className] then
		return
	end
	
	arches[className] = nil
	local instances = instanceMap[className]
	if instances then
		for k, instance in pairs(instances) do
			instance:Destroy()
		end
	end
	instanceMap[className] = {}
end

function class(className, base)
	base = base or Object
	local arche = arches[className]
	if not arche then
		arche =
		{
			__usedId = 0,
			type = className,
			base = base,
		}

		-- 创建实例弱引用表,用来排查内存泄露
		local instances = {}
		setmetatable(instances, {__mode = "v"})
		instanceMap[className] = instances

		local createInstance = function(...)
			-- 创建实例,并赋予id,绑定原表
			local instance = {}
			arche.__usedId = arche.__usedId + 1
			instance.alive = true
			instance.class = arche
			instance.instanceId =  arche.__usedId
			setmetatable(instance, {__index = arche})
			-- 初始化对象
			_initInstance(instance, arche, ...)
			return instance
		end

		local destroyInstance = function(instance, ...)
			instance.alive = false
			_releaseInstance(instance, arche)
		end

		arche.New = createInstance
		arche.Destroy = destroyInstance
		arche.IsDestroyed = _isDestroyed

		-- 关联父类
		setmetatable(arche, {__index = base})
	end

	return arche
end

function _initInstance(instance, type, ...)
	local super = type.base
	if super then
		_initInstance(instance, super, ...)
	end
	local onCreate = rawget(type, "OnCreate")
	if onCreate then
		onCreate(instance, ...)
	end
end

function _releaseInstance(instance, type, ...)
	local super = type.base
	if super then
		_releaseInstance(instance, super, ...)
	end

	local onDestroy = rawget(type, "OnDestroy")
	if onDestroy then
		onDestroy(instance, ...)
	end
end

function _isDestroyed(instance)
	return not instance.alive
end

function _modifyClassHook()
	error("只能修改实例数据, 不允许动态修改类型数据")
end