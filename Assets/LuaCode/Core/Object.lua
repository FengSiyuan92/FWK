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

Mod = {
	cs = "cs",
	event = "event",
}

local ModuleType = {
	[Mod.event] = require "Core.Modules.Event.EventModule",
	[Mod.cs] = require "Core.Modules.CSBind.CSBindModule",
}

function Object:OnCreate()
	local initModules = self.Mod
	if initModules then
		for index, moduleName in pairs(initModules) do
			local module = {}
			ModuleType[moduleName].Create(self, module)
			self[moduleName] = module
		end
	end
end

function Object:OnDestroy()
	local initModules = self.Mod
	if initModules then
		for index, moduleName in pairs(initModules) do
			ModuleType[moduleName].Destroy(self[moduleName])
		end
	end
end

return Object