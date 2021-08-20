---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 15:48:25
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class Object
local Object = {}

Object.className = "Object"

-- 动态创建 模块
setmetatable(Object, {__index = 
	function(t, k)
		if not Modules.ContainsModule(k) then
			return
		end
	
		local innerModules = t.__modules
		if not innerModules then
			t.__modules = {}
		end
		local module = innerModules[k]
		
		if not module then
			module = Modules.CreateModule(t, k)
			module:OnCreate()
			innerModules[k] = module
		end

		return module
	end})

function Object:OnDestroy()
	if self.__modules then
		for k, module in pairs(self.__modules) do
			module:OnDestroy()
		end
	end
end

return Object