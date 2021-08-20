---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 18:42:52
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class Modules
local Modules = {}


local Module = {
	cs = 1,
	listener = 2,
	asset = 3,
}

function Modules.ContainsModule(key)
	return Module[key] ~= nil
end

function Modules.CreateModule(owner, type)
	local module = {}
	module.owner = owner
	module.type = type
	setmetatable(module, {__index = Module[type]})
	return module
end

return Modules