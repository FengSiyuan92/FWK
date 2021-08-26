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
	-- 节点绑定
	cs = "cs",
	-- 事件派发
	event = "event",
	-- 资源存取
	asset = "asset",
	-- 行为托管
	action = "action",
	ui = "ui",
}

local ModuleType = {
	[Mod.event] = require "Core.Modules.Event.EventModule",
	[Mod.cs] = require "Core.Modules.CSBind.CSBindModule",
	[Mod.asset] = require "Core.Modules.Asset.AssetUser",
	[Mod.action] = require "Core.Modules.Action.ActionCollocation",
	[Mod.ui] = require "Core.Modules.UI.UIHelper",
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